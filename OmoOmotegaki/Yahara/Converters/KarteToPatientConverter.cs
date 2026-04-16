#nullable enable

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using OmoEReceLib;
using OmoSeitokuEreceipt.SER;
using omotegaki_xml.Libs.Yahara.Entities.PatientEntities;

namespace omotegaki_xml.Libs.Yahara.Converters
{
    /// <summary>
    /// Yahara - Patient
    /// </summary>
    public static partial class KarteToPatientConverter
    {
        public static Patient Convert(KarteId karteId, KarteData karte)
        {
            var patient = new Patient(karteNo: karteId.KarteNumber.ToString())
            {
                Name = karte.氏名カナ?.Replace("　", " "),
                // 名前の区切りは半角スペース
                KanjiName = karte.氏名?.Replace("　", " "),
                Birthday = karte.生年月日?.ToString("yyyy/MM/dd"),
                Sex = GetSex(karte),
                郵便番号 = GetPostalCode(karte),
                住所 = karte.住所,
                連絡先電話番号 = karte.電話番号,
                自宅電話番号 = karte.電話番号,
                主担当医Object = null, // GetPrimaryDoctor(karte),
                保険者 = CreateHokenshaList(karte),
            };

            return patient;
        }

        private static string? GetSex(KanjaData karte)
        {
            return karte.性別 switch
            {
                OmoEReceLib.ER_男女区分.男 => "Male",
                OmoEReceLib.ER_男女区分.女 => "Female",
                null => null,
                _ => throw new ArgumentException($"性別が不正な値: {karte.性別}"),
            };
        }

        private static string? GetPostalCode(KanjaData karte)
        {
            if ((karte.郵便番号 ?? 0) == 0) return null;
            string? p = karte.郵便番号?.ToString();
            return (p == null) ? null : PostCodeRegex.Replace(p, "$1-$2");
        }

        //private static Doctor? GetPrimaryDoctor(KanjaData karte)
        //{
        //    // int? doctorIndex = karte.担当医;

        //    return null;
        //}

        private static readonly string[] EDABAN_SEP = new[] { " 枝番" };
        private static List<Hokensha> CreateHokenshaList(KanjaData karte)
        {
            var spl = karte.被保険者証_番号?.Split(EDABAN_SEP, StringSplitOptions.RemoveEmptyEntries);

            var hokenshaList = new List<Hokensha>
            {
                new Hokensha
                {
                    保険者番号 = karte.保険者番号?.Code,
                    被保険者記号 = karte.被保険者証_記号,
                    被保険者番号 = spl == null ? null : spl[0],
                    被保険者氏名 = karte.氏名?.Replace("　", " "),
                    被保険者枝番 = (spl?.Length >= 2 ? spl[1] : null),
                    続柄 = Get続柄(karte.本人家族区分),
                    // 職務上の事由 = karte.職務上の事由 switch
                    // {
                    //   OmoEReceLib.ER_職務上の事由.職務上 => ,
                    //   OmoEReceLib.ER_職務上の事由.下船後３月以内 => ,
                    //   OmoEReceLib.ER_職務上の事由.通勤災害 => ,
                    //   null => "None",
                    //   _ => throw new ArgumentException($"職務上の事由が不正な値: {karte.職務上の事由}"),
                    // },
                    資格取得日 = karte.保険証使用開始日?.ToString("yyyy/MM/dd"),
                    保険有効期限 = karte.保険有効期限?.ToString("yyyy/MM/dd"),
                    Remarks = Get備考(karte.本人家族区分),
                    // SpecialInsuranceNumber = ,
                    // ValidityPeriodEnd = ,
                }
            };

            return hokenshaList;
        }

        private static readonly Regex PostCodeRegex = new Regex(@"^(\d{3})(\d{4})$", RegexOptions.Compiled);

        private static string? Get続柄(ER_レセプト種別_本人家族入院区分? value)
        {
            if (!value.HasValue) return null;
            switch (value.Value)
            {
                case ER_レセプト種別_本人家族入院区分.本人_入:
                case ER_レセプト種別_本人家族入院区分.本人_外:
                case ER_レセプト種別_本人家族入院区分.未就学_入:
                case ER_レセプト種別_本人家族入院区分.未就学_外:
                case ER_レセプト種別_本人家族入院区分.高齢一般_低所得_入:
                case ER_レセプト種別_本人家族入院区分.高齢一般_低所得_外:
                case ER_レセプト種別_本人家族入院区分.高齢受給者_入:
                case ER_レセプト種別_本人家族入院区分.高齢受給者_外:
                    return "本人";
                case ER_レセプト種別_本人家族入院区分.家族_入:
                case ER_レセプト種別_本人家族入院区分.家族_外:
                    return "家族";
                default:
                    throw new Exception("[2f9fjn8g02mnfjf4f]");
            }
        }
        private static string? Get備考(ER_レセプト種別_本人家族入院区分? value)
        {
            return null;

            if (!value.HasValue) return null;
            switch (value.Value)
            {
                case ER_レセプト種別_本人家族入院区分.本人_入:
                case ER_レセプト種別_本人家族入院区分.本人_外:
                case ER_レセプト種別_本人家族入院区分.家族_入:
                case ER_レセプト種別_本人家族入院区分.家族_外:
                case ER_レセプト種別_本人家族入院区分.未就学_入:
                case ER_レセプト種別_本人家族入院区分.未就学_外:
                    return null;
                case ER_レセプト種別_本人家族入院区分.高齢一般_低所得_入:
                case ER_レセプト種別_本人家族入院区分.高齢一般_低所得_外:
                case ER_レセプト種別_本人家族入院区分.高齢受給者_入:
                case ER_レセプト種別_本人家族入院区分.高齢受給者_外:
                    return null;
                default:
                    throw new Exception("[g3weggf23q2ffdfd]");
            }

            /*
"前期高齢者 一般"
"前期高齢者 一般 2割"
"前期高齢者 一般 低I"
"前期高齢者 一般 低I 2割"
"前期高齢者 一般 低II"
"前期高齢者 一般 低II 2割"
"前期高齢者 一定以上"
"後期高齢者 一般"
"後期高齢者 一般 2割"
"後期高齢者 一般 低I"
"後期高齢者 一般 低II"
"後期高齢者 一定以上"
             */
        }
    }
}
