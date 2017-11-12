using System;
using System.Collections.Generic;
using System.Linq;

namespace Turgo.Common
{
    /// <summary>
    /// player
    /// </summary>
    public class User
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public uint ID { get; set; }
    }

    /// <summary>
    /// hra
    /// </summary>
    public class Game
    {
        public List<User> SideA { get; set; }
        public List<User> SideB { get; set; }
        public int CourtNumber { get; set; }
        public GameResult Result { get; set; }
    }

    public class GameResult
    {
        public List<Set> Sets { get; set; }

        public bool AWinner => Sets.Select(a => a.SideA > a.SideB).Count(a => a)
                                   .CompareTo(Sets.Select(a => a.SideA < a.SideB).Count(a=>a)) == 1;

        public bool BWinner => Sets.Select(a => a.SideA > a.SideB).Count(a => a)
                                   .CompareTo(Sets.Select(a => a.SideA < a.SideB).Count(a => a)) == -1;
    }

    public class Set
    {
        public int SideA { get; set; }
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
        public DateTime DateTime { get; set; }
        public string Place { get; set; }
        public string Describtion { get; set; }

        public int CourtCount { get; set; }
        public List<User> AttentedPlayers { get; set; }
        public List<Game> Games { get; set; }
    }

    /// <summary>
    /// rocnik
    /// </summary>
    public class Class
    {
        public List<User> UserBase { get; set; }
        public List<Round> Rounds { get; set; }
    }
}
