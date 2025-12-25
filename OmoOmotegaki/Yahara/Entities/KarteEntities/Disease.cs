#nullable enable

using System.Xml;
using System.Xml.Serialization;

namespace omotegaki_xml.Libs.Yahara.Entities.KarteEntities
{
    public class Disease
    {
        [XmlAttribute]
        public string? Name { get; set; }
    }
}
