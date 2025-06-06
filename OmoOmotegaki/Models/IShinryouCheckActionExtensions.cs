using OmoEReceLib.ERObjects;
using OmoSeitokuEreceipt.SER;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OmoOmotegaki.Models
{
    public static class IShinryouCheckActionExtensions
    {
        public static string CreateResultTitle(this IShinryouCheckAction _,
            int shochiId, string shochiName)
        {
            if (string.IsNullOrWhiteSpace(shochiName))
            {
                return $"[{shochiId}]";
            }
            else if (shochiName[0] == '[')
            {
                // 処置名に処置番号が含まれている場合
                return shochiName;
            }
            else
            {
                return $"[{shochiId}] {shochiName}";
            }
        }

        public static bool TryGet処置名(this IShinryouCheckAction _,
            IEnumerable<SinryouData> shinryouDataCollection, int shochiId,
            out string shochiName, out string errorMessage)
        {
            errorMessage = null;
            foreach (SinryouData shinryouData in shinryouDataCollection)
            {
                try
                {
                    shochiName = SinryouDataLoader.GetSyochiName(shochiId, shinryouData.診療日);
                    return true;
                }
                catch (Exception ex)
                {
                    errorMessage = ex.Message;
                }
            }
            shochiName = null;
            return false;
        }

        public static bool TryGet処置(this IShinryouCheckAction _,
            SinryouData shinryouData, int shochiId, out SyochiData shochi)
        {
            foreach (SyochiData s in shinryouData.処置)
            {
                if (s.SyochiId == shochiId && s.Kaisuu > 0)
                {
                    shochi = s;
                    return true;
                }
            }
            shochi = default;
            return false;
        }

        public static bool IsSameBlock(this IShinryouCheckAction _,
            算定ブロック単位 算定ブロック単位, SinryouData p1, SinryouData p2)
        {
            return 算定ブロック単位 switch
            {
                算定ブロック単位.全歯 =>
                    (p1?.歯式 is object) && (p2?.歯式 is object),

                算定ブロック単位.顎 =>
                    (p1?.歯式 is object) && (p1.歯式.Count > 0) &&
                    (p2?.歯式 is object) && (p2.歯式.Count > 0) &&
                    p1.歯式.First().部位.Is上顎() &&
                    p2.歯式.First().部位.Is上顎(),

                算定ブロック単位.歯区分 =>
                    (p1?.歯式 is object) && (p1.歯式.Count > 0) &&
                    (p2?.歯式 is object) && (p2.歯式.Count > 0) &&
                    (p1.歯式.First().Get歯区分() == p2.歯式.First().Get歯区分()),

                _ => throw new InvalidOperationException("[error: 6312d62c] 未対応の 算定ブロック単位: " + 算定ブロック単位),
            };
        }
    }
}