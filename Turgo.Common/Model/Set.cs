using System.Xml.Serialization;

namespace Turgo.Common.Model
{
    public class Set
    {
        [XmlAttribute]
        public int SideA { get; set; }
        [XmlAttribute]
        public int SideB { get; set; }
    }
}