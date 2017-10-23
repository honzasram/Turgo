using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        
    }

    /// <summary>
    /// kurt
    /// </summary>
    public class Court
    {
        
    }

    /// <summary>
    /// kolo
    /// </summary>
    public class Round
    {
        public List<User> AttentedPlayers { get; set; }
    }

    /// <summary>
    /// rocnik
    /// </summary>
    public class Class
    {
        
    }
}
