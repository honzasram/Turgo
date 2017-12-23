using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using MahApps.Metro.Controls.Dialogs;
using Sramek.FX.WPF;

namespace Turgo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private static int mInstances;
        public MainWindow()
        {
            InitializeComponent();
            if (mInstances == 0)
            {
                new StandardMetroViewService(this);
            }
            mInstances++;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (mInstances == 1)
            {
                e.Cancel = true;
                CancellationToken token;
                TaskScheduler uiSched = TaskScheduler.FromCurrentSynchronizationContext();
                Task.Factory.StartNew(DialogsBeforeExit, token, TaskCreationOptions.None, uiSched);
            }
            mInstances--;
        }
        
        private async void DialogsBeforeExit()
        {
            MessageDialogResult result = await this.ShowMessageAsync(this.Title, "Opravdu chcete zavřít tutop aplikaci?", MessageDialogStyle.AffirmativeAndNegative);
            if (result == MessageDialogResult.Negative)
            {
                return;
            }
            else
            {
                Application.Current.Shutdown();
            }
        }
    }
}
