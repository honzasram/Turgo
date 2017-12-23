using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dragablz;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Sramek.FX.WPF.ViewModel
{
    public class BaseWindowViewModel : ObservableObject
    {
        protected readonly Dictionary<string,Func<object,object>> mFactoryFuncDictionary = new Dictionary<string, Func<object,object>>();
        protected int mInstanceCount;

        public static BaseWindowViewModel I { get; protected set; }

        public ObservableCollection<TabContent> TabContents { get; } = new ObservableCollection<TabContent>();

        private TabContent mSelectedTab;
        public TabContent SelectedTab
        {
            get { return mSelectedTab; }
            set
            {
                if (mSelectedTab == value) return;
                mSelectedTab = value;
                OnPropertyChanged();
            }
        }

        public IInterTabClient InterTabClient { get; }

        public ICommand ShowTabCommand => new RelayCommand<string>(a => ShowTab(a));

        public BaseWindowViewModel()
        {
            InterTabClient = new DefaultInterTabClient();
            I = this;
        }

        public TabContent CreateContent(string aName, object aPassingObject = null)
        {
            if (mFactoryFuncDictionary.ContainsKey(aName))
            {
                var lFunc = mFactoryFuncDictionary[aName];
                return lFunc.Invoke(aPassingObject) as TabContent;
            }
            else
            {
                return null;
            }
        }

        public void ShowTab(string aKey, object aPassingObject = null)
        {
            var lContent = CreateContent(aKey, aPassingObject);
            if (lContent == null) return;

            TabContents.Add(lContent);
            SelectedTab = lContent;
        }

        public virtual void CloseTab(object aVMToRemove)
        {
            var lToRemoveList = new List<object>();
            foreach (var iTabContent in TabContents)
            {
                if ((iTabContent.Content as Control)?.DataContext == aVMToRemove)
                {
                    lToRemoveList.Add(iTabContent);
                }
            }

            foreach (TabContent iRemove in lToRemoveList)
            {
                TabContents.Remove(iRemove);
            }
        }
    }
}
