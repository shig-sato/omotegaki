#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using OmoSeitoku;
using OmoSeitokuEreceipt.SER;
using omotegaki_xml.Libs.Yahara.Entities.KarteEntities;
using omotegaki_xml.Libs.Yahara.Entities.PatientEntities;

namespace omotegaki_xml.Libs.Yahara.Converters
{
    /// <summary>
    /// Yahara - Karte
    /// </summary>
    public static partial class KarteDataToKarteConverter
    {
        public static Karte Convert(KarteId karteId, KarteData seitokuKarte)
        {
            SinryouDataLoader sinryouDataLoader;
            using (var fs = new ShinryouFileStream(karteId.Shinryoujo))
            {
                sinryouDataLoader = fs.GetSinryouDataLoader(karteId);
            }

            ShinryouDataCollection shinryouData = sinryouDataLoader.GetShinryouData(DateRange.All, SinryouDataLoader.診療統合種別.統合なし);

            List<Day> days = shinryouData
                .GroupBy(p => p.診療日)
                .Select(ToDays)
                .ToList();

            var yaharaKarte = new Karte
            {
                KarteNo = karteId.KarteNumber.ToString(),
                Name = seitokuKarte.Get氏名(false)?.Replace("　", " "),
                Doctor = GetDoctorName(seitokuKarte.担当医),
                Days = days
            };

            return yaharaKarte;
        }

        private static Day ToDays(IGrouping<DateTime, SinryouData> dateGroup, int dayIndex)
        {
            var sinryouData = dateGroup.First();
            List<Bui> buis = new List<Bui>() { ToBui(sinryouData, dayIndex) };

            return new Day
            {
                Date = sinryouData.診療日.ToString("yyyy/MM/dd"),
                DateNo = dayIndex.ToString(),
                Doctor = GetDoctorName(sinryouData.担当医師),
                Buis = dateGroup.Select(ToBui).ToList()
            };
        }

        private static Bui ToBui(SinryouData sinryouData, int buiIndex)
        {
            return new Bui
            {
                No = buiIndex.ToString(),
                Teeth = TeethConverter.Convert(sinryouData.歯式),
                Doctor = GetDoctorName(sinryouData.担当医師),
                Diseases = sinryouData.病名.Select(p => new Disease { Name = p }).ToList(),
                Treatments = sinryouData.処置.Where(p => !p.IsSystemSyochi).Select(ToTreatment).ToList(),
            };
        }

        private static Treatment ToTreatment(SyochiData syochiData)
        {
            return new Treatment
            {
                Point = syochiData.Tensuu.ToString(),
                Count = syochiData.Kaisuu.ToString(),
                InnerBui = null,
                Name = syochiData.Name,
            };
        }

        private static string? GetDoctorName(int? doctorIndex)
        {
            //return doctorIndex == null ? null : $"担当医{doctorIndex}"; // TODO 担当医
            return doctorIndex == null ? null : $"担当医0";
        }
    }
}
