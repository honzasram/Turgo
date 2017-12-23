using System.Windows.Controls;
using Turgo.ViewModel;

namespace Turgo.View
{
    /// <summary>
    /// Interaction logic for PlayersView.xaml
    /// </summary>
    public partial class PlayersView : UserControl
    {
        public PlayersView()
        {
            InitializeComponent();
            DataContext = new PlayersViewModel();
        }
    }
}
