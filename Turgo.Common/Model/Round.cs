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
        public int Number { get; set; }
        [XmlAttribute]
        public DateTime DateTime { get; set; }
        [XmlAttribute]
        public string Place { get; set; }
        [XmlAttribute]
        public string Description { get; set; }
        [XmlAttribute]
        public int CourtCount { get; set; }
        [XmlAttribute]
        public int SetCountPerGame { get; set; } = 2;
        [XmlAttribute]
        public int MaximumPointsPerSet { get; set; } = 21;

        public List<User> AttentedPlayers { get; set; }
        public List<Game> Games { get; set; }
    }
}