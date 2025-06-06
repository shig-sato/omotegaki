using System;

namespace OmoSeitokuEreceipt.SER
{
    [Serializable]
    public struct SyochiData
    {
        public const int ALL_SYOCHI_COUNT = 1000;


        public readonly int Number;
        public readonly int SyochiId;
        public int Tensuu;
        public int Kaisuu;
        public string Name;
        public readonly bool IsSystemSyochi;
        // 装着フラグ 治療の終了
        public readonly bool IsFinish;


        // Constructor

        public SyochiData(string name, int number, int tensuu, int kaisuu, bool isFinish)
        {
            this.SyochiId = SyochiData.GetSyochiId(number);

            // 処置名から点数を削除
            name = name.Replace("(" + tensuu + ")", "");
            // 処置名内の処置番号を変更
            name = "[" + this.SyochiId + "] " + name.Replace(number + "*", "");

            this.Name = name;
            this.Number = number;
            this.Tensuu = tensuu;
            this.Kaisuu = kaisuu;
            this.IsSystemSyochi = SyochiData.Is特殊処置(this.SyochiId);
            this.IsFinish = isFinish;
        }


        // Method

        public bool Equals(SyochiData other)
        {
            return (this.Number == other.Number)
                && (this.Kaisuu == other.Kaisuu)
                && (this.Name == other.Name);
        }
        public override bool Equals(object obj)
        {
            return (GetType() == obj.GetType())
                && Equals((SyochiData)obj);
        }
        public override int GetHashCode()
        {
            return this.Number.GetHashCode()
                 ^ this.Kaisuu.GetHashCode()
                 ^ this.Name.GetHashCode();
        }

        #region Operator

        public static bool operator ==(SyochiData a, SyochiData b)
        {
            return a.Equals(b);
        }
        public static bool operator !=(SyochiData a, SyochiData b)
        {
            return !(a == b);
        }

        #endregion


        // Static Method

        public static bool Is特殊処置(int id)
        {
            return 900 <= id && id < ALL_SYOCHI_COUNT;
        }
        public static int GetSyochiId(int syochiNumber)
        {
            return syochiNumber % ALL_SYOCHI_COUNT;
        }
    }
}