#nullable enable

using System.Xml;
using System.Xml.Serialization;

namespace omotegaki_xml.Libs.Yahara.Entities.KarteEntities
{
    public class Treatment
    {
        [XmlAttribute]
        public string? Point { get; set; }

        [XmlAttribute]
        public string? Count { get; set; }

        [XmlAttribute]
        public string? InnerBui { get; set; }

        [XmlText]
        public string? Name { get; set; }
    }
}
