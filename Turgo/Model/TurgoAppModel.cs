using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turgo.Common.Model;

namespace Turgo.Model
{
    public class TurgoAppModel
    {
        public ObservableCollection<User> SelectedUsers { get; set; }

        public static TurgoAppModel I { get; } = new TurgoAppModel();

        private TurgoAppModel()
        {
            
        }
    }
}
