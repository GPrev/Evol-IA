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

            AIone.SetName("Red");
            AItwo.SetName("Blue");
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

        private void Battle_Click(object sender, RoutedEventArgs e)
        {
            List<Pokemon> TeamA = new List<Pokemon>();
            List<Pokemon> TeamB = new List<Pokemon>();
            foreach (Pokemon p in MatchMaker.TrainerA)
                TeamA.Add(new PokemonVM(p.data));
            foreach (Pokemon p in MatchMaker.TrainerB)
                TeamB.Add(new PokemonVM(p.data));

            List<Trainer> trainers = new List<Trainer>();
            trainers.Add(new TrainerVM("Red", TeamA));
            trainers.Add(new TrainerVM("Blue", TeamB));

            List<BattleAI> ais = new List<BattleAI>();
            ais.Add(AIone.MakeAI(trainers[0]));
            ais.Add(AItwo.MakeAI(trainers[1]));

            StartBattle(trainers, ais);
        }

        private void StartBattle(List<Trainer> trainers, List<BattleAI> ais)
        {
            foreach(Trainer t in trainers)
            {
                if (t.Team.Count == 0)
                    return;
            }
            Battle = new BattleVM(ais, trainers);
        }

        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            MatchMaker.Reset();
            Battle = null;
        }
    }
}
