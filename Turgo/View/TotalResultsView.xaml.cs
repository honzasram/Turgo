using Turgo.ViewModel;

namespace Turgo.View
{
    /// <summary>
    /// Interaction logic for TotalResultsView.xaml
    /// </summary>
    public partial class TotalResultsView 
    {
        public TotalResultsView()
        {
            InitializeComponent();
            DataContext = new TotalResultsViewModel();
        }
    }
}
