using PokeRules;
using System;
using System.Collections.Generic;
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
    /// Logique d'interaction pour MoveChoiceControl.xaml
    /// </summary>
    public partial class MoveChoiceControl : UserControl
    {
        public static readonly DependencyProperty PokemonProperty =
        DependencyProperty.RegisterAttached("Pokemon", typeof(PokemonVM), typeof(MoveChoiceControl));

        public PokemonVM Pokemon
        {
            get { return (PokemonVM)GetValue(PokemonProperty); }
            set { SetValue(PokemonProperty, value); }
        }

        public static readonly DependencyProperty SelectedProperty =
        DependencyProperty.RegisterAttached("Selected", typeof(Move), typeof(MoveChoiceControl));

        public Move Selected
        {
            get { return (Move)GetValue(SelectedProperty); }
            set { SetValue(SelectedProperty, value); }

        }

        public MoveChoiceControl()
        {
            InitializeComponent();
            LayoutRoot.DataContext = this;
        }

        private void Move_Click(object sender, RoutedEventArgs e)
        {
            Control c = sender as Control;
            if(c != null)
            {
                Move m = c.DataContext as Move;
                Selected = m;
            }
        }
    }
}
