using Turgo.ViewModel;

namespace Turgo.View
{
    /// <summary>
    /// Interaction logic for ClassView.xaml
    /// </summary>
    public partial class ClassView
    {
        public ClassView()
        {
            InitializeComponent();
            DataContext = new ClassViewModel();
        }
    }
}
