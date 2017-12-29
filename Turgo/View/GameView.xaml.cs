using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using MahApps.Metro.Controls;
using Sramek.FX.WPF.Converter;

namespace Turgo.View
{
    /// <summary>
    /// Interaction logic for GameView.xaml
    /// </summary>
    public partial class GameView
    {
        public GameView(int aSetCount)
        {
            InitializeComponent();
            for (int i = 0; i < aSetCount; i++)
                CreateNewSetTextboxes();
        }

        /// <summary>
        /// create new placeholdes for Set result
        /// </summary>
        private void CreateNewSetTextboxes()
        {
            SetGrid.RowDefinitions.Add(new RowDefinition {Height = new GridLength(1,GridUnitType.Star)});
            var lAtextbox = new NumericUpDown
            {
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Center,
                Minimum = 0
            };
            var lSetName = new TextBlock
            {
                Text = $"Set {SetGrid.RowDefinitions.Count}",
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
            };
            SetGrid.Children.Add(lAtextbox);
            SetGrid.Children.Add(lSetName);
            Grid.SetRow(lAtextbox, SetGrid.RowDefinitions.Count-1);
            Grid.SetColumn(lAtextbox, 0);
            Grid.SetRow(lSetName, SetGrid.RowDefinitions.Count-1);
            Grid.SetColumn(lSetName, 0);
            lAtextbox.SetBinding(NumericUpDown.ValueProperty, 
                new Binding($"Game.Result.Sets[{SetGrid.RowDefinitions.Count - 1}].SideA")
                {
                    Converter = new String2IntConverter()
                });
            
            var lDots = new TextBlock { HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center, Text = ":" };
            SetGrid.Children.Add(lDots);
            Grid.SetRow(lDots, SetGrid.RowDefinitions.Count - 1);
            Grid.SetColumn(lDots, 1);

            var lBtextbox = new NumericUpDown
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Center,
                Minimum = 0
            };
            SetGrid.Children.Add(lBtextbox);
            Grid.SetRow(lBtextbox, SetGrid.RowDefinitions.Count-1);
            Grid.SetColumn(lBtextbox, 2);
            lBtextbox.SetBinding(NumericUpDown.ValueProperty,
                new Binding($"Game.Result.Sets[{SetGrid.RowDefinitions.Count - 1}].SideB")
                {
                    Converter = new String2IntConverter()
                });
        }
    }
}
