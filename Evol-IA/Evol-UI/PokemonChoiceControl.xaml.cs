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
    /// Logique d'interaction pour PokemonChoiceControl.xaml
    /// </summary>
    public partial class PokemonChoiceControl : UserControl
    {
        public static readonly DependencyProperty TrainernProperty =
        DependencyProperty.RegisterAttached("Trainer", typeof(TrainerVM), typeof(PokemonChoiceControl));

        public TrainerVM Trainer
        {
            get { return (TrainerVM)GetValue(TrainernProperty); }
            set { SetValue(TrainernProperty, value); }
        }
        public PokemonChoiceControl()
        {
            InitializeComponent();
            LayoutRoot.DataContext = this;
        }
    }
}
