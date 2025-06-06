using System.Text.RegularExpressions;

namespace OmoEReceLib
{
    /*
     * 特定器材
     */
    public sealed class RecordTO
    {
        public const string 識別 = "TO";

        /// <summary>マスターコード正規表現 (GroupName: TOCode)</summary>
        public static Regex ERCodeRegex = new Regex("(?<" + 識別 + @"Code>7\d{8})");


        public static float CalcTensuu(float kingaku)
        {
            // 10で割って端数四捨五入
            return (int)(kingaku * 0.1f + 0.5f);
        }
    }
}
