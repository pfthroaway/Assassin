using Assassin.Models;
using Assassin.Models.Entities;
using Assassin.Models.Enums;
using Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Assassin.Views.City
{
    /// <summary>Interaction logic for InnPage.xaml</summary>
    public partial class InnPage : Page
    {
        /// <summary>Checks the User's Hunger and Thirst to determine whether or not they can continue.</summary>
        /// <returns>Returns true if player isn't too hungry or thirst to continue.</returns>
        private bool CheckHungerThirst()
        {
            Functions.AddTextToTextBox(TxtInn, GameState.CurrentUser.DisplayHungerThirstText());
            return GameState.CurrentUser.CanDoAction();
        }

        /// <summary>Closes the Page.</summary>
        private void ClosePage()
        {
            Functions.AddTextToTextBox(GameState.GamePage.TxtGame, TxtInn.Text.Trim());
            GameState.GoBack();
        }

        /// <summary>Disables all Buttons if the <see cref="User"/> has died.</summary>
        public void DisableButtons()
        {
            BtnBribe.IsEnabled = false;
            BtnLockpick.IsEnabled = false;
            BtnRegistry.IsEnabled = false;
            BtnSleep.IsEnabled = false;
        }

        #region Click

        private void BtnBack_Click(object sender, RoutedEventArgs e) => ClosePage();

        private void BtnBribe_Click(object sender, RoutedEventArgs e)
        {
            if (CheckHungerThirst())
            {
                List<User> guests = GameState.AllUsers.Where(user => user.CurrentLocation == SleepLocation.Inn).ToList();
                if (guests.Count > 0)
                    GameState.Navigate(new MembersPage(this, true));
                else
                    Functions.AddTextToTextBox(TxtInn, "There are no guests here.");
            }
        }

        private void BtnLockpick_Click(object sender, RoutedEventArgs e)
        {
            if (CheckHungerThirst() && GameState.CurrentUser.Lockpicks > 0)
            {
                List<User> guests = GameState.AllUsers.Where(user => user.CurrentLocation == SleepLocation.Inn).ToList();
                if (guests.Count > 0)
                    GameState.Navigate(new MembersPage(this));
                else
                    Functions.AddTextToTextBox(TxtInn, "There are no guests here.");
            }
            else if (GameState.CurrentUser.Lockpicks <= 0)
                Functions.AddTextToTextBox(TxtInn, "You do not have any lockpicks.");
        }

        private void BtnRegistry_Click(object sender, RoutedEventArgs e)
        {
            Functions.AddTextToTextBox(TxtInn, "You check the register.");

            List<User> guests = GameState.AllUsers.Where(user => user.CurrentLocation == SleepLocation.Inn).ToList();
            if (guests.Count > 0)
                Functions.AddTextToTextBox(TxtInn, string.Join("\n", guests.Select(user => user.Name)));
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