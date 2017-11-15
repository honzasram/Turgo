﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dragablz;
using System.Windows;

namespace Sramek.FX.WPF.ViewModel
{
    public class BaseWindowViewModel : ObservableObject
    {
        protected readonly Dictionary<string,Func<object>> mFactoryFuncDictionary = new Dictionary<string, Func<object>>();
        
        public ObservableCollection<TabContent> TabContents { get; } = new ObservableCollection<TabContent>();

        public IInterTabClient InterTabClient { get; }

        public BaseWindowViewModel()
        {
            InterTabClient = new DefaultInterTabClient();
        }

        public TabContent CreateContent(string aName)
        {
            if (mFactoryFuncDictionary.ContainsKey(aName))
            {
                var lFunc = mFactoryFuncDictionary[aName];
                return lFunc.Invoke() as TabContent;
            }
            else
            {
                return null;
            }
        }

        public void ShowTab(string aKey)
        {
            var lContent = CreateContent(aKey);
            TabContents.Add(lContent);
        }
    }
}
