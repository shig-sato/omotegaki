using OmoSeitoku.VB;
using System;
using System.Collections.Generic;
using System.IO;

namespace OmoSeitokuEreceipt.SER
{
    /// <summary>
    /// 診療録ソフト　カルテ編集日データ
    ///
    /// (RgsKrDy.***) カルテ番号と編集日編集日の昇順で記録されているVBバイナリファイル
    /// </summary>
    public sealed class KartePrintInfo
    {
        private readonly Shinryoujo _shinryoujo;

        public KartePrintInfo(Shinryoujo shinryoujo)
        {
            _shinryoujo = shinryoujo;
        }

        public KartePrintInfoItem[] Items;

        //private delegate int ReadInt(BinaryReader reader);
        //private delegate DateTime? ReadDate(BinaryReader reader);

        public void Read(StreamReader input, DateTime date)
        {
            var items = new List<KartePrintInfoItem>();

            int i = 0;
            while (!input.EndOfStream)
            {
                string line = input.ReadLine();

                if (0 < i)
                {
                    var split = line.Split(',');
                    if (split.Length == 2)
                    {
                        string path = split[0].Replace("\"", "");
                        int karteNumber = int.Parse(split[1]);
                        KarteId karteId = (karteNumber <= 0) ? null : new KarteId(_shinryoujo, karteNumber);
                        var item = new KartePrintInfoItem(karteId, date, 0);

                        items.Add(item);
                    }
                }

                ++i;
            }



            //public void Read(StreamReader input, DateRange range)
            //{

            //List<KartePrintInfoItem> items = new List<KartePrintInfoItem>();

            //HashSet<int> addedKarteNumbers = new HashSet<int>();
            //Stream st = input.BaseStream;
            //ReadInt readInt = VBRandomFile.ReadInt;
            //ReadDate readDate = VBRandomFile.ReadDate;

            //// ファイルの末尾から読み込む (カルテ番号は重複させない為、最新の日付の情報を優先)

            //st.Seek(0, SeekOrigin.End);

            //long pos = st.Position;

            //if (0 < pos)
            //{
            //    while (true)
            //    {
            //        pos -= KartePrintInfoItem.SIZE;

            //        if (pos < 0) break;

            //        st.Seek(pos, SeekOrigin.Begin);

            //        KartePrintInfoItem item = new KartePrintInfoItem(
            //            readInt(input), readDate(input), readInt(input)
            //        );

            //        if (!addedKarteNumbers.Contains(item.KarteNumber) &&
            //            item.Date.HasValue && range.Contains( item.Date.Value ))
            //        {
            //            items.Add(item);
            //            addedKarteNumbers.Add(item.KarteNumber);
            //        }
            //    }
            //}

            //items.Reverse();

            this.Items = items.ToArray();
        }
    }

    public struct KartePrintInfoItem
    {
        public const long SIZE = VBRandomFile.SIZE_INT + VBRandomFile.SIZE_DATE + VBRandomFile.SIZE_INT;

        public KarteId KarteId;
        /// <summary>
        /// 最終記録日　（行の日付とは異なる）
        /// </summary>
        public DateTime? Date;
        public int Etc;


        public KartePrintInfoItem(KarteId karteId, DateTime? dt, int etc)
        {
            this.KarteId = karteId;
            this.Date = dt;
            this.Etc = etc;
        }

        public override string ToString()
        {
            return string.Format("{0}, {1}, {2}",
                                this.KarteId,
                                this.Date,
                                this.Etc
                                );
        }
    }
}