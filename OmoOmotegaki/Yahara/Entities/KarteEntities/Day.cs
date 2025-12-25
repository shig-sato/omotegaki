#nullable enable

using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

namespace omotegaki_xml.Libs.Yahara.Entities.KarteEntities
{
    public class Day
    {
        [XmlAttribute]
        public string? Date { get; set; }

        [XmlAttribute]
        public string? DateNo { get; set; }

        [XmlAttribute]
        public string? Doctor { get; set; }

        [XmlElement("Bui")]
        public List<Bui> Buis { get; set; } = new List<Bui>();
    }
}
