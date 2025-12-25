#nullable enable

using System;
using System.Collections.Generic;
using System.IO;

namespace omotegaki_xml.Libs.Yahara.Entities.PatientEntities
{
    public static class PatientExample
    {
        public static void TestSerialization(string outputDir)
        {
            var patient = CreateSample();
            var xml = patient.ToXML();
            Console.WriteLine("=== シリアライゼーションテスト ===");
            Console.WriteLine(xml);

            var path = Path.Combine(outputDir, "Patient_debug.xml");
            File.WriteAllText(path, xml + "\n");
            Console.WriteLine($"\nXMLファイルを保存しました: {path}");
        }

        private static Patient CreateSample()
        {
            return new Patient("1")
            {
                Name = "ﾃｽﾄ ﾀﾛｳ",
                KanjiName = "テスト 太郎",
                Birthday = "1999/01/01",
                Sex = "Male",
                郵便番号 = "338-0013",
                住所 = "埼玉県さいたま市中央区鈴谷６－４－５",
                連絡先電話番号 = "048-855-9911",
                自宅電話番号 = "048-855-9922",
                主担当医Object = Doctor.FOR_DEBUG,
                保険者 = new List<Hokensha>
          {
            new Hokensha
            {
              保険者番号 = "11223344",
              被保険者記号 = "あいう",
              被保険者番号 = "9876543210",
              被保険者氏名 = "テスト 太郎",
              被保険者枝番 = "01",
              続柄 = "本人",
              職務上の事由 = "None",
              資格取得日 = "2014/06/04",
              保険有効期限 = "2021/07/31",
              Remarks = "",
              SpecialInsuranceNumber = "",
              ValidityPeriodEnd = ""
            }
          }
            };
        }
    }
}
