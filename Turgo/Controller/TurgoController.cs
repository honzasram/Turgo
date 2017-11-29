using Sramek.FX;
using System;
using System.Collections.Generic;
using System.Linq;
using Turgo.Common;
using Turgo.Common.Model;

namespace Turgo.Controller
{
    public class TurgoController : Controller<TurgoController>
    {
        
        public List<User> GetUserBase()
        {
            InternalLoad();

            return TurgoSettings.I.Model.ClassList[0].UserBase;
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