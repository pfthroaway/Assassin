using Assassin.Classes;
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

namespace Assassin.Pages.Shopping
{
    /// <summary>Interaction logic for WeaponsRUsPage.xaml</summary>
    public partial class WeaponsRUsPage : Page
    {
        public WeaponsRUsPage() => InitializeComponent();

        private void Page_Loaded(object sender, RoutedEventArgs e) => GameState.CalculateScale(Grid);
    }
}