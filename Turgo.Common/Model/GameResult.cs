using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace Turgo.Common.Model
{
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
}