#nullable enable

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
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

        private static List<Hokensha> CreateHokenshaList(KanjaData karte)
        {
            List<Hokensha> hokenshaList = new List<Hokensha>();

            hokenshaList.Add(new Hokensha
            {
                保険者番号 = Get保険者番号(karte.保険者番号),
                被保険者記号 = karte.被保険者証_記号,
                被保険者番号 = karte.被保険者証_番号,
                被保険者氏名 = karte.氏名?.Replace("　", " "),
                // 被保険者枝番 = karte.保険種別,
                // 続柄 = karte.本人家族区分,
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
                // Remarks = ,
                // SpecialInsuranceNumber = ,
                // ValidityPeriodEnd = ,
            });

            return hokenshaList;
        }

        private static readonly Regex PostCodeRegex = new Regex(@"^(\d{3})(\d{4})$", RegexOptions.Compiled);


        private static string? Get保険者番号(int? value)
        {
            return ((value ?? 0) == 0) ? null : value.ToString();
        }
    }
}
