using PokeMath;
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
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static readonly DependencyProperty TrainerAProperty =
        DependencyProperty.RegisterAttached("TrainerA", typeof(TrainerVM), typeof(MainWindow));

        public TrainerVM TrainerA
        {
            get { return (TrainerVM)GetValue(TrainerAProperty); }
            set { SetValue(TrainerAProperty, value); }
        }

        public static readonly DependencyProperty TrainerBProperty =
        DependencyProperty.RegisterAttached("TrainerB", typeof(TrainerVM), typeof(MainWindow));

        public TrainerVM TrainerB
        {
            get { return (TrainerVM)GetValue(TrainerBProperty); }
            set { SetValue(TrainerBProperty, value); }
        }

        public MainWindow()
        {
            InitializeComponent();
            LayoutRoot.DataContext = this;
            List<Move> movesA = new List<Move>();
            movesA.Add(new Move("MoveA", PokeMath.Type.BUG, 50, 80, false));
            movesA.Add(new Move("MoveB", PokeMath.Type.BUG, 50, 80, false));
            movesA.Add(new Move("MoveC", PokeMath.Type.BUG, 50, 80, false));
            List<Move> movesB = new List<Move>();
            movesB.Add(new Move("MoveD", PokeMath.Type.BUG, 50, 80, false));
            movesB.Add(new Move("MoveE", PokeMath.Type.BUG, 50, 80, false));
            List<Move> movesC = new List<Move>();
            movesC.Add(new Move("MoveF", PokeMath.Type.BUG, 50, 80, false));
            movesC.Add(new Move("MoveG", PokeMath.Type.BUG, 50, 80, false));
            List<Pokemon> TeamA = new List<Pokemon>();
            List<Pokemon> TeamB = new List<Pokemon>();
            TeamA.Add(new PokemonVM("pokA", 50, PokeMath.Type.BUG, PokeMath.Type.NONE, 50, 50, 50, 50, 50, 50, movesA));
            TeamA.Add(new PokemonVM("pokB", 50, PokeMath.Type.BUG, PokeMath.Type.NONE, 50, 50, 50, 50, 50, 50, movesB));
            TeamB.Add(new PokemonVM("pokC", 50, PokeMath.Type.BUG, PokeMath.Type.NONE, 50, 50, 50, 50, 50, 50, movesC));
            TrainerA = new TrainerVM("TrA", TeamA);
            TrainerB = new TrainerVM("TrB", TeamB);
        }
    }
}
