using System;
using System.IO;

namespace OmoSeitokuEreceipt.SER
{
    [Serializable]
    public sealed class KarteData : KanjaData
    {
        new public static readonly KarteData Empty = new KarteData();

        public string 職業 { get; }
        public string 世帯主との続柄 { get; }
        public string 世帯主氏名 { get; }
        public DateTime? 資格取得 { get; } //UNDONE 資格取得に対応するものがForm4にあるかどうか
        public string 特別区名 { get; }
        public string 市町村名 { get; } // UNDONE 市町村番号との関連
        public string 国民健康保険組合名 { get; }

        public KarteData(BinaryReader input) : base(input)
        {
        }

        public KarteData(KarteData source) : base(source)
        {
            if (source is null) { throw new ArgumentNullException(nameof(source)); }

            職業 = source.職業;
            世帯主との続柄 = source.世帯主との続柄;
            世帯主氏名 = source.世帯主氏名;
            資格取得 = source.資格取得;
            特別区名 = source.特別区名;
            市町村名 = source.市町村名;
            国民健康保険組合名 = source.国民健康保険組合名;
        }

        private KarteData() : base(KanjaData.Empty)
        {
        }
    }
}