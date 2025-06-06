using System;
using System.Collections.Generic;

namespace OmoSeitokuEreceipt.SER
{
    public sealed class Shinryoujo : IEquatable<Shinryoujo>
    {
        public Shinryoujo(string key)
        {
            if (key is null) { throw new ArgumentNullException(nameof(key)); }

            Key = To先頭大文字(key);
        }

        public string Key { get; }

        #region IEquatable

        public override bool Equals(object obj) { return (obj is Shinryoujo shinryoujo) && Equals(shinryoujo); }

        public bool Equals(Shinryoujo other)
        {
            return !(other is null) && Key.Equals(other.Key, StringComparison.OrdinalIgnoreCase);
        }

        public override int GetHashCode()
        {
            return 990326508 + EqualityComparer<string>.Default.GetHashCode(Key);
        }

        public static bool operator ==(Shinryoujo left, Shinryoujo right)
        {
            return ReferenceEquals(left, right) ||
                (!(left is null) && left.Equals(right));
        }
        public static bool operator !=(Shinryoujo left, Shinryoujo right) { return !(left == right); }

        #endregion


        public override string ToString()
        {
            return Key;
        }

        private static string To先頭大文字(string value)
        {
            return value.Length == 1
                    ? value[0].ToString().ToUpperInvariant()
                    : value[0].ToString().ToUpperInvariant() + value.Substring(1).ToLowerInvariant();
        }
    }
}
