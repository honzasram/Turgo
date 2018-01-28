using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Xml.Serialization;
using Sramek.FX;

namespace Turgo.Common.Model
{
    /// <summary>
    /// hra
    /// </summary>
    public class Game : ObservableObject
    {
        public List<User> SideA { get; set; }
        public List<User> SideB { get; set; }
        [XmlAttribute]
        public int CourtNumber { get; set; }
        public GameResult Result { get; set; }

        [XmlIgnore]
        private bool mFinished;
        public bool Finished
        {
            get { return mFinished; }
            set
            {
                if (mFinished == value) return;
                mFinished = value;
                OnPropertyChanged();
                AssignName();
            }
        }

        [XmlIgnore]
        public object Parent { get; set; }
        
        [XmlIgnore]
        private string mShowName;
        [XmlIgnore]
        public string ShowName
        {
            get
            {
                if(string.IsNullOrEmpty(mShowName)) ShowName = GameNameFactory.GameName(this);
                return mShowName;
            }
            set
            {
                if (mShowName == value) return;
                mShowName = value;
                OnPropertyChanged();
            }
        }

        private void AssignName()
        {
            ShowName = GameNameFactory.GameName(this);
        }

        public override string ToString()
        {
            return GameNameFactory.GameName(this);
        }
    }

    public static class GameNameFactory
    {
        public static string GameName(Game aGame)
        {
            if (aGame.SideA == null || aGame.SideB == null)
                return "";
            var lANames = SideCompanions(aGame.SideA.Select(UserShortcutName));
            var lBNames = SideCompanions(aGame.SideB.Select(UserShortcutName));
            return $"{lANames} vs. {lBNames}" + (aGame.Finished?$" - Dohráno {Results(aGame.Result)}":"");
        }

        public static string Results(GameResult aGame)
        {
            return "";
        }

        public static string SideCompanions(IEnumerable<string> aNames)
        {
            return aNames.Aggregate((a, b) => $"{a}/{b}");
        }

        public static string UserShortcutName(User aUser)
        {
            if(!string.IsNullOrEmpty(aUser.Surname))
                return $"{aUser.Name} {aUser.Surname[0]}{aUser.Surname[1]}.";

            return $"{aUser.Name}";
        }

        public static string UserName(User aUser)
        {
            if (!string.IsNullOrEmpty(aUser.Surname))
                return $"{aUser.Name} {aUser.Surname}.";

            return $"{aUser.Name}";
        }
    }
}