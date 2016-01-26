using PokeRules;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Evol_UI
{
    /// <summary>
    /// Logique d'interaction pour TeamEditControl.xaml
    /// </summary>
    public partial class TeamEditControl : UserControl
    {
        public static readonly DependencyProperty TrainernProperty =
        DependencyProperty.RegisterAttached("Trainer", typeof(ObservableCollection<Pokemon>), typeof(TeamEditControl));

        public ObservableCollection<Pokemon> Trainer
        {
            get { return (ObservableCollection<Pokemon>)GetValue(TrainernProperty); }
            set { SetValue(TrainernProperty, value); }
        }

        public TeamEditControl()
        {
            InitializeComponent();
            LayoutRoot.DataContext = this;
        }

        private void Pokemon_Click(object sender, RoutedEventArgs e)
        {
            Control c = sender as Control;
            if (c != null)
            {
                PokemonVM p = c.DataContext as PokemonVM;
                Trainer.Remove(p);
            }
        }
    }
}
