using System;
using System.ComponentModel;
using System.Text;

namespace OmoSeitoku
{
    [Serializable]
    public struct Term : IEquatable<Term>
    {
        public Term(int years, int months, int days)
        {
            Years = years;
            Months = months;
            Days = days;
            Hours = 0;
            Minutes = 0;
            Seconds = 0;
        }

        public Term(int years, int months, int days, int hours, int minutes, int seconds)
        {
            Years = years;
            Months = months;
            Days = days;
            Hours = hours;
            Minutes = minutes;
            Seconds = seconds;
        }

        #region Property

        [DisplayName("年数")]
        public int Years { get; set; }

        [DisplayName("月数")]
        public int Months { get; set; }

        [DisplayName("日数")]
        public int Days { get; set; }

        [DisplayName("時間数")]
        public int Hours { get; set; }

        [DisplayName("分数")]
        public int Minutes { get; set; }

        [DisplayName("秒数")]
        public int Seconds { get; set; }

        [Browsable(false)]
        public bool IsEmpty => !HasDate && !HasTime;

        [Browsable(false)]
        public bool HasDate => Years > 0 || Months > 0 || Days > 0;
        [Browsable(false)]
        public bool HasTime => Hours > 0 || Minutes > 0 || Seconds > 0;

        #endregion

        public DateTime GetOffsetDate(DateTime dateTime)
        {
            if (Years != 0) dateTime = dateTime.AddYears(Years);
            if (Months != 0) dateTime = dateTime.AddMonths(Months);
            if (Days != 0) dateTime = dateTime.AddDays(Days);
            if (Hours != 0) dateTime = dateTime.AddHours(Hours);
            if (Minutes != 0) dateTime = dateTime.AddMinutes(Minutes);
            if (Seconds != 0) dateTime = dateTime.AddSeconds(Seconds);

            return dateTime;
        }

        public override bool Equals(object obj)
        {
            return obj is Term term && Equals(term);
        }

        public bool Equals(Term other)
        {
            return Years == other.Years &&
                   Months == other.Months &&
                   Days == other.Days &&
                   Hours == other.Hours &&
                   Minutes == other.Minutes &&
                   Seconds == other.Seconds;
        }

        public override int GetHashCode()
        {
            int hashCode = 311204573;
            hashCode = hashCode * -1521134295 + Years.GetHashCode();
            hashCode = hashCode * -1521134295 + Months.GetHashCode();
            hashCode = hashCode * -1521134295 + Days.GetHashCode();
            hashCode = hashCode * -1521134295 + Hours.GetHashCode();
            hashCode = hashCode * -1521134295 + Minutes.GetHashCode();
            hashCode = hashCode * -1521134295 + Seconds.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(Term left, Term right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Term left, Term right)
        {
            return !(left == right);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            if (Years > 0) sb.Append(Years).Append("年");
            if (Months > 0) sb.Append(Months).Append("ヶ月");
            if (Days > 0) sb.Append(Days).Append("日");
            if (Hours > 0) sb.Append(Hours).Append("時間");
            if (Minutes > 0) sb.Append(Minutes).Append("分");
            if (Seconds > 0) sb.Append(Seconds).Append("秒");

            if (sb.Length == 0)
                sb.Append("0秒間");
            else if (sb[sb.Length - 1] != '間')
                sb.Append("間");

            string res = sb.ToString();

            if (res == "6ヶ月間") res = "半年間";

            return res;
        }
    }
}
