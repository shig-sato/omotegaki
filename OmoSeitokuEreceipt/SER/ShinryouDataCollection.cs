using OmoSeitoku;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace OmoSeitokuEreceipt.SER
{
    public sealed class ShinryouDataCollection : INotifyCollectionChanged, ICollection<SinryouData>
    {
        public const string P病名_名称 = "P"; //UNDONE P病名名称 ハードコード
        public const string G病名_名称 = "G"; //UNDONE G病名名称 ハードコード
        private static readonly int[] 義歯病名番号配列 = { //UNDONE 義歯病名番号 ハードコード
            8, 9, 22, 23, 24, 75, 77, 90, 109, 111, 115
        };

        private readonly List<SinryouData> _items = new List<SinryouData>();

        #region Constructor

        public ShinryouDataCollection()
        {
        }

        public ShinryouDataCollection(IEnumerable<SinryouData> collection)
        {
            AddRange(collection);
        }

        #endregion


        public event NotifyCollectionChangedEventHandler CollectionChanged = delegate { };


        // Property

        public int Count => _items.Count;
        public SinryouFilter Filter
        {
            get => __property_Filter;
            set
            {
                __property_Filter = value;
                OnCollectionChanged(
                    new NotifyCollectionChangedEventArgs(
                        NotifyCollectionChangedAction.Reset
                    ));
            }
        }
        public bool IsReadOnly => false;
        public Func<SinryouData, IComparable> OrderFunc
        {
            get => __property_OrderFunc;
            set
            {
                if (__property_OrderFunc != value)
                {
                    __property_OrderFunc = value;
                    OnCollectionChanged(
                        new NotifyCollectionChangedEventArgs(
                            NotifyCollectionChangedAction.Reset
                        ));
                }
            }
        }
        public DateRange[] SyosinKikans { get; private set; }

        private SinryouFilter __property_Filter;
        private Func<SinryouData, IComparable> __property_OrderFunc;




        // Method

        public void Add(SinryouData item)
        {
            int index = _items.Count;
            _items.Add(item);
            this.OnCollectionChanged(
                new NotifyCollectionChangedEventArgs(
                    NotifyCollectionChangedAction.Add, item, index
                ));
        }
        public void AddRange(IEnumerable<SinryouData> collection)
        {
            int index = _items.Count;
            _items.AddRange(collection);
            this.OnCollectionChanged(
                new NotifyCollectionChangedEventArgs(
                    NotifyCollectionChangedAction.Add, collection, index
                ));
        }
        public void Clear()
        {
            _items.Clear();
            this.OnCollectionChanged(
                new NotifyCollectionChangedEventArgs(
                    NotifyCollectionChangedAction.Reset
                ));
        }
        public bool Contains(SinryouData item)
        {
            return _items.Contains(item);
        }
        public void CopyTo(SinryouData[] array, int arrayIndex)
        {
            _items.CopyTo(array, arrayIndex);
        }
        public IEnumerator<SinryouData> GetEnumerator()
        {
            return this.GetFiltered().GetEnumerator();
        }
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetFiltered().GetEnumerator();
        }
        public IEnumerable<SinryouData> GetFiltered()
        {
            IEnumerable<SinryouData> items;
            if (this.OrderFunc != null)
                items = _items.OrderBy(this.OrderFunc);
            else
                items = _items;

            if (this.Filter == null)
            {
                foreach (var item in items)
                {
                    yield return item;
                }
                yield break;
            }

            SinryouFilter filter = this.Filter;
            bool pByoInvis = filter.P病名除外;
            bool gByoInvis = filter.G病名除外;
            bool gishiInvis = filter.義歯除外;
            int invisType = filter.除外種別;
            string searchByo = (filter.Has病名 ? filter.病名 : null);
            SinryouFilter.MatchType byoMatch = filter.病名MatchType;
            string searchSyo = (filter.Has処置 ? filter.処置 : null);
            SinryouFilter.MatchType syoMatch = filter.処置MatchType;
            OmoEReceLib.ERObjects.ER歯式 sisiki = (filter.Has歯式 ? filter.歯式 : null);
            bool sisyuKanzen = filter.歯種完全一致;

            foreach (SinryouData shinryouData in items)
            {
                if (invisType == SinryouFilter.除外種別_除外)
                {
                    // P病名・G病名の除外
                    if (pByoInvis || gByoInvis)
                    {
                        string byo = shinryouData.病名.FirstOrDefault(
                                        name => (pByoInvis && (name == P病名_名称)) ||
                                                (gByoInvis && (name == G病名_名称)));

                        if (byo != null)
                        {
                            // skip
                            continue;
                        }
                    }

                    // 義歯の除外
                    if (gishiInvis &&
                        (shinryouData.病名番号.Intersect(義歯病名番号配列).Count() > 0))
                    {
                        // skip
                        continue;
                    }
                }
                else if (invisType == SinryouFilter.除外種別_のみ)
                {
                    if (// いずれかの項目は有効
                        (pByoInvis || gByoInvis || gishiInvis) &&
                        // P病名のみ
                        (!pByoInvis || !shinryouData.病名.Any(name => name == P病名_名称)) &&
                        // G病名のみ
                        (!gByoInvis || !shinryouData.病名.Any(name => name == G病名_名称)) &&
                        // 義歯のみ
                        (!gishiInvis || (shinryouData.病名番号.Intersect(義歯病名番号配列).Count() == 0)))
                    {
                        // skip
                        continue;
                    }
                }

                if (searchByo != null)
                {
                    bool skip = true;

                    foreach (string byoumei in shinryouData.病名)
                    {
                        int pos = byoumei.IndexOf(searchByo, StringComparison.OrdinalIgnoreCase);

                        if (pos < 0) continue;

                        if (((byoMatch == SinryouFilter.MatchType.前方一致) && (pos == 0)) ||
                            (byoMatch == SinryouFilter.MatchType.部分一致) ||
                            ((byoMatch == SinryouFilter.MatchType.完全一致) && (byoumei.Length == searchByo.Length)) ||
                            ((byoMatch == SinryouFilter.MatchType.後方一致) && (pos == byoumei.Length - searchByo.Length)))
                        {
                            skip = false;
                            break;
                        }
                    }

                    if (skip) continue;
                }

                if (searchSyo != null)
                {
                    bool skip = true;

                    foreach (SyochiData shochiData in shinryouData.処置)
                    {
                        string shochiName = shochiData.Name;

                        int pos = shochiName.IndexOf(searchSyo, StringComparison.OrdinalIgnoreCase);

                        if (pos < 0) continue;

                        if (((syoMatch == SinryouFilter.MatchType.前方一致) && (pos == 0)) ||
                            (syoMatch == SinryouFilter.MatchType.部分一致) ||
                            ((syoMatch == SinryouFilter.MatchType.完全一致) && (shochiName.Length == searchSyo.Length)) ||
                            ((syoMatch == SinryouFilter.MatchType.後方一致) && (pos == shochiName.Length - searchSyo.Length)))
                        {
                            skip = false;
                            break;
                        }
                    }

                    if (skip) continue;
                }

                if (sisiki != null)
                {
                    bool skip = sisyuKanzen
                                    // 歯種完全一致
                                    ? !shinryouData.歯式.Equals歯種(sisiki)
                                    // 部分一致
                                    : !shinryouData.歯式.Contains歯種(sisiki);

                    if (skip) continue;
                }

                yield return shinryouData;
            }
        }
        public IEnumerable<SinryouData> GetNonFiltered()
        {
            IEnumerable<SinryouData> items;
            if (this.OrderFunc != null)
                items = _items.OrderBy(this.OrderFunc);
            else
                items = _items;

            foreach (var item in items)
            {
                yield return item;
            }
            yield break;
        }
        public SinryouData[] GetRange(IEnumerable<OmoSeitoku.DateRange> dateRanges)
        {
            return this
                    .Where(p => dateRanges.Any(range => range.Contains(p.診療日)))
                    .ToArray();
        }
        public ReadOnlyCollection<SinryouData> GetRaw()
        {
            return _items.AsReadOnly();
        }
        public bool Remove(SinryouData item)
        {
            int index = _items.IndexOf(item);
            if (index < 0) return false;
            SinryouData deleted = _items[index];
            _items.RemoveAt(index);
            this.OnCollectionChanged(
                new NotifyCollectionChangedEventArgs(
                    NotifyCollectionChangedAction.Remove, deleted, index
                ));
            return true;
        }

        private void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            SyosinKikans = Get初診期間(_items).ToArray();

            this.CollectionChanged(this, e);
        }


        #region Helper

        public static IEnumerable<DateRange> Get初診期間(IEnumerable<SinryouData> sinryouData)
        {
            DateTime? prev初診日 = null;
            DateTime? prevDate = null;

            foreach (var item in sinryouData.OrderBy(p => p.診療日))
            {
                if (item.Is初診日)
                {
                    if (prev初診日.HasValue)
                    {
                        yield return new DateRange(prev初診日.Value, prevDate.Value);
                    }
                    prev初診日 = item.診療日;
                }

                prevDate = item.診療日;
            }

            if (prev初診日.HasValue && prevDate.HasValue)
            {
                yield return new DateRange(prev初診日.Value, prevDate.Value);
            }
        }

        #endregion
    }
}
