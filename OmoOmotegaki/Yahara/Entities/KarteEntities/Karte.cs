#nullable enable

using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace omotegaki_xml.Libs.Yahara.Entities.KarteEntities
{
    [XmlRoot("Karte")]
    public class Karte
    {
        [XmlAttribute]
        public string? KarteNo { get; set; }

        [XmlAttribute]
        public string? Name { get; set; }

        [XmlAttribute]
        public string? Doctor { get; set; }

        [XmlElement("Day")]
        public List<Day> Days { get; set; } = new List<Day>();

        public string ToXML()
        {
            using var sw = new StringWriter();
            using var writer = XmlWriter.Create(sw, new XmlWriterSettings
            {
                OmitXmlDeclaration = true,
                Indent = true,
            });
            var namespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });

            var serializer = new XmlSerializer(typeof(Karte));
            serializer.Serialize(writer, this, namespaces);
            return sw.ToString();
        }
    }
}
