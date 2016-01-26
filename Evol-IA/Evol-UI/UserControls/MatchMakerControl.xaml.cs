using Evol_IA;
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
    /// Logique d'interaction pour MatchMakerControl.xaml
    /// </summary>
    public partial class MatchMakerControl : UserControl
    {

        public static readonly DependencyProperty TrainerAProperty =
        DependencyProperty.RegisterAttached("TrainerA", typeof(ObservableCollection<Pokemon>), typeof(MatchMakerControl));

        public ObservableCollection<Pokemon> TrainerA
        {
            get { return (ObservableCollection<Pokemon>)GetValue(TrainerAProperty); }
            set { SetValue(TrainerAProperty, value); }
        }

        public static readonly DependencyProperty TrainerBProperty =
        DependencyProperty.RegisterAttached("TrainerB", typeof(ObservableCollection<Pokemon>), typeof(MatchMakerControl));

        public ObservableCollection<Pokemon> TrainerB
        {
            get { return (ObservableCollection<Pokemon>)GetValue(TrainerBProperty); }
            set { SetValue(TrainerBProperty, value); }
        }


        public static readonly DependencyProperty AvailableProperty =
        DependencyProperty.RegisterAttached("AvailablePokemon", typeof(List<Pokemon>), typeof(MatchMakerControl));

        public List<Pokemon> AvailablePokemon
        {
            get { return (List<Pokemon>)GetValue(AvailableProperty); }
            set { SetValue(AvailableProperty, value); }
        }

        public MatchMakerControl()
        {
            InitializeComponent();

            TrainerA = new ObservableCollection<Pokemon>();
            TrainerB = new ObservableCollection<Pokemon>();
            Pokedex.ActivePokedex.LoadAllPokemon(251);
            AvailablePokemon = new List<Pokemon>();
            List<PokeData> data = Pokedex.ActivePokedex.GetAllData(251);
            foreach (PokeData d in data)
                AvailablePokemon.Add(new PokemonVM(d));

            LayoutRoot.DataContext = this;
        }


        private void AddLeft_Click(object sender, RoutedEventArgs e)
        {
            Control c = sender as Control;
            if (c != null)
            {
                Pokemon p = c.DataContext as Pokemon;
                TrainerA.Add(new PokemonVM(p.data));
            }
        }


        private void AddRight_Click(object sender, RoutedEventArgs e)
        {
            Control c = sender as Control;
            if (c != null)
            {
                Pokemon p = c.DataContext as Pokemon;
                TrainerB.Add(new PokemonVM(p.data));
            }
        }

        private void Make3A_Click(object sender, RoutedEventArgs e) { MakeTeam(0, 3); }
        private void Make6A_Click(object sender, RoutedEventArgs e) { MakeTeam(0, 6); }
        private void Make3B_Click(object sender, RoutedEventArgs e) { MakeTeam(1, 3); }
        private void Make6B_Click(object sender, RoutedEventArgs e) { MakeTeam(1, 6); }

        private void MakeTeam(int teamId, int size)
        {
            TeamAI t = new TeamAI(20, size, Pokedex.ActivePokedex.GetAllData(251), 5, 5);
            List<PokeData> iat = t.selectTeamAI();

            ObservableCollection<Pokemon> team;
            if (teamId == 0)
                team = TrainerA;
            else //if (teamId == 1)
                team = TrainerB;

            team.Clear();
            foreach (PokeData p in iat)
            {
                team.Add(new PokemonVM(p));
            }
        }

        internal void Reset()
        {
            TrainerA.Clear();
            TrainerB.Clear();
        }
    }
}
