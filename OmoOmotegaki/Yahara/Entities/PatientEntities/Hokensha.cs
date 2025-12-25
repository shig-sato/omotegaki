#nullable enable

using System.Xml.Serialization;

namespace omotegaki_xml.Libs.Yahara.Entities.PatientEntities
{
    public class Hokensha
    {
        /// <summary>
        /// 全角
        /// </summary>
        [XmlAttribute]
        public string? 保険者番号 { get; set; }

        [XmlAttribute]
        public string? 被保険者記号 { get; set; }

        /// <summary>
        /// 半角
        /// </summary>
        [XmlAttribute]
        public string? 被保険者番号 { get; set; }

        [XmlAttribute]
        public string? 被保険者氏名 { get; set; }

        /// <summary>
        /// 半角
        /// </summary>
        [XmlAttribute]
        public string? 被保険者枝番 { get; set; }

        /// <summary>
        /// "本人" または "家族"
        /// </summary>
        [XmlAttribute]
        public string? 続柄 { get; set; }

        [XmlAttribute]
        public string? 職務上の事由 { get; set; }

        /// <summary>
        /// "yyyy/mm/dd"
        /// </summary>
        [XmlAttribute]
        public string? 資格取得日 { get; set; }

        /// <summary>
        /// "yyyy/mm/dd"
        /// </summary>
        [XmlAttribute]
        public string? 保険有効期限 { get; set; }

        [XmlAttribute("備考")]
        public string? Remarks { get; set; }

        [XmlAttribute("特殊保険番号")]
        public string? SpecialInsuranceNumber { get; set; }

        [XmlAttribute("有効期間終了")]
        public string? ValidityPeriodEnd { get; set; }
    }
}
