using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Sramek.FX;

namespace Turgo.Common.Model
{
    public class GameResult: ObservableObject
    {
        public List<Set> Sets { get; set; }

        [XmlIgnore]
        public bool AWinner => Sets.Select(a => a.SideA > a.SideB).Count(a => a)
                                   .CompareTo(Sets.Select(a => a.SideA < a.SideB).Count(a => a)) == 1;
        [XmlIgnore]
        public bool BWinner => Sets.Select(a => a.SideA > a.SideB).Count(a => a)
                                   .CompareTo(Sets.Select(a => a.SideA < a.SideB).Count(a => a)) == -1;
        //[XmlIgnore]
        //private bool mASideWon;
        //public bool ASideWon
        //{
        //    get { return mASideWon; }
        //    set
        //    {
        //        if (mASideWon == value) return;
        //        mASideWon = value;
        //        OnPropertyChanged();
        //    }
        //}
        //[XmlIgnore]
        //private bool mBSideWon;
        //public bool BSideWon
        //{
        //    get { return mBSideWon; }
        //    set
        //    {
        //        if (mBSideWon == value) return;
        //        mBSideWon = value;
        //        OnPropertyChanged();
        //    }
        //}
    }
}