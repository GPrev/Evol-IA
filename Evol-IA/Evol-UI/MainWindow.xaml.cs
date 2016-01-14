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
            Stream file1 = GenerateStreamFromString(Properties.Resources.ResourceManager.GetString("_003"));
            Stream file2 = GenerateStreamFromString(Properties.Resources.ResourceManager.GetString("_006"));
            Stream file3 = GenerateStreamFromString(Properties.Resources.ResourceManager.GetString("_009"));
            Stream file4 = GenerateStreamFromString(Properties.Resources.ResourceManager.GetString("_135"));
            Stream file5 = GenerateStreamFromString(Properties.Resources.ResourceManager.GetString("_196"));
            Stream file6 = GenerateStreamFromString(Properties.Resources.ResourceManager.GetString("_197"));

            PokeData d1, d2, d3, d4, d5, d6;
            XmlSerializer serializer = new XmlSerializer(typeof(PokeData));
            d1 = (PokeData)serializer.Deserialize(file1);
            d2 = (PokeData)serializer.Deserialize(file2);
            d3 = (PokeData)serializer.Deserialize(file3);
            d4 = (PokeData)serializer.Deserialize(file4);
            d5 = (PokeData)serializer.Deserialize(file5);
            d6 = (PokeData)serializer.Deserialize(file6);

            PokemonVM p1, p2, p3, p4, p5, p6;
            p1 = new PokemonVM(d1);
            p2 = new PokemonVM(d2);
            p3 = new PokemonVM(d3);
            p4 = new PokemonVM(d4);
            p5 = new PokemonVM(d5);
            p6 = new PokemonVM(d6);

            List<Pokemon> TeamA = new List<Pokemon>();
            List<Pokemon> TeamB = new List<Pokemon>();
            TeamB.Add(p1);
            TeamB.Add(p2);
            TeamB.Add(p3);
            TeamA.Add(p4);
            TeamA.Add(p5);
            TeamA.Add(p6);
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
