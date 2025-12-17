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
        public static void Convert(
                Shinryoujo shinryoujo,
                KarteBatchSettings batchSettings,
                SinryouDataLoader.診療統合種別 sinryouTougou,
                out List<string> errors
            )
        {
            // 対象期間
            DateRange dateRange = batchSettings.DateRange.Range.ToDateRange();
            // 処理数
            int limit = batchSettings.PreviewLimit > 0
                            ? batchSettings.PreviewLimit
                            : 0; // 0=Max

            // UNDONE 診療録データ ハードコード
            string randaPath = Path.Combine(global::OmoSeitokuEreceipt.Properties.Settings.Default.DataFolder, $"RANDA{shinryoujo.Key}.900");

            bool expandToSyosinbi = batchSettings.DateRange.ExpandToSyosinbi;
            bool expandToLastDate = batchSettings.DateRange.ExpandToLastDate;

            errors = new List<string>();

            var items = new KartePrintItemList();

            using (FileStream karteFS = KarteRepository.OpenKarteDataFile(shinryoujo))
            using (var karteBR = new BinaryReader(karteFS))
            using (FileStream fs = File.Open(randaPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (var br = new BinaryReader(fs, Encoding.GetEncoding("Shift_JIS")))
            {
                int maxKarteNumber = (int)(fs.Length / KanjaData.SIZE);

                for (int karteNumber = 1; karteNumber <= maxKarteNumber; karteNumber++)
                {
                    var karteId = new KarteId(shinryoujo, karteNumber);
                    
                    var loader = new SinryouDataLoader(karteId, fs, br);
                     
                    DateRange range = loader.ConvertSinryouDateRange(dateRange, expandToSyosinbi, expandToLastDate);
                    ShinryouDataCollection sinryouData = loader.GetShinryouData(range, sinryouTougou);

                    // リストが期間内に含まれるかどうか
                    if (0 < sinryouData.Count
                        //&& sinryouData[0].開始 <= dateRange.Max && // 先頭が最大日以下
                        //dateRange.Min <= sinryouData[sinryouData.Count - 1].開始 // 終端が最小日以上
                        )
                    {
                        // カルテデータ読み込み
                        long pos = (karteId.KarteNumber - 1) * KanjaData.SIZE;
                        if (0 > pos || pos >= fs.Length)
                        {
                            throw new Exception("患者番号が範囲外です。");
                        }

                        fs.Seek(pos, SeekOrigin.Begin);

                        KarteData karteData = new KarteData(karteBR);

                        // 患者負担率
                        if (!KarteRepository.GetKanjaHutanritu(out double hutanRitu, out string hutanErrMsg, karteData, false))
                        {
                            errors.Add(hutanErrMsg);
                            //ShowMessage(hutanErrMsg, true, true);

                            errors.Add("患者負担率が異常です。カルテ番号 = " + karteId.KarteNumber);

                            // キャンセル
                            return;
                        }

                        // バッチ処理リストに追加
                        items.Add(
                            new KartePrintItem(
                                    karteId: karteId,
                                    karteName: karteData.Get氏名(kana: false),
                                    syochiList: sinryouData.ToArray(),
                                    hutanWariai: hutanRitu
                                    ));
                    }

                    if (0 < limit && limit <= items.Count) break;
                }
            }

            // バッチ処理開始
            if (items.Count == 0)
            {
                errors.Add("処理できるカルテがありません。");
            }
            else
            {
                items.Sort(batchSettings.SortSettings);

                foreach (var item in items)
                {
                    Console.WriteLine(item.KarteName + ": " + item.SyochiList.Length);
                }
            }
        }
    }
}
