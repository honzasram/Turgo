using System.Collections.Generic;
using System.Xml.Serialization;

namespace Turgo.Common.Model
{
    /// <summary>
    /// rocnik
    /// </summary>
    public class Class
    {
        [XmlAttribute]
        public string Name { get; set; }
        [XmlAttribute]
        public int Year { get; set; }
        public List<User> UserBase { get; set; }
        public List<Round> Rounds { get; set; }
    }
}