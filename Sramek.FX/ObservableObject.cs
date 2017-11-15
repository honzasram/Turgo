using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Sramek.FX
{
    [Serializable]
    public class ObservableObject : INotifyPropertyChanged
    {
        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName]string aPropertyName = null)
        {
            OnPropertyChangedInternal(aPropertyName);
        }

        protected virtual void OnPropertyChangedInternal(string aPropertyName)
        {
            var lPropChng = PropertyChanged;
            if (lPropChng != null)
            {
                lPropChng.Invoke(this, new PropertyChangedEventArgs(aPropertyName));
            }
        }

        protected virtual bool SetProperty<T>(ref T aValue, T aNewValue, [CallerMemberName]string aPropertyName = null)
        {
            if (!EqualityComparer<T>.Default.Equals(aValue, aNewValue))
            {
                aValue = aNewValue;
                OnPropertyChangedInternal(aPropertyName);
                return true;
            }

            return false;
        }
    }
}
