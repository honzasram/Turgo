using Sramek.FX;
using System;
using System.Collections.Generic;
using Turgo.Common;
using Turgo.Common.Model;

namespace Turgo.Controller
{
    public class TurgoController : Controller<TurgoController>
    {
        
        public List<User> GetUserBase()
        {
            InternalLoad();

            return TurgoSettings.I.BaseClassConfiguration.UserBaseList;
        }

        public void UpdateUserBase(List<User> aUsers)
        {
            TurgoSettings.I.BaseClassConfiguration.UserBaseList = aUsers;
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