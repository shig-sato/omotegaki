#nullable enable

using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace omotegaki_xml.Libs.Yahara.Entities.PatientEntities
{
    [XmlRoot("Patient")]
    public class Patient
    {
        [XmlAttribute]
        public string KarteNo { get; }

        [XmlAttribute]
        public string? Name { get; set; }

        [XmlAttribute]
        public string? KanjiName { get; set; }

        [XmlAttribute]
        public string? Birthday { get; set; }

        [XmlAttribute]
        public string? Sex { get; set; }

        [XmlAttribute]
        public string? 郵便番号 { get; set; }

        [XmlAttribute]
        public string? 住所 { get; set; }

        [XmlAttribute]
        public string? 連絡先電話番号 { get; set; }

        [XmlAttribute]
        public string? 自宅電話番号 { get; set; }

        [XmlIgnore]
        public Doctor? 主担当医Object { get; set; }

        [XmlAttribute]
        public string? 主担当医
        {
            get => 主担当医Object?.ToXML();
            set => 主担当医Object = Doctor.FromXML(value);
        }

        [XmlElement]
        public List<Hokensha> 保険者 { get; set; } = new List<Hokensha>();

        public Patient(string karteNo)
        {
            KarteNo = karteNo ?? throw new ArgumentNullException(nameof(karteNo));
        }

        private Patient()
        {
        }

        public string ToXML()
        {
            using var sw = new StringWriter();
            using var writer = XmlWriter.Create(sw, new XmlWriterSettings
            {
                OmitXmlDeclaration = true,
                Indent = true,
            });
            var namespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });

            var serializer = new XmlSerializer(typeof(Patient));
            serializer.Serialize(writer, this, namespaces);
            return sw.ToString();
        }
    }
}
