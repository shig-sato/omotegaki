using System;
using System.Collections.Generic;

namespace OmoEReceLib.ERObjects
{
    /// <summary>
    /// 歯式の構成要素
    /// </summary>
    /// <example>
    /// ER歯式単位 t = 歯式単位("1004", ER_状態.現存歯, ER_部分.部分指定なし);
    /// </example>
    /// <example>
    /// ER歯式単位 t = 歯式単位.FromERCode("100400");
    /// </example>
    /// /// <seealso cref="マスターファイル/歯式マスター"/>
    [Serializable]
    public struct ER歯式単位 : IER歯式表示単位, IComparable<ER歯式単位>, IComparable
    {
        #region Const

        internal const int 歯種_文字数 = 4;
        internal const int 歯式単位_文字数 = 歯種_文字数 + 2;

        private const string 歯種_部位_右側上顎 = "101";
        private const string 歯種_部位_左側上顎 = "102";
        private const string 歯種_部位_左側下顎 = "103";
        private const string 歯種_部位_右側下顎 = "104";
        private const string 歯種_部位_右側上顎乳 = "105";
        private const string 歯種_部位_左側上顎乳 = "106";
        private const string 歯種_部位_左側下顎乳 = "107";
        private const string 歯種_部位_右側下顎乳 = "108";

        #endregion


        #region Static Field

        public static readonly ER歯式単位 Empty = new ER歯式単位();

        public static readonly ER歯式省略単位 Syouryaku = new ER歯式省略単位(isNyuusi: false);
        public static readonly ER歯式省略単位 Syouryaku乳歯 = new ER歯式省略単位(isNyuusi: true);

        public static readonly ER歯式単位 右側上顎中切歯 = new ER歯式単位 { 歯種 = ER歯種s.i["右側上顎中切歯"] };
        public static readonly ER歯式単位 左側上顎中切歯 = new ER歯式単位 { 歯種 = ER歯種s.i["左側上顎中切歯"] };
        public static readonly ER歯式単位 左側下顎中切歯 = new ER歯式単位 { 歯種 = ER歯種s.i["左側下顎中切歯"] };
        public static readonly ER歯式単位 右側下顎中切歯 = new ER歯式単位 { 歯種 = ER歯種s.i["右側下顎中切歯"] };

        public static readonly ER歯式単位 右側上顎乳中切歯 = new ER歯式単位 { 歯種 = ER歯種s.i["右側上顎乳中切歯"] };
        public static readonly ER歯式単位 左側上顎乳中切歯 = new ER歯式単位 { 歯種 = ER歯種s.i["左側上顎乳中切歯"] };
        public static readonly ER歯式単位 左側下顎乳中切歯 = new ER歯式単位 { 歯種 = ER歯種s.i["左側下顎乳中切歯"] };
        public static readonly ER歯式単位 右側下顎乳中切歯 = new ER歯式単位 { 歯種 = ER歯種s.i["右側下顎乳中切歯"] };

        public static readonly string[] 歯席番号Array = new[]{
            "1", "2", "3", "4", "5", "6", "7", "8",
            "A", "B", "C", "D", "E",
        };

        #endregion


        // Property

        /// <see cref="ER歯種s"/>
        public string 歯種
        {
            get { return __property__歯種; }
            private set
            {
                if (value.Length != 歯種_文字数)
                {
                    throw new ApplicationException(
                        "歯種に長さ" + 歯種_文字数 + "以外の文字列が指定されました。");
                }

                __property__歯種 = value;

                this.部位 = Get部位(value);
                this.Is乳歯 = 歯種Is乳歯(value);
                this.Is中切歯 = value[3] == '1';
            }
        }
        public ER_状態 状態
        {
            get;
            private set;
        }
        public ER_部分 部分
        {
            get;
            private set;
        }
        public string ERCode
        {
            get
            {
                string code = __property__ERCode;
                if (code == null)
                {
                    code = ToERCode(this);
                    __property__ERCode = code;
                }
                return code;
            }
            private set
            {
                __property__ERCode = value;
            }
        }
        public ER歯式.部位 部位
        {
            get;
            private set;
        }
        public bool Is中切歯
        {
            get;
            private set;
        }
        public bool Is乳歯
        {
            get;
            private set;
        }

        // Only Get

        public string 歯席表示
        {
            get
            {
                return Get歯席番号(this.歯種, this.Is乳歯);
            }
        }
        public string 歯席
        {
            get
            {
                return (this.部位 == ER歯式.部位.不明)
                            ? this.歯席表示
                            : this.部位 + this.歯席表示;
            }
        }

        #region Property Field

        private string __property__歯種;
        private string __property__ERCode;

        #endregion


        // Constructor

        public ER歯式単位(string shisyu, ER_状態 joutai = ER_状態.現存歯, ER_部分 bubun = ER_部分.部分指定なし)
            : this()
        {
            this.歯種 = shisyu;
            this.状態 = joutai;
            this.部分 = bubun;
        }


        // Method

        /// <summary>
        /// 指定した歯式単位が隣接している場合はtrue。
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public bool IsNext(ER歯式単位 target)
        {
            // 部位が同じ側で、歯種コードの4文字目が隣り合う数字の場合は隣接している。
            if (IsEqualOrNext(target.部位, this.部位))
            {
                if (this.歯種[3] - 1 == target.歯種[3]) return true;
                if (this.歯種[3] == target.歯種[3] - 1) return true;
            }
            return false;
        }
        public override bool Equals(object obj)
        {
            return (obj is ER歯式単位) && (this == (ER歯式単位)obj);
        }
        public override int GetHashCode()
        {
            return this.ERCode.GetHashCode();
        }
        public override string ToString()
        {
            return "{ 歯種=" + this.歯種 + ", 状態=" + this.状態 + ", 部分=" + this.部分 + " }";
        }
        public int CompareTo(ER歯式単位 other)
        {
            if (this.Is乳歯)
            {
                if (!other.Is乳歯)
                {
                    return -1; // 自身が乳歯で相手が永久歯の場合、自身は小さい
                }
            }
            else if (other.Is乳歯)
            {
                return 1; // 自身が永久歯で相手が乳歯の場合、自身は大きい
            }

            int res = this.ERCode.CompareTo(other.ERCode);

            // 歯種 が同一で ER_状態.部近心隙 の場合 逆にする
            if ((this.状態 == ER_状態.部近心隙)
                && this.歯種.Equals(other.歯種)
                )
            {
                res = -res;
            }

            return res;
        }
        public int CompareTo(object obj)
        {
            if (obj is ER歯式単位)
            {
                return ((IComparable<ER歯式単位>)this).CompareTo((ER歯式単位)obj);
            }

            throw new InvalidOperationException(typeof(ER歯式単位) + "と比較できないオブジェクトが比較されました。");
        }

        public ER歯式.歯区分 Get歯区分()
        {
            bool is前歯 = 歯種[3] <= '3';

            switch (部位)
            {
                case ER歯式.部位.右側上顎:
                    return is前歯 ? ER歯式.歯区分.上顎前歯 : ER歯式.歯区分.右側上顎臼歯;

                case ER歯式.部位.左側上顎:
                    return is前歯 ? ER歯式.歯区分.上顎前歯 : ER歯式.歯区分.左側上顎臼歯;

                case ER歯式.部位.左側下顎:
                    return is前歯 ? ER歯式.歯区分.下顎前歯 : ER歯式.歯区分.左側下顎臼歯;

                case ER歯式.部位.右側下顎:
                    return is前歯 ? ER歯式.歯区分.下顎前歯 : ER歯式.歯区分.右側下顎臼歯;

                default:
                    throw new InvalidOperationException("[error: fc634e14] 未対応の部位: " + 部位);
            }
        }


        #region Static Method

        public static ER歯式単位[] Get7to7(bool jougaku)
        {
            var list = new List<ER歯式単位>();
            ER_状態 joutai = ER_状態.現存歯;
            ER_部分 bubun = ER_部分.部分指定なし;
            if (jougaku)
            {
                string prefix = 歯種_部位_右側上顎;
                for (int i = 7; 1 <= i; i--)
                {
                    string sisyu = prefix + i;
                    list.Add(new ER歯式単位(sisyu, joutai, bubun));
                }

                prefix = 歯種_部位_左側上顎;
                for (int i = 1; i <= 7; i++)
                {
                    string sisyu = prefix + i;
                    list.Add(new ER歯式単位(sisyu, joutai, bubun));
                }
            }
            else
            {
                string prefix = 歯種_部位_左側下顎;
                for (int i = 7; 1 <= i; i--)
                {
                    string sisyu = prefix + i;
                    list.Add(new ER歯式単位(sisyu, joutai, bubun));
                }

                prefix = 歯種_部位_右側下顎;
                for (int i = 1; i <= 7; i++)
                {
                    string sisyu = prefix + i;
                    list.Add(new ER歯式単位(sisyu, joutai, bubun));
                }
            }
            return list.ToArray();
        }
        public static ER歯式単位[] GetEtoE(bool jougaku)
        {
            var list = new List<ER歯式単位>();
            ER_状態 joutai = ER_状態.現存歯;
            ER_部分 bubun = ER_部分.部分指定なし;
            if (jougaku)
            {
                string prefix = 歯種_部位_右側上顎乳;
                for (int i = 5; 1 <= i; i--)
                {
                    string sisyu = prefix + i;
                    list.Add(new ER歯式単位(sisyu, joutai, bubun));
                }

                prefix = 歯種_部位_左側上顎乳;
                for (int i = 1; i <= 5; i++)
                {
                    string sisyu = prefix + i;
                    list.Add(new ER歯式単位(sisyu, joutai, bubun));
                }
            }
            else
            {
                string prefix = 歯種_部位_左側下顎乳;
                for (int i = 5; 1 <= i; i--)
                {
                    string sisyu = prefix + i;
                    list.Add(new ER歯式単位(sisyu, joutai, bubun));
                }

                prefix = 歯種_部位_右側下顎乳;
                for (int i = 1; i <= 5; i++)
                {
                    string sisyu = prefix + i;
                    list.Add(new ER歯式単位(sisyu, joutai, bubun));
                }
            }
            return list.ToArray();
        }
        public static string Get歯席番号(string sisyu, bool nyuusi)
        {
            string sisekiBangou = sisyu.Substring(3, 1);

            if (nyuusi)
            {
                // キャラクターに64を足して数字を英字に変換
                return ((char)(int.Parse(sisekiBangou) + 64)).ToString();
            }

            return sisekiBangou;
        }
        public static ER歯式単位 FromERCode(string erCode)
        {
            return FromERCode(erCode, 0);
        }
        public static ER歯式単位[] FromERCodeToArray(string erCode)
        {
            int length = erCode.Length / 歯式単位_文字数;
            var ar = new ER歯式単位[length];

            for (int i = 0; i < length; ++i)
            {
                ar[i] = FromERCode(erCode, i * 歯式単位_文字数);
            }
            return ar;
        }
        internal static ER歯式単位 FromERCode(string erCode, int startIndex)
        {
            var shisyu = erCode.Substring(startIndex, 歯種_文字数);
            startIndex += 歯種_文字数;
            var joutai = (ER_状態)Char.GetNumericValue(erCode, startIndex);
            startIndex++;
            var bubun = (ER_部分)Char.GetNumericValue(erCode, startIndex);
            return new ER歯式単位(shisyu, joutai, bubun);
        }
        internal static ER歯式.部位 Get部位(string sisyu)
        {
            switch (sisyu.Substring(0, 3))
            {
                case 歯種_部位_右側上顎:
                case 歯種_部位_右側上顎乳:
                    return ER歯式.部位.右側上顎;

                case 歯種_部位_左側上顎:
                case 歯種_部位_左側上顎乳:
                    return ER歯式.部位.左側上顎;

                case 歯種_部位_左側下顎:
                case 歯種_部位_左側下顎乳:
                    return ER歯式.部位.左側下顎;

                case 歯種_部位_右側下顎:
                case 歯種_部位_右側下顎乳:
                    return ER歯式.部位.右側下顎;
            }

            throw new Exception("歯種(" + sisyu + ") の部位が判定できません。");
        }
        internal static bool 歯種Is乳歯(string sisyu)
        {
            string h = sisyu.Substring(0, 3);

            return h.Equals(歯種_部位_右側上顎乳)
                || h.Equals(歯種_部位_左側上顎乳)
                || h.Equals(歯種_部位_左側下顎乳)
                || h.Equals(歯種_部位_右側下顎乳);
        }
        /// <summary>
        /// 2つの部位が同一または隣接している場合はtrueを返す。
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        private static bool IsEqualOrNext(ER歯式.部位 a, ER歯式.部位 b)
        {
            return (
                (a == b)
                || ((a == ER歯式.部位.右側上顎) && (b == ER歯式.部位.左側上顎))
                || ((a == ER歯式.部位.左側上顎) && (b == ER歯式.部位.右側上顎))
                || ((a == ER歯式.部位.左側下顎) && (b == ER歯式.部位.右側下顎))
                || ((a == ER歯式.部位.右側下顎) && (b == ER歯式.部位.左側下顎))
                );
        }
        private static string ToERCode(ER歯式単位 obj)
        {
            return obj.歯種 + obj.状態.ToERCode() + obj.部分.ToERCode();
        }

        #endregion


        #region Operator Overload

        public static bool operator ==(ER歯式単位 a, ER歯式単位 b)
        {
            return (a.ERCode == b.ERCode);
        }
        public static bool operator !=(ER歯式単位 a, ER歯式単位 b)
        {
            return !(a == b);
        }

        #region 比較演算子のオーバーロード

        public static bool operator <(ER歯式単位 x, ER歯式単位 y)
        {
            return x.CompareTo(y) < 0;
        }
        public static bool operator <=(ER歯式単位 x, ER歯式単位 y)
        {
            return x.CompareTo(y) <= 0;
        }
        public static bool operator >(ER歯式単位 x, ER歯式単位 y)
        {
            return y < x;
        }
        public static bool operator >=(ER歯式単位 x, ER歯式単位 y)
        {
            return y <= x;
        }

        #endregion

        #endregion
    }
}
