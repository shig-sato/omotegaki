using System;

namespace OmoSeitoku
{
    public struct DateRange : IComparable<DateRange>, IComparable<DateTime>, IComparable
    {
        public static readonly DateRange All = new DateRange(DateTime.MinValue, DateTime.MaxValue);


        public DateTime Min;
        public DateTime Max;

        public bool IsFullMin { get { return (Min == DateTime.MinValue); } }
        public bool IsFullMax { get { return (Max == DateTime.MaxValue); } }


        public DateRange(DateTime min, DateTime max)
        {
            Min = min;
            Max = max;
        }


        public bool Contains(DateTime dt)
        {
            return Min <= dt && dt <= Max;
        }
        public bool Contains(DateRange range)
        {
            return Min <= range.Min && range.Max <= Max;
        }
        public void Set(DateTime min, DateTime max)
        {
            Min = min;
            Max = max;
        }
        public void Set(DateRange range)
        {
            Min = range.Min;
            Max = range.Max;
        }
        public override string ToString()
        {
            return Min + " ～ " + Max;
        }

        #region Equals, Compare

        public override bool Equals(object obj)
        {
            return (obj != null)
                && (obj.GetType() != GetType())
                && this.Equals((DateRange)obj);
        }
        public bool Equals(DateRange other)
        {
            return (this.Min == other.Min)
                && (this.Max == other.Max);
        }
        public override int GetHashCode()
        {
            return Min.GetHashCode() ^ Max.GetHashCode();
        }
        public int CompareTo(DateRange other)
        {
            int res = Min.CompareTo(other.Min);
            return (res == 0)
                    ? Max.CompareTo(other.Max)
                    : res;
        }
        public int CompareTo(DateTime other)
        {
            int res = Min.CompareTo(other);
            return (res == 0)
                    ? Max.CompareTo(other)
                    : res;
        }
        public int CompareTo(object obj)
        {
            if (obj is DateRange)
            {
                return this.CompareTo((DateRange)obj);
            }

            if (obj is DateTime)
            {
                return this.CompareTo((DateTime)obj);
            }

            return 1;
        }

        public static bool operator ==(DateRange a, DateRange b)
        {
            return a.Equals(b);
        }
        public static bool operator !=(DateRange a, DateRange b)
        {
            return !(a == b);
        }

        #endregion
    }


    public struct DateRange2 : IComparable<DateRange2>, IComparable<DateTime>, IComparable
    {
        public static readonly DateRange2 Infinite = new DateRange2();


        public DateTime? Min;
        public DateTime? Max;

        public bool IsInfiniteMin { get { return !Min.HasValue; } }
        public bool IsInfiniteMax { get { return !Max.HasValue; } }


        public DateRange2(DateTime? min, DateTime? max)
        {
            Min = min;
            Max = max;
        }
        public DateRange2(DateRange range)
        {
            Min = range.IsFullMin ? default(DateTime?) : range.Min;
            Max = range.IsFullMax ? default(DateTime?) : range.Max;
        }


        public bool Contains(DateTime dt)
        {
            DateTime min = (Min.HasValue ? Min.Value : DateTime.MinValue);
            DateTime max = (Max.HasValue ? Max.Value : DateTime.MaxValue);
            return min <= dt && dt <= max;
        }
        public bool Contains(DateRange range)
        {
            DateTime min = (Min.HasValue ? Min.Value : DateTime.MinValue);
            DateTime max = (Max.HasValue ? Max.Value : DateTime.MaxValue);
            return min <= range.Min && range.Max <= max;
        }
        public bool Contains(DateRange2 range)
        {
            return Contains(range.ToDateRange());
        }
        public void Set(DateTime? min, DateTime? max)
        {
            Min = min;
            Max = max;
        }
        public void Set(DateRange2 range)
        {
            Min = range.Min;
            Max = range.Max;
        }
        public void Set(DateRange range)
        {
            Min = range.Min;
            Max = range.Max;
        }
        public DateRange ToDateRange()
        {
            DateTime min = (Min.HasValue ? Min.Value : DateTime.MinValue);
            DateTime max = (Max.HasValue ? Max.Value : DateTime.MaxValue);
            return new DateRange(min, max);
        }
        public override string ToString()
        {
            return Min + " ～ " + Max;
        }

        #region Equals, Compare

        public override bool Equals(object obj)
        {
            return (obj != null)
                && (obj.GetType() != GetType())
                && this.Equals((DateRange2)obj);
        }
        public bool Equals(DateRange2 other)
        {
            return (this.Min == other.Min)
                && (this.Max == other.Max);
        }
        public override int GetHashCode()
        {
            return Min.GetHashCode() ^ Max.GetHashCode();
        }
        public int CompareTo(DateRange2 other)
        {
            int res;

            // Compare Min

            DateTime? t = this.Min;
            DateTime? o = other.Min;

            if (t.HasValue)
            {
                res = o.HasValue
                        ? t.Value.CompareTo(o.Value)
                        : t.Value.CompareTo(null);
            }
            else
            {
                res = o.HasValue
                        ? -o.Value.CompareTo(null)
                        : 0;
            }

            if (res != 0)
            {
                return res;
            }

            // Compare Max

            t = this.Max;
            o = other.Max;

            if (t.HasValue)
            {
                return o.HasValue
                        ? t.Value.CompareTo(o.Value)
                        : t.Value.CompareTo(null);
            }
            else
            {
                return o.HasValue
                        ? -o.Value.CompareTo(null)
                        : 0;
            }
        }
        public int CompareTo(DateTime other)
        {
            int res;

            // Compare Min

            DateTime? t = this.Min;

            if (t.HasValue)
            {
                res = t.Value.CompareTo(other);
            }
            else
            {
                res = -other.CompareTo(null);
            }

            if (res != 0)
            {
                return res;
            }

            // Compare Max

            t = this.Max;

            return t.HasValue
                        ? -other.CompareTo(t.Value)
                        : -other.CompareTo(null);
        }
        public int CompareTo(object obj)
        {
            if (obj is DateRange2)
            {
                return this.CompareTo((DateRange2)obj);
            }

            if (obj is DateTime)
            {
                return this.CompareTo((DateTime)obj);
            }

            return 1;
        }

        public static bool operator ==(DateRange2 a, DateRange2 b)
        {
            return a.Equals(b);
        }
        public static bool operator !=(DateRange2 a, DateRange2 b)
        {
            return !(a == b);
        }

        #endregion
    }
}
