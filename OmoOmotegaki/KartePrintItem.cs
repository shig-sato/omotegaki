using OmoSeitokuEreceipt.SER;
using System;
using System.Collections.Generic;

namespace OmoOmotegaki
{
    public sealed class KartePrintItem
    {
        public readonly KarteId KarteId;
        public readonly string KarteName;
        public readonly SinryouData[] SyochiList;
        public readonly double HutanWariai;

        public KartePrintItem(KarteId karteId, string karteName, SinryouData[] syochiList, double hutanWariai)
        {
            this.KarteId = karteId;
            this.KarteName = karteName;
            this.SyochiList = syochiList;
            this.HutanWariai = hutanWariai;
        }

        /// <summary>
        /// 徴収金額計算
        /// </summary>
        public int CalcHutanKingaku(int tensuu)
        {
            // 金額 = 点数 * 10 * 負担割合　（1の位を四捨五入）

            double res = tensuu * this.HutanWariai;
            return (int)Math.Round(res, MidpointRounding.AwayFromZero) * 10;
        }
    }

    public sealed class KartePrintItemList : List<KartePrintItem>
    {
        public enum SortType
        {
            カルテ番号,
            最古診療日,
            最新診療日
        }

        public struct SortSettings
        {
            public static readonly SortSettings Default =
                                    new SortSettings(SortType.カルテ番号, false);

            public SortType SortType;
            public bool OrderDesc;

            public SortSettings(SortType sortType, bool orderDesc)
            {
                this.SortType = sortType;
                this.OrderDesc = orderDesc;
            }
        }



        private bool _orderDesc;

        public void Sort(SortSettings sortSettings)
        {
            _orderDesc = sortSettings.OrderDesc;

            switch (sortSettings.SortType)
            {
                case SortType.カルテ番号:
                    this.Sort(this.SortComparisonByKarteNumber);
                    break;
                case SortType.最古診療日:
                    this.Sort(this.SortComparisonByMinDate);
                    break;
                case SortType.最新診療日:
                    this.Sort(this.SortComparisonByMaxDate);
                    break;
            }
        }

        // 並び替え用メソッド
        // ここから

        public int SortComparisonByMinDate(KartePrintItem a, KartePrintItem b)
        {
            SinryouData[] listA = a.SyochiList;
            SinryouData[] listB = b.SyochiList;

            if (listA == null || listA.Length == 0) return 0;
            if (listB == null || listB.Length == 0) return 0;

            return listA[0].診療日.CompareTo(listB[0].診療日) * (_orderDesc ? -1 : 1);
        }
        public int SortComparisonByMaxDate(KartePrintItem a, KartePrintItem b)
        {
            SinryouData[] listA = a.SyochiList;
            SinryouData[] listB = b.SyochiList;

            if (listA == null || listA.Length == 0) return 0;
            if (listB == null || listB.Length == 0) return 0;

            return listA[listA.Length - 1].診療日.CompareTo(
                                                listB[listB.Length - 1].診療日) * (_orderDesc ? -1 : 1);
        }
        public int SortComparisonByKarteNumber(KartePrintItem a, KartePrintItem b)
        {
            if (_orderDesc)
                return -a.KarteId.CompareTo(b.KarteId);
            else
                return a.KarteId.CompareTo(b.KarteId);
        }

        // ここまで
    }
}
