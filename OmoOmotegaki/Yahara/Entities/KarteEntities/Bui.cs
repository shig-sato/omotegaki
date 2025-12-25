#nullable enable

using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

namespace omotegaki_xml.Libs.Yahara.Entities.KarteEntities
{
    public class Bui
    {
        [XmlAttribute]
        public string? No { get; set; }

        [XmlAttribute]
        public string? Teeth { get; set; }

        [XmlAttribute]
        public string? Doctor { get; set; }

        [XmlElement("Disease")]
        public List<Disease> Diseases { get; set; } = new List<Disease>();

        [XmlElement("Treatment")]
        public List<Treatment> Treatments { get; set; } = new List<Treatment>();
    }
}
