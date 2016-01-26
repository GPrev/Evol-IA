using Evol_IA;
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
                return new MinMaxAI(t);
            //else
            if (type == "mcts")
                return new MctsAI(t);
            //else
            return null;
        }

        public void SetName(string name)
        {
            NameText.Text = name;
        }
    }
}
