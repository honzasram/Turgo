using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Sramek.FX
{
    public class ObservableObject : INotifyPropertyChanged
    {
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
