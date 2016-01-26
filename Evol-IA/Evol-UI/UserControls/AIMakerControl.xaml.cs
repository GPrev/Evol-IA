using Evol_IA;
using PokeRules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Logique d'interaction pour AIMakerControl.xaml
    /// </summary>
    public partial class AIMakerControl : UserControl
    {
        public AIMakerControl()
        {
            InitializeComponent();
        }

        public BattleAI MakeAI(Trainer t)
        {
            t.Name = NameText.Text;

            string type = (AIType.SelectedItem as ComboBoxItem).Tag as string;

            if (type == "minmax")
            {
                int maxprof = Int32.Parse(MaxProfText.Text);
                return new MinMaxAI(t, maxprof);
            }
            //else
            if (type == "mcts")
            {
                int nbit = Int32.Parse(NbIteText.Text);
                int nbs = Int32.Parse(NbSimuText.Text);
                return new MctsAI(t, nbit, nbs);
            }
            //else
            return null;
        }

        public void SetName(string name)
        {
            NameText.Text = name;
        }

        private void PreviewNumericTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }
        private static bool IsTextAllowed(string text)
        {
            Regex regex = new Regex("[^0-9.-]+"); //regex that matches disallowed text
            return !regex.IsMatch(text);
        }
    }
}
