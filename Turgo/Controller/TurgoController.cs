﻿using Sramek.FX;
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
            return TurgoSettings.I.Model.ClassList[0].UserBase;
        }

        public void SaveRound(Round aRound)
        {
            TurgoSettings.I.Model.ClassList[0].Rounds.Add(aRound);
            TurgoSettings.Save(TurgoSettings.I);
        }

        public List<Round> GetAllRounds()
        {
            return TurgoSettings.I.Model.ClassList[0].Rounds.ToList();
        }

        public void UpdateUserBase(List<User> aUsers)
        {
            TurgoSettings.I.Model.ClassList[0].UserBase = aUsers;
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
        }
    }
}