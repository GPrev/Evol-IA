using Evol_IA;
using PokeRules;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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
using System.Xml.Serialization;

namespace Evol_UI
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static readonly DependencyProperty BattleProperty =
        DependencyProperty.RegisterAttached("Battle", typeof(BattleVM), typeof(MainWindow));

        public BattleVM Battle
        {
            get { return (BattleVM)GetValue(BattleProperty); }
            set { SetValue(BattleProperty, value); }
        }

        public MainWindow()
        {
            InitializeComponent();
            LayoutRoot.DataContext = this;

            Assembly a = Assembly.GetExecutingAssembly();

            Pokedex dex = Pokedex.ActivePokedex;
            dex.LoadAllPokemon(251);

            PokeData d1 = dex.GetData(3);
            PokeData d2 = dex.GetData(6);
            PokeData d3 = dex.GetData(9);
            PokeData d4 = dex.GetData(135);
            PokeData d5 = dex.GetData(196);
            PokeData d6 = dex.GetData(197);

            PokemonVM p1, p2, p3, p4, p5, p6;
            p1 = new PokemonVM(dex.GetData(3));
            p2 = new PokemonVM(dex.GetData(6));
            p3 = new PokemonVM(dex.GetData(9));
            p4 = new PokemonVM(dex.GetData(135));
            p5 = new PokemonVM(dex.GetData(196));
            p6 = new PokemonVM(dex.GetData(197));

            List<Pokemon> TeamA = new List<Pokemon>();
            List<Pokemon> TeamB = new List<Pokemon>();

            TeamAI t = new TeamAI(4, 3, new List<PokeData>() { d1, d2, d3, d4, d5, d6 },2,3);
            List< PokeData> iat= t.selectTeamAI();
            foreach(PokeData p in iat)
            {
                TeamB.Add(new PokemonVM(p));
            }
            //TeamB.Add(p1);
            //TeamB.Add(p2);
            //TeamB.Add(p3);
            TeamA.Add(p4);
            TeamA.Add(p5);
            TeamA.Add(p6);
            /*les deux lignes servent à créer la team via l'IA*/
            //TeamAI t = new TeamAI(4, 3, new List<PokeData>() { d1, d2, d3, d4, d5, d6 },2,3);
            //TeamB = t.selectTeamAI();
            List<Trainer> trainers  =new List<Trainer>();
            trainers.Add(new TrainerVM("Red", TeamA));
            trainers.Add(new TrainerVM("Blue", TeamB));

            List<BattleAI> ais = new List<BattleAI>();
            ais.Add(null); // Player
            ais.Add(new MinMaxAI(trainers[1])); // AI

            Battle = new BattleVM(ais, trainers);
        }

        private Stream GenerateStreamFromString(string s)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        private void OnLogChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            textBox.ScrollToEnd();
        }
    }
}
