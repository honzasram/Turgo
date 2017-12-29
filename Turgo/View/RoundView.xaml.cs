using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
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
            PrepareView();
        }

        private void PrepareView()
        {
            var lCourtCount = VM.Round.CourtCount;
            PrepareGrid(lCourtCount);

            for (int i = 0; i < lCourtCount; i++)
            {
                var lGameView = new GameView(VM.Round.SetCountPerGame);
                lGameView.SetBinding(DataContextProperty, new Binding($"GameViewModelList[{i}]"));
                
                GameViews.Children.Add(lGameView);
                Grid.SetRow(lGameView, i);
            }
        }

        private void PrepareGrid(int aRows)
        {
            for (int i = 0; i < aRows; i++)
                GameViews.RowDefinitions.Add(new RowDefinition {Height = new GridLength(1, GridUnitType.Star)});
        }
    }
}
