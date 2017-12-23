using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Win32;

namespace Sramek.FX.WPF
{
    public interface IViewService
    {
        bool OkCancelQuestion(string aHeader, string aMessage);
        void Message(string aHeader, string aMessage);
        T Execute<T>(Func<T> aAction);
        bool SaveFileDialog(out string aPath, string aExt, string aFilter);

    }
    
    public class StandardMetroViewService : StaticInstance<StandardMetroViewService>, IViewService
    {
        private readonly MetroWindow mWindow;

        public StandardMetroViewService() { }

        public StandardMetroViewService(MetroWindow aWindow)
        {
            mWindow = aWindow;
            I = this;
        }

        private async Task ShowMessageAsync(string aTitle, string aText)
        {
            await mWindow.ShowMessageAsync(aTitle, aText);
        }

        public bool OkCancelQuestion(string aHeader, string aMessage)
        {
            return mWindow.ShowModalMessageExternal(aHeader, aMessage, MessageDialogStyle.AffirmativeAndNegative,
                       new MetroDialogSettings {AnimateHide = false}) == MessageDialogResult.Affirmative;
        }

        public async void Message(string aHeader, string aMessage)
        {
            await ShowMessageAsync(aHeader, aMessage);
        }

        public T Execute<T>(Func<T> aAction)
        {
            Dispatcher lDispatcher = GetUIDispatcher(mWindow);
            
            if (lDispatcher == null)
                return aAction();
            else
                return lDispatcher.Invoke(aAction);
        }

        public bool SaveFileDialog(out string aPath, string aExt, string aFilter)
        {
            aPath = null;
            var lDialog = new SaveFileDialog { AddExtension = true, DefaultExt = aExt, Filter = aFilter};
            if (lDialog.ShowDialog(mWindow) ?? false)
            {
                aPath = lDialog.FileName;
                return true;
            }
            return false;
        }

        private static Dispatcher GetUIDispatcher(Window aWindow)
        {
            Dispatcher lDispatecher;
            if (aWindow != null && aWindow.Dispatcher != null)
                lDispatecher = aWindow.Dispatcher;
            else
                lDispatecher = Application.Current.Dispatcher;

            if (lDispatecher != null && !lDispatecher.CheckAccess())
                return lDispatecher;
            else
                return null;
        }
    }
}
