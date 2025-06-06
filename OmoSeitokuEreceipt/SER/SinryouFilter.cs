using Microsoft.VisualBasic;
using OmoEReceLib.ERObjects;
using System;

namespace OmoSeitokuEreceipt.SER
{
    [Serializable]
    public sealed class SinryouFilter
    {
        public const int 除外種別_除外 = 0;
        public const int 除外種別_のみ = 1;

        private string _byoumei;
        private string _syochi;


        public string Title
        {
            get;
            set;
        }

        public string 病名
        {
            get { return _byoumei; }
            set { _byoumei = Normalize(value); }
        }
        public MatchType 病名MatchType = MatchType.前方一致;
        public bool 病名否定 = false;

        public string 処置
        {
            get { return _syochi; }
            set { _syochi = Normalize(value); }
        }
        public MatchType 処置MatchType = MatchType.部分一致;
        public bool 処置否定 = false;

        public ER歯式 歯式
        {
            get;
            set;
        }
        public bool 歯種完全一致 = false;

        public bool P病名除外;
        public bool G病名除外;
        public bool 義歯除外;
        public int 除外種別 = 除外種別_除外;

        public ShinryouOrderType 並び順Type;


        // Get Only Property
        public bool Has病名
        {
            get { return _byoumei != null && _byoumei.Length != 0; }
        }
        public bool Has処置
        {
            get { return _syochi != null && _syochi.Length != 0; }
        }
        public bool Has歯式
        {
            get { return this.歯式 != null; }
        }


        // Constructor

        public SinryouFilter(string title)
        {
            this.Title = title;
        }

        public SinryouFilter(string title, SinryouFilter source)
            : this(title)
        {
            this.病名 = source.病名;
            this.病名MatchType = source.病名MatchType;
            this.病名否定 = source.病名否定;

            this.処置 = source.処置;
            this.処置MatchType = source.処置MatchType;
            this.処置否定 = source.処置否定;

            this.歯式 = (source.歯式 != null) ? new ER歯式(source.歯式) : null;
            this.歯種完全一致 = source.歯種完全一致;

            this.P病名除外 = source.P病名除外;
            this.G病名除外 = source.G病名除外;
            this.義歯除外 = source.義歯除外;
            this.除外種別 = source.除外種別;

            this.並び順Type = source.並び順Type;
        }

        public SinryouFilter(SinryouFilter source)
            : this(source.Title, source)
        {
        }

        public SinryouFilter() :
            this(string.Empty)
        {
        }


        // Method

        public void Combine(SinryouFilter other)
        {
            if (other.Has病名)
            {
                this.病名 = other.病名;
                this.病名MatchType = other.病名MatchType;
                this.病名否定 = other.病名否定;
            }
            if (other.Has処置)
            {
                this.処置 = other.処置;
                this.処置MatchType = other.処置MatchType;
                this.処置否定 = other.処置否定;
            }
            if (other.Has歯式)
            {
                this.歯式 = new ER歯式(other.歯式);
                this.歯種完全一致 = other.歯種完全一致;
            }

            this.P病名除外 |= other.P病名除外;
            this.G病名除外 |= other.G病名除外;
            this.義歯除外 |= other.義歯除外;
            this.除外種別 = other.除外種別;

            this.並び順Type = other.並び順Type;
        }

        public string GetKey()
        {
            return this.GetHashCode().ToString();
        }

        public override int GetHashCode()
        {
            return (!this.Has病名 ? 0 : this.病名.GetHashCode() ^ this.病名MatchType.GetHashCode() ^ this.病名否定.GetHashCode()) ^
                   (!this.Has処置 ? 0 : this.処置.GetHashCode() ^ this.処置MatchType.GetHashCode() ^ this.処置否定.GetHashCode()) ^
                   (!this.Has歯式 ? 0 : this.歯式.GetHashCode() ^ this.歯種完全一致.GetHashCode()) ^
                   this.P病名除外.GetHashCode() ^
                   this.G病名除外.GetHashCode() ^
                   this.義歯除外.GetHashCode() ^
                   this.除外種別.GetHashCode() ^
                   this.並び順Type.GetHashCode();
        }


        // Helper

        private static string Normalize(string value)
        {
            return Microsoft.VisualBasic.Strings.StrConv(
                    value, (VbStrConv.Hiragana | VbStrConv.Narrow));
        }



        public enum MatchType
        {
            部分一致,
            前方一致,
            後方一致,
            完全一致
        }
    }
}
