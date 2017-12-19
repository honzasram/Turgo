using Turgo.ViewModel;

namespace Turgo.View
{
    /// <summary>
    /// Interaction logic for RoundView.xaml
    /// </summary>
    public partial class RoundView
    {
        public RoundViewModel VM => DataContext as RoundViewModel;
        public RoundView(RoundViewModel aVm)
        {
            InitializeComponent();
            DataContext = aVm;
        }
    }
}
