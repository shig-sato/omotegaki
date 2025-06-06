using System;
using System.Collections.Generic;

namespace OmoSeitokuEreceipt.SER
{
    public sealed class KarteId : IEquatable<KarteId>, IComparable<KarteId>
    {
        public KarteId(Shinryoujo shinryoujo, int karteNumber)
        {
            if (shinryoujo is null) { throw new ArgumentNullException(nameof(shinryoujo)); }
            if (karteNumber <= 0) { throw new ArgumentOutOfRangeException(nameof(karteNumber)); }

            Shinryoujo = shinryoujo;
            KarteNumber = karteNumber;
        }

        public Shinryoujo Shinryoujo { get; }
        public int KarteNumber { get; }

        #region IEquatable

        public override bool Equals(object obj)
        {
            return Equals(obj as KarteId);
        }

        public bool Equals(KarteId other)
        {
            return !(other is null) &&
                   this.Shinryoujo.Equals(other.Shinryoujo) &&
                   this.KarteNumber.Equals(other.KarteNumber);
        }

        public override int GetHashCode()
        {
            var hashCode = -415356826;
            hashCode = hashCode * -1521134295 + EqualityComparer<Shinryoujo>.Default.GetHashCode(Shinryoujo);
            hashCode = hashCode * -1521134295 + KarteNumber.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(KarteId left, KarteId right)
        {
            return EqualityComparer<KarteId>.Default.Equals(left, right);
        }

        public static bool operator !=(KarteId left, KarteId right) { return !(left == right); }

        #endregion

        public int CompareTo(KarteId other)
        {
            int c = this.Shinryoujo.Key.CompareTo(other.Shinryoujo.Key);
            return (c != 0)
                ? c
                : this.KarteNumber.CompareTo(other.KarteNumber);
        }

        public override string ToString()
        {
            return $"{Shinryoujo}: {KarteNumber}";
        }
    }
}
