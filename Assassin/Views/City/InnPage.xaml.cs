using Assassin.Models;
using Assassin.Models.Entities;
using Assassin.Models.Enums;
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
        /// <summary>Closes the Page.</summary>
        private void ClosePage()
        {
            Functions.AddTextToTextBox(GameState.GamePage.TxtGame, TxtInn.Text);
            GameState.GoBack();
        }

        #region Click

        private void BtnBack_Click(object sender, RoutedEventArgs e) => ClosePage();

        private void BtnBribe_Click(object sender, RoutedEventArgs e)
        {
            List<User> guests = GameState.AllUsers.Where(user => user.CurrentLocation == SleepLocation.Inn).ToList();
            if (guests.Count > 0)
                GameState.Navigate(new MembersPage(this));
            else
                Functions.AddTextToTextBox(TxtInn, "There are no guests here.");
        }

        private void BtnRegistry_Click(object sender, RoutedEventArgs e)
        {
            Functions.AddTextToTextBox(TxtInn, "You check the register.");

            List<User> guests = GameState.AllUsers.Where(user => user.CurrentLocation == SleepLocation.Inn).ToList();
            if (guests.Count > 0)
            {
                Functions.AddTextToTextBox(TxtInn, string.Join("\n", guests.Select(user => user.Name)));
            }
            else
                Functions.AddTextToTextBox(TxtInn, "There are no guests here.");
        }

        private void BtnSleep_Click(object sender, RoutedEventArgs e)
        {
            if (GameState.CurrentUser.GoldOnHand >= 100 && GameState.YesNoNotification("It will cost 100 gold for a room to sleep. Would you like to pay it and sleep here?", "Assassin"))
            {
                GameState.CurrentUser.GoldOnHand -= 100;
                GameState.CurrentUser.CurrentLocation = SleepLocation.Inn;
                GameState.GamePage.ToggleButtons(false);
                ClosePage();
            }
            else if (GameState.CurrentUser.GoldOnHand < 100)
                Functions.AddTextToTextBox(TxtInn, "You don't have enough gold on hand to sleep here.");
        }

        #endregion Click

        #region Page-Manipulation Methods

        public InnPage()
        {
            InitializeComponent();
            Functions.AddTextToTextBox(TxtInn, "You enter The Inn. The innkeeper smiles warmly from the reception desk nearby.");
        }

        #endregion Page-Manipulation Methods
    }
}