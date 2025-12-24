using OmoSeitoku;
using OmoSeitokuEreceipt.SER;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace OmoOmotegaki.Models.Converters
{
    internal sealed class YaharaConverter
    {
        public sealed class ConverterOption
        {
            /// <summary>
            /// 0 == Max
            /// </summary>
            public readonly int Limit;

            /// <summary>
            /// 設定されている場合、batchSettingsのlimitは無視されて対象のカルテ番号のみ出力する。
            /// </summary>
            public readonly KarteId OneKarte;

            /// <summary>
            /// すべてを変換（本院・分院の両方とも変換）
            /// </summary>
            public ConverterOption()
            {
                this.Limit = 0;
                this.OneKarte = null;
            }

            /// <summary>
            /// 最大数を指定して変換（本院・分院の両方とも変換）
            /// </summary>
            public ConverterOption(int limit)
            {
                this.Limit = limit;
                this.OneKarte = null;
            }

            /// <summary>
            /// 個別のカルテを変換
            /// </summary>
            public ConverterOption(KarteId oneKarte)
            {
                this.Limit = 0;
                this.OneKarte = oneKarte ?? throw new ArgumentNullException(nameof(oneKarte));
            }
        }

        /// <summary>
        /// すべてのカルテを変換
        /// </summary>
        public static void ConvertAll(
                ConverterOption option,
                out List<string> errors
            )
        {
            errors = new List<string>();

            ConvertAll(new Shinryoujo("Hon"), option, errors);
            ConvertAll(new Shinryoujo("Bun"), option, errors);
        }

        private static void ConvertAll(
                Shinryoujo shinryoujo,
                ConverterOption option,
                List<string> errors
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

            int maxKarteNumber = option.OneKarte?.KarteNumber ?? karteFS.MaxKarteNumber;
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
                if (karteData.氏名.StartsWith("No acc")) continue;

                // 個別のカルテを変換
                ConvertKarte(karteId, karteData, shinryouData);

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
        private static void ConvertKarte(
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

            string karteName = karteData.Get氏名(kana: false);

            Console.WriteLine($"{karteId}: {karteName} ({shinryouData.Count})");
        }
    }
}
