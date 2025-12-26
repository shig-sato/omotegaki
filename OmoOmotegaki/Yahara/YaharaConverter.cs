#nullable enable

using OmoSeitoku;
using OmoSeitokuEreceipt.SER;
using omotegaki_xml.Libs.Yahara.Converters;
using omotegaki_xml.Libs.Yahara.Entities.KarteEntities;
using omotegaki_xml.Libs.Yahara.Entities.PatDataEntities;
using omotegaki_xml.Libs.Yahara.Entities.PatientEntities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OmoOmotegaki.Yahara
{
    internal sealed class YaharaConverter
    {
        private const bool OUTPUT_FILE = true;


        public sealed class ConverterOption
        {
            /// <summary>
            /// 0 == Max
            /// </summary>
            public readonly int Limit;

            /// <summary>
            /// 設定されている場合、batchSettingsのlimitは無視されて対象のカルテ番号のみ出力する。
            /// </summary>
            public readonly KarteId? OneKarte;

            public readonly DirectoryInfo OutputFolderPath;

            /// <summary>
            /// すべてを変換（本院・分院の両方とも変換）
            /// </summary>
            public ConverterOption(DirectoryInfo outputFolderPath)
            {
                this.Limit = 0;
                this.OneKarte = null;
                this.OutputFolderPath = outputFolderPath;
            }

            /// <summary>
            /// 最大数を指定して変換（本院・分院の両方とも変換）
            /// </summary>
            public ConverterOption(DirectoryInfo outputFolderPath, int limit)
            {
                this.Limit = limit;
                this.OneKarte = null;
                this.OutputFolderPath = outputFolderPath;
            }

            /// <summary>
            /// 個別のカルテを変換
            /// </summary>
            public ConverterOption(DirectoryInfo outputFolderPath, KarteId oneKarte)
            {
                this.Limit = 1;
                this.OneKarte = oneKarte ?? throw new ArgumentNullException(nameof(oneKarte));
                this.OutputFolderPath = outputFolderPath;
            }
        }

        /// <summary>
        /// すべてのカルテを変換
        /// </summary>
        public static async Task ConvertAll(
            ConverterOption option
        )
        {
            // 出力ディレクトリーを削除して作り直し
            if (option.OutputFolderPath.Exists) option.OutputFolderPath.Delete(true);
            option.OutputFolderPath.Create();

            var maxConcurrency = 4;
            using (var semaphore = new SemaphoreSlim(maxConcurrency))
            {
                foreach (var shinryoujo in new[] { new Shinryoujo("Hon"), new Shinryoujo("Bun") })
                {
                    using FileStream zipStream = new FileStream(
                        Path.Combine(option.OutputFolderPath.FullName, GetDirName(shinryoujo) + ".zip"),
                        FileMode.Create);
                    using ZipArchive archive = new ZipArchive(zipStream, ZipArchiveMode.Create);

                    await _ConvertAll(archive, semaphore, shinryoujo, option);
                }
            }

            // ディレクトリーを開く
            using var _ = Process.Start(option.OutputFolderPath.FullName);
        }

        private static async Task _ConvertAll(
            ZipArchive archive,
            SemaphoreSlim semaphore,
            Shinryoujo shinryoujo,
            ConverterOption option
        )
        {
            // 個別のカルテ変換
            if (option.OneKarte != null)
            {
                // 診療所が異なるなら中断
                if (!option.OneKarte.Shinryoujo.Equals(shinryoujo)) return;
            }

            using var karteFS = new KarteFileStream(shinryoujo);
            using var shinryouFS = new ShinryouFileStream(shinryoujo);

            int maxKarteNumber = karteFS.MaxKarteNumber;
            int prevProgress = 0;
            int limitCounter = 0;

            Console.WriteLine($"変換開始: 診療所={shinryoujo.Key}, 最大カルテ番号={maxKarteNumber}");

            for (int currentKarteNumber = option.OneKarte?.KarteNumber ?? 1;
                currentKarteNumber <= maxKarteNumber;
                currentKarteNumber++)
            {
                var karteId = new KarteId(shinryoujo, currentKarteNumber);

                // 診療データ読み込み
                var shinryouLoader = shinryouFS.GetSinryouDataLoader(karteId);
                ShinryouDataCollection shinryouData =
                    shinryouLoader.GetShinryouData(DateRange.All, SinryouDataLoader.診療統合種別.統合なし);

                // 診療データがない場合はスキップ
                if (0 == shinryouData.Count) continue;

                // カルテデータ読み込み
                KarteData karteData = karteFS.GetKarteData(karteId.KarteNumber);

                // 患者氏名が未指定ならスキップ
                if (string.IsNullOrWhiteSpace(karteData.氏名) || karteData.氏名.StartsWith("No acc")) continue;

                // 個別のカルテを変換
                await ConvertOne(archive, semaphore, option, karteId, karteData, shinryouData);

                // 進捗処理
                int currentProgress = (int)((double)karteId.KarteNumber / maxKarteNumber * 100d);
                if (prevProgress < currentProgress)
                {
                    Console.WriteLine($"診療所:{shinryoujo.Key}, {currentProgress} %");
                    prevProgress = currentProgress;
                }

                if (0 < option.Limit && option.Limit <= ++limitCounter) break;
            }
        }

        /// <summary>
        /// 個別のカルテを変換
        /// </summary>
        private static async Task ConvertOne(
            ZipArchive archive,
            SemaphoreSlim semaphore,
            ConverterOption option,
            KarteId karteId,
            KarteData karteData,
            ShinryouDataCollection shinryouData
            )
        {
            //// 患者負担率
            //if (!KarteRepository.GetKanjaHutanritu(out double hutanRitu, out string hutanErrMsg, karteData, false))
            //{
            //    errors.Add(hutanErrMsg);
            //    //ShowMessage(hutanErrMsg, true, true);

            //    errors.Add("患者負担率が異常です。カルテ番号 = " + karteId.KarteNumber);

            //    // キャンセル
            //    return karteList;
            //}

            //string karteName = karteData.Get氏名(kana: false);

            //Console.WriteLine($"{karteId}: {karteName} ({shinryouData.Count})");


            // XMLシリアライゼーション
            Patient patient = KarteToPatientConverter.Convert(karteId, karteData);
            //PatData patData = KarteToPatDataConverter.Convert(karteId, karteData);
            Karte karte = KarteDataToKarteConverter.Convert(karteId, karteData);

            if (OUTPUT_FILE)
            {
                await OutputXmlFile(archive, semaphore, option, karteId, patient, karte);
            }
        }

        private static string GetDirName(Shinryoujo shinryoujo)
        {
            return shinryoujo.Key == "Hon" ? "1_本院" : "2_分院";
        }

        /// <summary>
        /// 個別のカルテXMLを出力
        /// </summary>
        private static async Task OutputXmlFile(
            ZipArchive archive,
            SemaphoreSlim semaphore,
            ConverterOption option,
            KarteId karteId,
            Patient patient,
            Karte karte
            )
        {
            // 個別の患者ディレクトリーを作成
            string kanjaDirName = $@"{GetDirName(karteId.Shinryoujo)}/{karteId.KarteNumber:D5}";

            try
            {
                await semaphore.WaitAsync();

                // A: 患者情報-α (Patient.xml)
                {
                    var entry = archive.CreateEntry(kanjaDirName + "/Patient.xml", CompressionLevel.Fastest);
                    using var writer = new StreamWriter(entry.Open(), Encoding.UTF8);
                    await writer.WriteAsync(patient.ToXML());
                }

                // A: 患者情報-β (PatData.csv)
                {
                }

                // B: カルテ情報 (Karte.xml)
                {
                    var entry = archive.CreateEntry(kanjaDirName + "/Karte.xml", CompressionLevel.Fastest);
                    using var writer = new StreamWriter(entry.Open(), Encoding.UTF8);
                    await writer.WriteAsync(karte.ToXML());
                }

                // C: 欠損情報 (OralInformation.xml)
                {
                }
            }
            catch (Exception err)
            {
                var msg = $"[e37469bb-ccc7-4ab3-a0f1-47fbe81cf7cf] KarteId: {karteId}, XML出力でエラー: {err.Message}";
                Console.WriteLine(msg);
                //logger.Log(msg);
            }
            finally
            {
                semaphore.Release();
            }
        }
    }
}
