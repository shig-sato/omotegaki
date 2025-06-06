using System;
using System.Globalization;
using System.Text;

namespace OmoEReceLib
{
    public abstract class ERDateTime
    {
        // GetEra値
        private const int ERA_MEIJI = 1;
        private const int ERA_TAISHO = 2;
        private const int ERA_SHOWA = 3;
        private const int ERA_HEISEI = 4;
        private const int ERA_REIWA = 5;

        private static Calendar _calendar = new JapaneseCalendar();
        private static CultureInfo _culture = new CultureInfo("ja-JP", true);

        static ERDateTime()
        {
            _culture.DateTimeFormat.Calendar = _calendar;
        }


        public static Calendar CurrentCalendar
        {
            get { return _calendar; }
            set { _calendar = value; }
        }

        public static CultureInfo CurrentCultureInfo
        {
            get { return _culture; }
            set { _culture = value; }
        }


        public static string Format(DateTime dt, string format)
        {
            return dt.ToString(format, _culture);
        }

        public static int GetEra(DateTime dt)
        {
            return _calendar.GetEra(dt);
        }

        /// <summary>
        /// 年号と年を返す。
        /// </summary>
        /// <param name="dt">日付</param>
        /// <returns>(ex: "平成 24")</returns>
        public static string GetEraYear(DateTime dt)
        {
            return new StringBuilder(DateTo年号区分(dt).ToString()).Append(' ').Append(_calendar.GetYear(dt)).ToString();
        }

        /// <summary>
        /// 年号と年を返す。
        /// </summary>
        /// <param name="dt">日付</param>
        /// <param name="shortEra">短い年号形式を使う場合true</param>
        /// <returns>(ex: "平成 24")</returns>
        public static string GetEraYear(DateTime dt, bool shortEra)
        {
            string e = DateTo年号区分(dt).ToString();
            return (shortEra ? e[0].ToString() : e) + ' ' + _calendar.GetYear(dt);
        }

        public static ER_年号区分 DateTo年号区分(DateTime dt)
        {
            return EraTo年号区分(_calendar.GetEra(dt));
        }

        public static ER_年号区分 EraTo年号区分(int era)
        {
            switch (era)
            {
                case ERA_MEIJI: return ER_年号区分.明治;
                case ERA_TAISHO: return ER_年号区分.大正;
                case ERA_SHOWA: return ER_年号区分.昭和;
                case ERA_HEISEI: return ER_年号区分.平成;
                case ERA_REIWA: return ER_年号区分.令和;
            }

            throw new Exception(typeof(ERDateTime).Name + ".EraTo年号区分: 変換できる年号がありません。");
        }

        /// <example>DateTo年月日(new DateTime(1995,1,23)) == "4070123" // 平成7年1月23日</example>
        public static ER年月日 DateTo年月日(DateTime dt)
        {
            return new ER年月日(
                DateTo年号区分(dt),
                _calendar.GetYear(dt),
                dt.Month,
                dt.Day
                );
        }

        /// <example>DateTo年月(new DateTime(1995,1,23)) == "40701" // 平成7年1月</example>
        public static ER年月 DateTo年月(DateTime dt)
        {
            return new ER年月(
                DateTo年号区分(dt),
                _calendar.GetYear(dt),
                dt.Month
                );
        }
    }

    public struct ER年月 : IComparable<ER年月>, IComparable
    {
        public string ERCode;


        public string Year { get { return ERCode.Substring(1, 2); } }
        public string Month { get { return ERCode.Substring(3, 2); } }
        public int RawYear { get { return _rawYear; } }
        public int RawMonth { get { return _rawMonth; } }

        private int _rawYear;
        private int _rawMonth;

        public ER年月(ER_年号区分 nengou, int year, int month)
        {
            ERCode = new StringBuilder()
                            .Append(nengou.ToERCode())
                            .Append(year.ToString().PadLeft(2, '0'))
                            .Append(month.ToString().PadLeft(2, '0'))
                            .ToString();
            _rawYear = year;
            _rawMonth = month;
        }


        public override bool Equals(object obj)
        {
            return (GetType() == obj.GetType())
                && (ERCode == ((ER年月)obj).ERCode);
        }
        public override int GetHashCode()
        {
            return ERCode.GetHashCode();
        }
        public override string ToString()
        {
            return ERCode;
        }
        public int CompareTo(ER年月 other)
        {
            int c = RawYear.CompareTo(other.RawYear);
            if (c != 0) return c;
            return RawMonth.CompareTo(other.RawMonth);
        }
        public int CompareTo(object obj)
        {
            return this.CompareTo((ER年月)obj);
        }

        #region Operator

        public static bool operator ==(ER年月 a, ER年月 b)
        {
            return (a.ERCode == b.ERCode);
        }
        public static bool operator !=(ER年月 a, ER年月 b)
        {
            return !(a == b);
        }

        public static bool operator <(ER年月 x, ER年月 y)
        {
            return x.CompareTo(y) < 0;
        }
        public static bool operator <=(ER年月 x, ER年月 y)
        {
            return x.CompareTo(y) <= 0;
        }
        public static bool operator >(ER年月 x, ER年月 y)
        {
            return y < x;
        }
        public static bool operator >=(ER年月 x, ER年月 y)
        {
            return y <= x;
        }

        #endregion
    }

    public struct ER年月日 : IComparable<ER年月日>, IComparable
    {
        public string ERCode;


        public string Year { get { return ERCode.Substring(1, 2); } }
        public string Month { get { return ERCode.Substring(3, 2); } }
        public string Day { get { return ERCode.Substring(5, 2); } }
        public int RawYear { get { return _年月.RawYear; } }
        public int RawMonth { get { return _年月.RawMonth; } }
        public int RawDay { get { return _rawDay; } }
        public ER年月 年月 { get { return _年月; } }

        private ER年月 _年月;
        private int _rawDay;


        public ER年月日(ER_年号区分 nengou, int year, int month, int day)
        {
            _年月 = new ER年月(nengou, year, month);
            ERCode = new StringBuilder(10)
                            .Append(_年月.ERCode)
                            .Append(day.ToString().PadLeft(2, '0'))
                            .ToString();
            _rawDay = day;
        }


        public override bool Equals(object obj)
        {
            return (GetType() == obj.GetType())
               && (ERCode == ((ER年月日)obj).ERCode);
        }
        public override int GetHashCode()
        {
            return ERCode.GetHashCode();
        }
        public override string ToString()
        {
            return ERCode;
        }
        public int CompareTo(ER年月日 other)
        {
            int c = RawYear.CompareTo(other.RawYear);
            if (c != 0) return c;
            c = RawMonth.CompareTo(other.RawMonth);
            if (c != 0) return c;
            return RawDay.CompareTo(other.RawDay);
        }
        public int CompareTo(object obj)
        {
            return this.CompareTo((ER年月日)obj);
        }

        #region Operator

        public static bool operator ==(ER年月日 a, ER年月日 b)
        {
            return (a.ERCode == b.ERCode);
        }
        public static bool operator !=(ER年月日 a, ER年月日 b)
        {
            return !(a == b);
        }

        public static bool operator <(ER年月日 x, ER年月日 y)
        {
            return x.CompareTo(y) < 0;
        }
        public static bool operator <=(ER年月日 x, ER年月日 y)
        {
            return x.CompareTo(y) <= 0;
        }
        public static bool operator >(ER年月日 x, ER年月日 y)
        {
            return y < x;
        }
        public static bool operator >=(ER年月日 x, ER年月日 y)
        {
            return y <= x;
        }

        #endregion
    }
}
