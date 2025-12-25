using OmoSeitokuEreceipt.SER;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace OmoEReceLib.ERObjects
{
    // 歯の配列要素は右上８番から左上８番、左下８番から右下８番の順で並ぶ。
    //   8→|→8
    //   8←|←8

    [Serializable]
    public sealed class ER歯式 : IEnumerable<ER歯式単位>, IXmlSerializable, ISerializable, IComparable<ER歯式>, IComparable
    {
        public const int 最大単位数 = 64;

        public static readonly 部位[] 部位Array = new[] { 部位.右側上顎, 部位.左側上顎, 部位.左側下顎, 部位.右側下顎 };

        private readonly Dictionary<部位, List<ER歯式単位>> _dict = new Dictionary<部位, List<ER歯式単位>>
        {
            { 部位.右側上顎, new List<ER歯式単位>() },
            { 部位.左側上顎, new List<ER歯式単位>() },
            { 部位.左側下顎, new List<ER歯式単位>() },
            { 部位.右側下顎, new List<ER歯式単位>() },
        };

        #region Constructor

        public ER歯式()
        {
            this.ERCode = string.Empty;
        }

        public ER歯式(ER歯式 source)
        {
            this.ERCode = string.Empty;
            this.AddByERCode(source.ERCode);
        }

        public ER歯式(ER歯式単位[] collection)
        {
            this.ERCode = string.Empty;
            this.Add(collection);
        }

        // Deserialize Constructor: ISerializable
        public ER歯式(SerializationInfo info, StreamingContext context)
        {
            this.ERCode = string.Empty;

#if DEBUG
            Console.WriteLine(typeof(ER歯式) + " Deserialize Constructor");
#endif

            this.AddByERCode(info.GetString("ERCode"));
        }

        #endregion Constructor

        public static ER歯式 Full
        {
            get
            {
                if (__property_Full == null)
                {
                    var collection = new ER歯式単位[52];
                    for (int i = 0, bui = 1; bui <= 8; bui++)
                    {
                        bool isNyuusi = (5 <= bui);

                        for (int ha = 1; ha <= 8; ha++)
                        {
                            if (isNyuusi && (ha == 6)) break;
                            string hasyu = "10" + bui + ha;

                            collection[i++] = new ER歯式単位(hasyu, ER_状態.現存歯);
                        }
                    }
                    __property_Full = new ER歯式(collection);
                }
                return new ER歯式(__property_Full);
            }
        }

        #region Property Field

        private static ER歯式 __property_Full;

        #endregion Property Field

        // Property

        public int Count
        {
            get
            {
                int ct = 0;
                foreach (var list in _dict.Values) ct += list.Count;
                return ct;
            }
        }

        public string ERCode
        {
            get;
            private set;
        }

        public bool IsEmpty
        {
            get
            {
                foreach (var list in _dict.Values)
                {
                    if (list.Count != 0) return false;
                }
                return true;
            }
        }

        #region Method

        public void Add(IReadOnlyList<ER歯式単位> collection)
        {
            if (最大単位数 < this.Count + collection.Count)
            {
                throw new Exception("歯式の単位数が最大単位数(" + 最大単位数 + ")を超えています。");
            }

            var dict = _dict;
            try
            {
                foreach (var item in collection)
                {
                    dict[item.部位].Add(item);
                }
            }
            catch (KeyNotFoundException e)
            {
                throw new KeyNotFoundException("歯の部位が不正です。", e);
            }

            // SortList
            foreach (var item in dict)
            {
                部位 bui = item.Key;
                List<ER歯式単位> list = item.Value;

                if (1 < list.Count)
                {
                    list.Sort();

                    if ((bui == 部位.右側上顎) || (bui == 部位.右側下顎))
                    {
                        list.Reverse();
                    }
                }
            }

            this.OnChange();
        }

        /// <summary>
        /// erCodeに含まれるすべての歯式単位を追加する。
        /// </summary>
        /// <param name="erCode"></param>
        /// <param name="sisiki"></param>
        public void AddByERCode(string erCode)
        {
            this.Add(ER歯式単位.FromERCodeToArray(erCode));
        }

        public int Count算定ブロック(算定ブロック単位 blockType)
        {
            switch (blockType)
            {
                case 算定ブロック単位.全歯:
                    return Count;

                case 算定ブロック単位.顎:
                    return
                        ((_dict[部位.右側上顎].Count > 0) || (_dict[部位.左側上顎].Count > 0) ? 1 : 0)
                        +
                        ((_dict[部位.左側下顎].Count > 0) || (_dict[部位.右側下顎].Count > 0) ? 1 : 0);

                case 算定ブロック単位.歯区分:
                    HashSet<歯区分> hashSet = new HashSet<歯区分>();
                    foreach (ER歯式単位 ha in this)
                    {
                        hashSet.Add(ha.Get歯区分());
                    }
                    return hashSet.Count;

                default:
                    throw new InvalidOperationException("[error: 632d038c] 未対応の算定ブロック: " + blockType);
            }
        }

        public void Clear()
        {
            foreach (var list in _dict.Values)
            {
                list.Clear();
            }
            this.OnChange();
        }

        //public ER歯式 Clone()
        //{
        //    var result = new ER歯式();
        //    var src = _dict;
        //    var dst = result._dict;

        //    foreach (var keyBui in _buiArray)
        //    {
        //        dst[keyBui].AddRange(src[keyBui]);
        //    }

        //    return result;
        //}
        public void CopyTo(ER歯式単位[] destArray, int arrayIndex)
        {
            if (destArray.Length - arrayIndex < this.Count)
            {
                throw new ArgumentOutOfRangeException("array", "コピー先の配列のサイズが足りません。");
            }

            foreach (var list in _dict.Values)
            {
                list.CopyTo(destArray, arrayIndex);
                arrayIndex += list.Count;
            }
        }

        public int CountBui(部位 bui)
        {
            return _dict[bui].Count;
        }

        /// <summary>
        /// 同一の歯式単位がある場合はtrue
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Contains(ER歯式単位 item)
        {
            return _dict.TryGetValue(item.部位, out List<ER歯式単位> list)
                && list.Contains(item);
        }

        /// <summary>
        /// 指定した歯式のすべての歯式単位が含まれる場合はtrue。
        /// </summary>
        /// <param name="sisiki"></param>
        /// <returns></returns>
        public bool Contains(ER歯式 sisiki)
        {
            var otherDict = sisiki._dict;
            foreach (var myKV in _dict)
            {
                foreach (var otherTanni in otherDict[myKV.Key])
                {
                    // 一つでも含まれない歯式単位があったら false を返す
                    if (!myKV.Value.Contains(otherTanni)) return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 同一の歯種がある場合はtrue。
        /// </summary>
        public bool Contains歯種(string sisyu)
        {
            部位 bui = ER歯式単位.Get部位(sisyu);

            return InternalContains歯種(_dict[bui], sisyu);
        }

        /// <summary>
        /// 同一の歯種がある場合はtrue。
        /// 歯種と状態から比較する。
        /// </summary>
        public bool Contains歯種(string sisyu, ER_状態 joutai)
        {
            部位 bui = ER歯式単位.Get部位(sisyu);

            return InternalContains歯種と状態(_dict[bui], sisyu, joutai);
        }

        /// <summary>
        /// 指定した歯式のすべての歯種が含まれる場合はtrue。
        /// </summary>
        /// <param name="sisiki"></param>
        /// <returns></returns>
        public bool Contains歯種(ER歯式 sisiki)
        {
            var otherDict = sisiki._dict;
            foreach (var myKV in _dict)
            {
                foreach (var otherTanni in otherDict[myKV.Key])
                {
                    // 一つでも含まれない歯種があったら false を返す
                    if (!InternalContains歯種(myKV.Value, otherTanni.歯種)) return false;
                }
            }
            return true;
        }

        public bool Equals(ER歯式 other)
        {
            if (other is null) return false;
            return (ERCode == other.ERCode);
        }

        public bool Equals歯種(ER歯式 other)
        {
            if (other is null) return false;
            if (Count != other.Count) return false;
            // 歯数を比較済みなので一本でも片方に含まれていなければ false を返す。
            var myDict = _dict;
            foreach (var otherKV in other._dict)
            {
                var myList = myDict[otherKV.Key];
                foreach (var otherHa in otherKV.Value)
                {
                    if (!InternalContains歯種(myList, otherHa.歯種)) return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 各部位の歯式単位の配列を表示用の形式に変換して取得。中切歯～大臼歯の順のリストを返す。
        /// </summary>
        /// <param name="syouryaku">省略形式（歯式単位省略記号 を使用した形式）に変換する場合はTrue。省略部分には 歯式単位.Syouryaku が入る。</param>
        /// <returns></returns>
        public Dictionary<部位, IER歯式表示単位[]> Get表示用リスト(bool syouryaku)
        {
            var result = new Dictionary<部位, IER歯式表示単位[]>();

            foreach (var kv in _dict)
            {
                部位 bui = kv.Key;
                List<ER歯式単位> list = kv.Value;

                var dispList = new List<IER歯式表示単位>();
                list.ForEach(
                    item => dispList.Add(item)
                    );

                if (syouryaku)
                {
                    Get表示用リスト_省略(this, bui, ref dispList);
                }

                result.Add(bui, dispList.ToArray());
            }

            return result;
        }

        public ReadOnlyCollection<ER歯式単位> Get歯列By部位(部位 bui)
        {
            return _dict[bui].AsReadOnly();
        }

        /// <summary>
        /// 歯全体に対し現在の歯式に含まれている歯種と含まれていない歯種を反転した歯式を返す。
        /// （歯種のみ）
        /// </summary>
        /// <returns></returns>
        public ER歯式 Inverse()
        {
            var result = new List<ER歯式単位>();
            foreach (var list in Full._dict.Values)
            {
                list.ForEach(
                    ha =>
                    {
                        if (!this.Contains歯種(ha.歯種))
                        {
                            result.Add(ha);
                        }
                    }
                );
            }
            ER歯式 sisiki = new ER歯式
            {
                result
            };
            return sisiki;
        }

        public bool Remove(ER歯式単位 item)
        {
            bool res = _dict[item.部位].Remove(item);
            if (res) this.OnChange();
            return res;
        }

        public override bool Equals(object obj)
        {
            if (object.ReferenceEquals(this, obj)) return true;
            if ((obj != null) && (GetType() == obj.GetType()))
            {
                return (this.ERCode == ((ER歯式)obj).ERCode);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return this.ERCode.GetHashCode();
        }

        public string ToString(bool syouryaku)
        {
            var sb = new StringBuilder(1024);
            var dict = this.Get表示用リスト(syouryaku);

            sb.Append("┘: ");
            foreach (var item in dict[部位.右側上顎])
            {
                sb.Append(item.歯席表示);
            }

            sb.Append("   ");

            sb.Append("└: ");
            foreach (var item in dict[部位.左側上顎])
            {
                sb.Append(item.歯席表示);
            }
            sb.AppendLine();

            sb.Append("┐: ");
            foreach (var item in dict[部位.右側下顎])
            {
                sb.Append(item.歯席表示);
            }

            sb.Append("   ");

            sb.Append("┌: ");
            foreach (var item in dict[部位.左側下顎])
            {
                sb.Append(item.歯席表示);
            }

            return sb.ToString();
        }

        private void OnChange()
        {
            // Update ERCode
            this.ERCode = ToERCode(this);
        }

        #region IEnumerable

        IEnumerator<ER歯式単位> IEnumerable<ER歯式単位>.GetEnumerator()
        {
            var d = _dict;
            foreach (var ha in d[部位.右側上顎]) yield return ha;
            foreach (var ha in d[部位.左側上顎]) yield return ha;
            foreach (var ha in d[部位.左側下顎]) yield return ha;
            foreach (var ha in d[部位.右側下顎]) yield return ha;
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<ER歯式単位>)this).GetEnumerator();
        }

        public IEnumerable<ER歯式単位> GetYaharaEnumerator()
        {
            var d = _dict;
            foreach (var ha in d[部位.右側上顎]) yield return ha;
            foreach (var ha in d[部位.左側上顎]) yield return ha;
            {
                var list = d[部位.左側下顎];
                for (int i = list.Count - 1; i >= 0; i--)
                {
                    yield return list[i];
                }
            }
            {
                var list = d[部位.右側下顎];
                for (int i = list.Count - 1; i >= 0; i--)
                {
                    yield return list[i];
                }
            }
        }

        #endregion IEnumerable

        #region ISerializable

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
#if DEBUG
            Console.WriteLine(typeof(ER歯式) + ".GetObjectData: Serialize");
#endif

            info.AddValue("ERCode", this.ERCode);
        }

        #endregion ISerializable

        #region IXmlSerializable

        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
#if DEBUG
            Console.WriteLine(typeof(ER歯式) + ".ReadXml: Deserialize");
#endif

            reader.ReadStartElement();

            this.Clear();
            this.AddByERCode(reader.ReadString());

            reader.ReadEndElement();
        }

        public void WriteXml(XmlWriter writer)
        {
#if DEBUG
            Console.WriteLine(typeof(ER歯式) + ".WriteXml: Serialize");
#endif

            writer.WriteString(this.ERCode);
        }

        #endregion IXmlSerializable

        // Compare

        public int CompareTo(ER歯式 other)
        {
            CompareHint left = this.GetCompareHint();
            CompareHint right = other.GetCompareHint();

            return left.CompareTo(right);
        }

        public int CompareTo(object obj)
        {
            var sisi = obj as ER歯式;
            if (sisi != null)
                return ((IComparable<ER歯式>)this).CompareTo(sisi);
            return 1;
        }

        /// <summary>
        /// 歯式の右側から左側にかけて比較用の値を取得する。
        /// </summary>
        /// <returns></returns>
        private CompareHint GetCompareHint()
        {
            var list = _dict[部位.右側上顎];
            if (list.Count != 0)
                return new CompareHint(1, list[list.Count - 1]);

            list = _dict[部位.左側上顎];
            if (list.Count != 0)
                return new CompareHint(2, list[0]);

            list = _dict[部位.左側下顎];
            if (list.Count != 0)
                return new CompareHint(3, list[0]);

            list = _dict[部位.右側下顎];
            if (list.Count != 0)
                return new CompareHint(4, list[list.Count - 1]);

            return new CompareHint(CompareHint.EMPTY_HINT, ER歯式単位.Empty);
        }

        #endregion Method

        #region Static Method

        /// <summary>
        /// ERCodeから歯式を作成する。
        /// </summary>
        /// <param name="erCode"></param>
        public static ER歯式 FromERCode(string erCode)
        {
            var sisiki = new ER歯式();
            sisiki.AddByERCode(erCode);
            return sisiki;
        }

        public void ForEach(Action<ER歯式単位> action)
        {
            var d = _dict;
            d[部位.右側上顎].ForEach(action);
            d[部位.左側上顎].ForEach(action);
            d[部位.左側下顎].ForEach(action);
            d[部位.右側下顎].ForEach(action);
        }

        public static string ToERCode(ER歯式 sisiki)
        {
            var sb = new StringBuilder(500);

            sisiki.ForEach(ha => sb.Append(ha.ERCode));

            return sb.ToString();
        }

        private static bool InternalContains歯種と状態(List<ER歯式単位> list, string sisyu, ER_状態 joutai)
        {
            foreach (var item in list)
            {
                if ((sisyu == item.歯種) && (joutai == item.状態)) return true;
            }
            return false;
        }

        private static bool InternalContains歯種(List<ER歯式単位> list, string sisyu)
        {
            foreach (var item in list)
            {
                if (sisyu.Equals(item.歯種)) return true;
            }
            return false;
        }

        private static void Get表示用リスト_省略(ER歯式 sisiki, 部位 bui, ref List<IER歯式表示単位> list)
        {
            ソート済み歯式単位リストを永久歯と乳歯に分割(ref list, out List<IER歯式表示単位> listNyuusi);

            // 永久歯
            InternalGet表示用リスト_省略(sisiki, bui, ref list);

            // 乳歯
            if (listNyuusi != null)
            {
                InternalGet表示用リスト_省略(sisiki, bui, ref listNyuusi);

                list.AddRange(listNyuusi);
            }
        }

        private static void InternalGet表示用リスト_省略(ER歯式 sisiki, 部位 bui, ref List<IER歯式表示単位> list)
        {
            if (list.Count < 2)
            {
                return;
            }

            bool prevIsNext = false;
            bool removed = false;
            bool stop;
            int insertDif;

            for (int i = list.Count - 2; 0 <= i; --i)
            {
                var item = (ER歯式単位)list[i];
                var nextItem = (ER歯式単位)list[i + 1];

                // 両方が現存歯 かつ
                // 次のアイテムが隣接歯の場合。
                if ((item.状態 == ER_状態.現存歯)
                    && (nextItem.状態 == ER_状態.現存歯)
                    && (item.IsNext(nextItem))
                    )
                {
                    if (prevIsNext)
                    {
                        // 前回から隣接フラグが立っているなら歯を削除
                        list.RemoveAt(i + 1);
                        removed = true;
                    }
                    prevIsNext = true;
                    stop = (i == 0);
                    insertDif = 1;
                }
                else
                {
                    stop = true;
                    insertDif = 2;
                }

                bool is乳歯 = false;

                if ((insertDif != 2)
                    && (i == 0)
                    && (item.状態 == ER_状態.現存歯)
                    && (nextItem.状態 == ER_状態.現存歯)
                    && (item.Is中切歯)
                    )
                {
                    is乳歯 = item.Is乳歯;

                    ER歯式単位 target;

                    switch (bui)
                    {
                        case 部位.右側上顎:
                            target = (is乳歯 ? ER歯式単位.左側上顎乳中切歯
                                             : ER歯式単位.左側上顎中切歯);
                            break;

                        case 部位.左側上顎:
                            target = (is乳歯 ? ER歯式単位.右側上顎乳中切歯
                                             : ER歯式単位.右側上顎中切歯);
                            break;

                        case 部位.左側下顎:
                            target = (is乳歯 ? ER歯式単位.右側下顎乳中切歯
                                             : ER歯式単位.右側下顎中切歯);
                            break;

                        case 部位.右側下顎:
                            target = (is乳歯 ? ER歯式単位.左側下顎乳中切歯
                                             : ER歯式単位.左側下顎中切歯);
                            break;

                        default:
                            throw new ApplicationException("不明な部位です。");
                    }

                    if (sisiki.Contains(target))
                    {
                        stop = true;
                        list.RemoveAt(i);
                        removed = true;
                        insertDif = 0;
                    }
                }

                if (stop)
                {
                    if (removed)
                    {
                        // 省略単位を挿入する。
                        var tanni = (is乳歯
                                        ? ER歯式単位.Syouryaku乳歯
                                        : ER歯式単位.Syouryaku);

                        list.Insert(i + insertDif, tanni);
                    }
                    prevIsNext = false;
                    removed = false;
                }
            }
        }

        private static void ソート済み歯式単位リストを永久歯と乳歯に分割(
            ref List<IER歯式表示単位> sourceList, out List<IER歯式表示単位> listNyuusi)
        {
            listNyuusi = null;

            for (int i = 0, len = sourceList.Count; i < len; ++i)
            {
                if (sourceList[i].Is乳歯)
                {
                    listNyuusi = sourceList.GetRange(i, len - i);

                    sourceList.RemoveRange(i, len - i);

                    return;
                }
            }
        }

        #endregion Static Method

        // 演算子オーバーロード

        public static bool operator ==(ER歯式 a, ER歯式 b)
        {
            if (ReferenceEquals(a, b)) return true;
            if ((a is null) || (b is null)) return false;

            return a.ERCode == b.ERCode;
        }

        public static bool operator !=(ER歯式 a, ER歯式 b)
        {
            return !(a == b);
        }

        // Enum, Class

        public enum 部位
        {
            不明 = 0,
            右側上顎 = 1,
            左側上顎 = 2,
            左側下顎 = 3,
            右側下顎 = 4
        }

        public enum 歯区分
        {
            右側上顎臼歯 = 1003,
            上顎前歯 = 1004,
            左側上顎臼歯 = 1005,
            左側下顎臼歯 = 1006,
            下顎前歯 = 1007,
            右側下顎臼歯 = 1008,
        }

        private sealed class CompareHint : IComparable<CompareHint>
        {
            public const int EMPTY_HINT = -1;

            private readonly int _hint;
            private readonly ER歯式単位 _value;

            public CompareHint(int hint, ER歯式単位 value)
            {
                _hint = hint;
                _value = value;
            }

            public int CompareTo(CompareHint other)
            {
                int res = _hint.CompareTo(other._hint);

                if ((res == 0) && (_hint != EMPTY_HINT))
                {
                    res = _value.CompareTo(other._value);
                    if (_value.部位.Is右側()) res = -res;
                }

                return res;
            }
        }
    }

    public static class 部位Extensions
    {
        /// <summary>
        /// 2つの部位が同一または隣接している場合はtrueを返す。
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool IsEqualOrNext(this ER歯式.部位 a, ER歯式.部位 b)
        {
            if (a == b) return true;
            if (a == ER歯式.部位.右側上顎)
            {
                if (b == ER歯式.部位.左側上顎) return true;
            }
            if (a == ER歯式.部位.左側上顎)
            {
                if (b == ER歯式.部位.右側上顎) return true;
            }
            if (a == ER歯式.部位.左側下顎)
            {
                if (b == ER歯式.部位.右側下顎) return true;
            }
            if (a == ER歯式.部位.右側下顎)
            {
                if (b == ER歯式.部位.左側下顎) return true;
            }
            return false;
        }

        public static bool Is右側(this ER歯式.部位 bui)
        {
            return (bui == ER歯式.部位.右側上顎) || (bui == ER歯式.部位.右側下顎);
        }

        public static bool Is上顎(this ER歯式.部位 bui)
        {
            return (bui == ER歯式.部位.右側上顎) || (bui == ER歯式.部位.左側上顎);
        }
    }
}