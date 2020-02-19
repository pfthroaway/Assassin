using Assassin.Models;
using Extensions;
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

namespace Assassin.Views.City
{
    /// <summary>Interaction logic for InnPage.xaml</summary>
    public partial class InnPage : Page
    {
        #region Click

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            Functions.AddTextToTextBox(GameState.GamePage.TxtGame, TxtInn.Text);
            GameState.GoBack();
        }

        private void BtnBribe_Click(object sender, RoutedEventArgs e)
        {
        }

        private void BtnRegistry_Click(object sender, RoutedEventArgs e)
        {
        }

        private void BtnSleep_Click(object sender, RoutedEventArgs e)
        {
        }

        #endregion Click

        #region Page-Manipulation Methods

        public InnPage()
        {
            InitializeComponent();
        }

        #endregion Page-Manipulation Methods
    }
}