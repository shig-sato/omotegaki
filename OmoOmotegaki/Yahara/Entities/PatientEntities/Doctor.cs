#nullable enable

using System;

namespace omotegaki_xml.Libs.Yahara.Entities.PatientEntities
{
    public class Doctor
    {
        public static readonly Doctor FOR_DEBUG = new Doctor("担当医", "氏名");

        /// <summary>
        /// ドクター番号（半角）
        /// </summary>
        public string No { get; }

        /// <summary>
        /// ドクター名（全角）
        /// </summary>
        public string Name { get; }

        public Doctor(string no, string name)
        {
            No = no ?? throw new ArgumentNullException(nameof(no));
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        internal static Doctor? FromXML(string? value)
        {
            if (value == null) return null;

            var split = value.Split('_');
            return new Doctor(split[0], split[1]);
        }

        public string ToXML()
        {
            // debug
            if (No == "担当医" && Name == "氏名")
            {
                return "担当医 氏名";
            }

            return $"{No}_{Name}";
        }
    }
}
