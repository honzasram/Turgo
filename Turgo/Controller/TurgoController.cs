using Sramek.FX;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Turgo.Common;
using Turgo.Common.Model;

namespace Turgo.Controller
{
    public class TurgoController : StaticInstance<TurgoController>
    {
        public ObservableCollection<User> SelectedPlayers { get; set; }

        public List<User> GetUserBase()
        {
            InternalLoad();
            var lSelectedClass = TurgoSettings.I.SelectedClassIndex;
            return TurgoSettings.I.Model.ClassList[lSelectedClass].UserBase;
        }

        public List<User> GetStandardUserBase()
        {
            InternalLoad();
            return TurgoSettings.I.UserBaseList;
        }

        public void SaveRound(Round aRound)
        {
            var lSelectedClass = TurgoSettings.I.SelectedClassIndex;

            TurgoSettings.I.Model.ClassList[lSelectedClass].Rounds.Add(aRound);
            TurgoSettings.Save(TurgoSettings.I);
        }

        public List<Round> GetAllRounds()
        {
            var lSelectedClass = TurgoSettings.I.SelectedClassIndex;

            return TurgoSettings.I.Model.ClassList[lSelectedClass].Rounds.ToList();
        }

        public void UpdateUserBase(List<User> aUsers)
        {
            var lSelectedClass = TurgoSettings.I.SelectedClassIndex;

            TurgoSettings.I.Model.ClassList[lSelectedClass].UserBase = aUsers;
            foreach (var iUser in aUsers)
            {
                if (iUser.ID == 0)
                {
                    iUser.ID = aUsers.Max(a => a.ID) + 1;
                }
            }
            TurgoSettings.Save(TurgoSettings.I);
            TurgoSettings.Load();
        }

        public void UpdateStandardUserBase(List<User> aUsers)
        {
            TurgoSettings.I.UserBaseList = aUsers;
            foreach (var iUser in aUsers)
            {
                if (iUser.ID == 0)
                {
                    iUser.ID = aUsers.Max(a => a.ID) + 1;
                }
            }
            TurgoSettings.Save(TurgoSettings.I);
        }

        private void InternalLoad()
        {
            if (TurgoSettings.I == null)
            {
                TurgoSettings.Load();
            }

            TurgoSettings.I.Model.ClassList[TurgoSettings.I.SelectedClassIndex].Selected = true;
        }
    }

    public class TurgoSettingsController : StaticInstance<TurgoSettingsController>
    {
        public List<Class> GetAllClasses()
        {
            return TurgoSettings.I.Model.ClassList;
        }

        public void SaveModel()
        {
            TurgoSettings.Save(TurgoSettings.I);
        }

        public void SelectClass(Class aSelectedItem)
        {
            TurgoSettings.I.Model.ClassList.ForEach(a =>
            {
                a.Selected = false;
            });
            aSelectedItem.Selected = true;

            var lIndex = TurgoSettings.I.Model.ClassList.FindIndex(a=>a == aSelectedItem);
            TurgoSettings.I.SelectedClassIndex = lIndex;
            TurgoSettings.Save(TurgoSettings.I);
        }
        
        public void CreateNew(string aName, int aYear, bool aStandartBase)
        {
            ClassManager.CreateClass(TurgoSettings.I.Model, aStandartBase?TurgoSettings.I.UserBaseList:new List<User>(), aName, aYear);
            TurgoSettings.Save(TurgoSettings.I);
            TurgoSettings.Load();
        }
    }
}