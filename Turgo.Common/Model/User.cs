using System.Xml.Serialization;

namespace Turgo.Common.Model
{
    /// <summary>
    /// player
    /// </summary>
    public class User : ISelectable, IChangeable
    {
        [XmlAttribute]
        public string Name { get; set; }
        [XmlAttribute]
        public string Surname { get; set; }
        [XmlAttribute]
        public uint ID { get; set; }

        [XmlIgnore]
        public bool IsSelected { get; set; }
        [XmlIgnore]
        public bool IsChanged { get; set; }
    }
}