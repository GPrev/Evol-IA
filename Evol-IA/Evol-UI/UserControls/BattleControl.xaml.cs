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
    /// Logique d'interaction pour BattleControl.xaml
    /// </summary>
    public partial class BattleControl : UserControl
    {
        public static readonly DependencyProperty VMProperty =
        DependencyProperty.RegisterAttached("VM", typeof(BattleControlVM), typeof(BattleControl));

        public BattleControlVM VM
        {
            get { return (BattleControlVM)GetValue(VMProperty); }
            set { SetValue(VMProperty, value); }
        }
        public BattleControl()
        {
            InitializeComponent();
            LayoutRoot.DataContext = this;
        }
    }
}
