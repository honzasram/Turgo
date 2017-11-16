using System.Collections.Generic;
using Turgo.Common.Model;

namespace Turgo.Common
{
    public class ClassConfiguration
    {
        public ClassConfiguration()
        {
            UserBaseList = new List<User>();
        }

        public List<User> UserBaseList { get; set; }
    }
}