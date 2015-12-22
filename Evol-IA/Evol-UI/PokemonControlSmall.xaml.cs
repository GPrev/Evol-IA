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
    /// Logique d'interaction pour PokemonControlSmall.xaml
    /// </summary>
    public partial class PokemonControlSmall : UserControl
    {
        public static readonly DependencyProperty PokemonProperty =
        DependencyProperty.RegisterAttached("Pokemon", typeof(PokemonVM), typeof(PokemonControlSmall));

        public PokemonVM Pokemon
        {
            get { return (PokemonVM)GetValue(PokemonProperty); }
            set { SetValue(PokemonProperty, value); }
        }

        public PokemonControlSmall()
        {
            InitializeComponent();
            LayoutRoot.DataContext = this;
        }
    }
}
