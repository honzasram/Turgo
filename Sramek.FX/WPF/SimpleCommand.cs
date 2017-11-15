using System;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows;
using System.Threading.Tasks;
using log4net;

namespace Sramek.FX.WPF
{
    public class SimpleCommand : ICommand
    {
        public Predicate<object> CanExecuteDelegate { get; set; }
        public Action<object> ExecuteDelegate { get; set; }

        public bool CanExecute(object parameter)
        {
            if (CanExecuteDelegate != null)
                return CanExecuteDelegate(parameter);
            return true; // if there is no can execute default to true
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameter)
        {
            if (ExecuteDelegate != null)
                ExecuteDelegate(parameter);
        }
    }

    public abstract class RelayCommandBase2<T> : ICommand, INotifyPropertyChanged
    {
        #region Fields & Properties

        private static readonly ILog mLog = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly Action<T> mAction;
        private readonly Func<T, Task> mFunc;
        private readonly Func<T, bool> mCanExecute;

        private bool mIsExecuting;
        public bool IsExecuting
        {
            get { return mIsExecuting; }
            private set
            {
                if (mIsExecuting == value)
                    return;
                mIsExecuting = value;
                OnPropertyChanged();
                OnCanExecuteChanged();
            }
        }

        #endregion

        protected RelayCommandBase2(Action<T> aExecute, Func<T, bool> aCanExecute = null)
        {
            if (aExecute == null)
                throw new ArgumentNullException("aExecute");

            mAction = aExecute;
            mCanExecute = aCanExecute;
        }

        protected RelayCommandBase2(Func<T, Task> aExecute, Func<T, bool> aCanExecute = null)
        {
            if (aExecute == null)
                throw new ArgumentNullException("aExecute");

            mFunc = aExecute;
            mCanExecute = aCanExecute;
        }

        #region ICommand
        public bool CanExecute(object aArg)
        {
            var lArg = ConvertValue(aArg);
            return !mIsExecuting && (mCanExecute == null || mCanExecute(lArg));
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public virtual async void Execute(object aArg)
        {
            IsExecuting = true;
            try
            {
                var lArg = ConvertValue(aArg);

                if (mAction != null)
                {
                    mAction(lArg);
                    return;
                }

                if (mFunc != null)
                {
                    await mFunc(lArg);
                    return;
                }
            }
            catch (Exception lEx)
            {
                mLog.ErrorFormat("Executing command...error: {0}", Messages.BuildErrorMessage(lEx));
            }
            finally
            {
                IsExecuting = false;
            }
        }
        #endregion

        protected static T ConvertValue(object aValue)
        {
            if (!(aValue is IConvertible))
                return (T)aValue;
            
            var lType = Nullable.GetUnderlyingType(typeof(T)) ?? typeof(T);
            var lValue = (T)Convert.ChangeType(aValue, lType);
            return lValue;
        }

        protected void OnCanExecuteChanged()
        {
            var lCurrent = Application.Current;
            if (lCurrent == null)
                return;

            var lDispatcher = lCurrent.Dispatcher;
            if (lDispatcher == null || lDispatcher.HasShutdownStarted || lDispatcher.HasShutdownFinished)
                return;

            try
            {
                lDispatcher.Invoke((Action)(() => CommandManager.InvalidateRequerySuggested()));
            }
            catch { }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string aPropertyName = null)
        {
            PropertyChangedEventHandler lHandler = PropertyChanged;
            if (lHandler != null) lHandler(this, new PropertyChangedEventArgs(aPropertyName));
        }

    }

    public class RelayCommand : RelayCommandBase2<object>
    {
        public RelayCommand(Action aExecute, Func<bool> aCanExecute = null)
            : base(a => aExecute(), aCanExecute == null ? null : new Func<object, bool>(a => aCanExecute()))
        {
        }
    }
}
