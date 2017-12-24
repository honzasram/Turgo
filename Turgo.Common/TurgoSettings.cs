using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using Sramek.FX;
using Turgo.Common.Model;

namespace Turgo.Common
{
    public class TurgoSettings : SettingsBase<TurgoSettings>
    {
        /// <summary>
        /// index of selected class
        /// </summary>
        public int SelectedClassIndex { get; set; }
        /// <summary>
        /// default base
        /// </summary>
        public List<User> UserBaseList { get; set; }
        /// <summary>
        /// Model for storing data
        /// </summary>
        public TurgoModel Model { get; set; }

        public TurgoSettings()
        {
            Model = new TurgoModel();
            UserBaseList = new List<User>();
        }
    }

    public static class ClassManager
    {
        public static void CreateClass(TurgoModel aModel, List<User> aUserBase, string aName, int aYear)
        {
            if(aModel == null) throw new ArgumentNullException(nameof(aModel));
            if (aUserBase == null) throw new ArgumentNullException(nameof(aUserBase));

            if (aModel.ClassList == null)
            {
                aModel.ClassList = new List<Class>();
            }

            aModel.ClassList.Add(new Class
            {
                UserBase =  aUserBase,
                Year = aYear,
                Name = aName, 
                Rounds = new List<Round>()
            });
        }


    }
}