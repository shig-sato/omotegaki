using OmoSeitoku;
using OmoSeitokuEreceipt.SER;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OmoOmotegaki.Controls
{
    public sealed class ShinryouListViewGroupSource : ShinryouListViewNode, IEnumerable<ShinryouListViewItemSource>
    {
        private readonly List<ShinryouListViewItemSource> _items;

        #region Constructor

        private ShinryouListViewGroupSource(DateRange2 kikan)
            : base(null, -1)
        {
            _items = new List<ShinryouListViewItemSource>();
            Kikan = kikan;
        }

        private ShinryouListViewGroupSource()
            : base(null, -1)
        {
            _items = new List<ShinryouListViewItemSource>();
            Kikan = DateRange2.Infinite;
        }

        #endregion

        #region Property

        public override bool IsSelected
        {
            get => false;
            set { }
        }

        public string KikanText => __property_KikanText ??= CreateKikanText(this.Kikan);
        private string __property_KikanText;

        public IEnumerable<ShinryouListViewItemSource> Items => _items;

        public DateRange2 Kikan { get; }

        #endregion


        #region Static Method

        internal static IEnumerable<ShinryouListViewGroupSource> CreateGroups(ShinryouDataCollection collection)
        {
            if (collection is null) throw new ArgumentNullException(nameof(collection));

            var groups = new List<ShinryouListViewGroupSource>();

            if (collection.OrderFunc != null)
            {
                // 日付順以外の場合は単一のグループに入れて返す。

                var group = new ShinryouListViewGroupSource();
                group.AddRange(
                    collection.Select(sinryouData =>
                        new ShinryouListViewItemSource(group, sinryouData)));
                groups.Add(group);
            }
            else
            {
                SinryouData firstData = collection.FirstOrDefault();
                if (firstData != null)
                {
                    DateRange firstSyosinKikan = collection.SyosinKikans.FirstOrDefault();
                    if (firstData.診療日 < firstSyosinKikan.Min)
                    {
                        groups.Add(
                            new ShinryouListViewGroupSource(new DateRange2(null, null)));
                    }

                    foreach (DateRange syosinKikan in collection.SyosinKikans)
                    {
                        groups.Add(
                            new ShinryouListViewGroupSource(new DateRange2(syosinKikan)));
                    }

                    foreach (var sinryouData in collection)
                    {
                        foreach (ShinryouListViewGroupSource group in groups)
                        {
                            if (group.Kikan.Contains(sinryouData.診療日))
                            {
                                group.Add(
                                    new ShinryouListViewItemSource(group, sinryouData));
                                break;
                            }
                        }
                    }
                }
            }
            return groups;
        }

        private static string CreateKikanText(DateRange2 kikan)
        {
            return string.Concat(
                kikan.IsInfiniteMin ? string.Empty : kikan.Min.Value.ToShortDateString(),
                " ～ ",
                kikan.IsInfiniteMax ? string.Empty : kikan.Max.Value.ToShortDateString());
        }

        #endregion

        #region Method

        public void Add(ShinryouListViewItemSource item)
        {
            if (item.Parent != this)
                throw new Exception("[error: edabf34b] Parent が正しく設定されていません。");

            _items.Add(item);
        }
        public void AddRange(IEnumerable<ShinryouListViewItemSource> collection)
        {
            if (collection.Any(item => item.Parent != this))
                throw new Exception("[error: 849a58f1] Parent が正しく設定されていません。");

            _items.AddRange(collection);
        }
        public IEnumerator<ShinryouListViewItemSource> GetEnumerator()
        {
            return Items.GetEnumerator();
        }
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}
