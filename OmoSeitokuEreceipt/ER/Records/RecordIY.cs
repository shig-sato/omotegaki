using System.Text.RegularExpressions;

namespace OmoEReceLib
{
    /*
     * 医薬品
     */
    public sealed class RecordIY
    {
        /// <summary>レコード識別</summary>
        public const string 識別 = "IY";

        /// <summary>マスターコード正規表現 (GroupName: IYCode)</summary>
        public static Regex ERCodeRegex = new Regex("(?<" + 識別 + @"Code>6\d{8})");


        public static float CalcTensuu(float yakka)
        {
            if (0 < yakka && yakka <= 15)
            {
                return 1;
            }
            else
            {
                return Ceil(1 + (yakka - 15) * 0.1f);
            }

            // 外用薬は1調剤の薬価を計算
            // 内服ではこれに日数をかける
        }

        // 1点未満切り上げ
        private static int Ceil(float tensuu)
        {
            if (tensuu == 0) return 0;
            int res = (int)tensuu;
            if (tensuu % res == 0 || tensuu < 0)
                return res;
            return res + 1;
        }
    }
}
