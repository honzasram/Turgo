using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace Turgo.Common
{
    /// <summary>
    /// player
    /// </summary>
    public class User
    {
        [XmlAttribute]
        public string Name { get; set; }
        [XmlAttribute]
        public string Surname { get; set; }
        [XmlAttribute]
        public uint ID { get; set; }
    }

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

    public class GameResult
    {
        public List<Set> Sets { get; set; }
        [XmlIgnore]
        public bool AWinner => Sets.Select(a => a.SideA > a.SideB).Count(a => a)
                                   .CompareTo(Sets.Select(a => a.SideA < a.SideB).Count(a=>a)) == 1;
        [XmlIgnore]
        public bool BWinner => Sets.Select(a => a.SideA > a.SideB).Count(a => a)
                                   .CompareTo(Sets.Select(a => a.SideA < a.SideB).Count(a => a)) == -1;
    }

    public class Set
    {
        [XmlAttribute]
        public int SideA { get; set; }
        [XmlAttribute]
        public int SideB { get; set; }
    }

    ///// <summary>
    ///// kurt
    ///// </summary>
    //public class Court
    //{

    //}

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
