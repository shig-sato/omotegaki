using OmoSeitoku;
using OmoSeitokuEreceipt.SER;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace OmoOmotegaki.Models.Converters
{
    internal class YaharaConverter
    {
        sealed class KarteItem
        {
            public readonly KarteId KarteId;
            public readonly string KarteName;
            public readonly SinryouData[] SyochiList;

            public KarteItem(KarteId karteId, string karteName, SinryouData[] syochiList)
            {
                this.KarteId = karteId;
                this.KarteName = karteName;
                this.SyochiList = syochiList;
            }
        }


        /// <param name="shinryoujo"></param>
        /// <param name="karteNumber">設定されている場合、batchSettingsのlimitは無視されて対象のカルテ番号のみ出力する。</param>
        /// <param name="errors"></param>
        /// <param name="limit">0=Max</param>
        /// <exception cref="Exception"></exception>
        public static void Convert(
                Shinryoujo shinryoujo,
                int? karteNumber,
                out List<string> errors,
                int limit = 0
            )
        {
            List<KarteItem> karteList = Load(shinryoujo, karteNumber, out errors, limit);

            // バッチ処理開始
            if (karteList.Count == 0)
            {
                errors.Add("処理できるカルテがありません。");
            }
            else
            {
                foreach (var item in karteList)
                {
                    Console.WriteLine($"[{item.KarteId.KarteNumber}] {item.KarteName}: 処置数{item.SyochiList.Length}");
                }
            }
        }

        private static List<KarteItem> Load(
                Shinryoujo shinryoujo,
                int? karteNumber,
                out List<string> errors,
                int limit = 0
            )
        {
            var karteList = new List<KarteItem>();

            errors = new List<string>();
            // UNDONE 診療録データ ハードコード
            string randaPath = Path.Combine(global::OmoSeitokuEreceipt.Properties.Settings.Default.DataFolder, $"RANDA{shinryoujo.Key}.900");

            using FileStream karteFS = KarteRepository.OpenKarteDataFile(shinryoujo);
            using var karteBR = new BinaryReader(karteFS);
            using FileStream shinryouFS = File.Open(randaPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using var shinryouBR = new BinaryReader(shinryouFS, Encoding.GetEncoding("Shift_JIS"));

            int maxKarteNumber = karteNumber ?? (int)(karteFS.Length / KanjaData.SIZE);
            int prevProgress = 0;

            for (int currentKarteNumber = karteNumber ?? 1; currentKarteNumber <= maxKarteNumber; currentKarteNumber++)
            {
                var karteId = new KarteId(shinryoujo, currentKarteNumber);
                var shinryouLoader = new SinryouDataLoader(karteId, shinryouFS, shinryouBR);
                ShinryouDataCollection shinryouData =
                    shinryouLoader.GetShinryouData(DateRange.All, SinryouDataLoader.診療統合種別.統合なし);

                bool contains = 0 < shinryouData.Count;
                // リストが期間内に含まれるかどうか
                // && shinryouData[0].開始 <= dateRange.Max && // 先頭が最大日以下
                // dateRange.Min <= shinryouData[shinryouData.Count - 1].開始 // 終端が最小日以上

                if (contains)
                {
                    // カルテデータ読み込み
                    {
                        long pos = (karteId.KarteNumber - 1) * KanjaData.SIZE;
                        if (0 > pos || pos >= karteFS.Length)
                        {
                            throw new Exception("患者番号が範囲外です。");
                        }
                        karteFS.Seek(pos, SeekOrigin.Begin);
                    }
                    KarteData karteData = new KarteData(karteBR);

                    //// 患者負担率
                    //if (!KarteRepository.GetKanjaHutanritu(out double hutanRitu, out string hutanErrMsg, karteData, false))
                    //{
                    //    errors.Add(hutanErrMsg);
                    //    //ShowMessage(hutanErrMsg, true, true);

                    //    errors.Add("患者負担率が異常です。カルテ番号 = " + karteId.KarteNumber);

                    //    // キャンセル
                    //    return karteList;
                    //}

                    if (karteData.氏名.StartsWith("No acc")) continue;

                    string karteName = karteData.Get氏名(kana: false);

                    // バッチ処理リストに追加
                    karteList.Add(
                        new KarteItem(
                                karteId: karteId,
                                karteName: karteName,
                                syochiList: shinryouData.ToArray()
                                ));

                    int currentProgress = (int)((double)karteId.KarteNumber / maxKarteNumber * 100d);
                    if (prevProgress < currentProgress)
                    {
                        Console.WriteLine($"{currentProgress} %");
                        prevProgress = currentProgress;
                    }
                }

                if (0 < limit && limit <= karteList.Count) break;
            }

            return karteList;
        }
    }
}
