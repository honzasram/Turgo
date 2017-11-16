using System.Collections.Generic;
using System.Xml.Serialization;

namespace Turgo.Common.Model
{
    /// <summary>
    /// hra
    /// </summary>
    public class Game
    {
        public List<User> SideA { get; set; }
        public List<User> SideB { get; set; }
        [XmlAttribute]
        public int CourtNumber { get; set; }
        public GameResult Result { get; set; }
    }
}