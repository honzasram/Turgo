using System.Windows.Controls;
using Turgo.ViewModel;

namespace Turgo.View
{
    /// <summary>
    /// Interaction logic for ResultsView.xaml
    /// </summary>
    public partial class ResultsView : UserControl
    {
        public ResultViewModel VM => DataContext as ResultViewModel;
        public ResultsView(ResultViewModel aVM)
        {
            InitializeComponent();
            DataContext = aVM;
        }
    }
}
