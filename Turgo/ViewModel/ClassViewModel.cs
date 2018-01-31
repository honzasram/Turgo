using System;
using System.Windows.Input;
using Sramek.FX;
using Sramek.FX.WPF;
using Turgo.Common.Model;
using Turgo.Controller;

namespace Turgo.ViewModel
{
    public class ClassViewModel : StandartViewModel
    {
        private FullyObservableCollection<Class> mClassList;
        public FullyObservableCollection<Class> ClassList
        {
            get { return mClassList; }
            set
            {
                if (mClassList == value) return;
                mClassList = value;
                OnPropertyChanged();
            }
        }

        private Class mSelectedClass;
        public Class SelectedClass
        {
            get { return mSelectedClass; }
            set
            {
                if (mSelectedClass == value) return;
                mSelectedClass = value;
                OnPropertyChanged();
            }
        }

        public ICommand SelectClassCommand => new RelayCommand(() =>
        {
            TurgoSettingsController.I.SelectClass(SelectedClass);
            UpdateClasses();
        });

        public ICommand NewClassCommand => new RelayCommand(() =>
        {
            var lName = StandardMetroViewService.I.UserInputDialog("Jméno tøídy", "Vložte jméno nové tøídy.");
            var lStandartBase =  StandardMetroViewService.I.OkCancelQuestion("Hráèi", "Zahrnout hráèe ze standartního seznamu?");
            TurgoSettingsController.I.CreateNew(lName, DateTime.Now.Year, lStandartBase);
            UpdateClasses();
        });

        public ICommand EndClassCommand => new RelayCommand(() =>
        {
            SelectedClass.Finished = !SelectedClass.Finished;
            TurgoSettingsController.I.SaveModel();
        });

        public ICommand DeleteClassCommand => new RelayCommand(() =>
        {
            TurgoSettingsController.I.DeleteClass(SelectedClass);
            UpdateClasses();
        });

        public ClassViewModel()
        {
            UpdateClasses();
        }

        private void UpdateClasses()
        {
            var lClasses = TurgoSettingsController.I.GetAllClasses();
            ClassList = new FullyObservableCollection<Class>(lClasses);
        }
    }
}
