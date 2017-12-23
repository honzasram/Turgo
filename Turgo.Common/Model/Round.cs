using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Turgo.Common.Model
{
    /// <summary>
    /// kolo
    /// </summary>
    public class Round
    {
        [XmlAttribute]
        public DateTime DateTime { get; set; }
        [XmlAttribute]
        public string Place { get; set; }
        public string Describtion { get; set; }
        [XmlAttribute]
        public int CourtCount { get; set; }
        public List<User> AttentedPlayers { get; set; }
        public List<Game> Games { get; set; }
    }
}