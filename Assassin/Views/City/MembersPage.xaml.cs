using Assassin.Models;
using Assassin.Models.Entities;
using Assassin.Models.Enums;
using Assassin.Views.Battle;
using Assassin.Views.Guilds;
using Extensions;
using Extensions.DataTypeHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Assassin.Views.City
{
    /// <summary>Interaction logic for MembersPage.xaml</summary>
    public partial class MembersPage : Page
    {
        private string _previousPage;
        private List<User> _users;
        private User _selectedUser = new User();

        public bool _blnBribe { get; set; }
        private InnPage RefToInnPage { get; set; }

        /// <summary>Goes to the BattlePage.</summary>
        /// <param name="text">Text to be displayed on the BattlePage.</param>
        private void BattleFromInn(string text)
        {
            GameState.CurrentUser.GainHungerThirst();
            BattlePage battlePage = new BattlePage(true) { RefToInnPage = RefToInnPage };
            battlePage.AddText(text);
            GameState.CurrentEnemy = new Enemy(_selectedUser);
            GameState.Navigate(battlePage);
        }

        /// <summary>Checks the User's Hunger and Thirst to determine whether or not they can continue.</summary>
        /// <returns>Returns true if player isn't too hungry or thirst to continue.</returns>
        private bool CheckHungerThirst()
        {
            if (GameState.CurrentUser.DisplayHungerThirstText().Length > 0)
                GameState.DisplayNotification(GameState.CurrentUser.DisplayHungerThirstText(), "Assassin");
            return GameState.CurrentUser.CanDoAction();
        }

        /// <summary>Refreshes the list of members.</summary>
        private void RefreshItemsSource() => LstMembers.ItemsSource = _users;

        #region Click

        private void BtnAttack_Click(object sender, RoutedEventArgs e)
        {
            if (CheckHungerThirst())
            {
                GameState.CurrentUser.GainHungerThirst();
                GameState.CurrentEnemy = new Enemy(_selectedUser);
                GameState.Navigate(new BattlePage(true));
            }
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e) => GameState.GoBack();

        private void BtnBribe_Click(object sender, RoutedEventArgs e)
        {
            if (CheckHungerThirst())
            {
                if (_blnBribe)
                {
                    string bribeText = GameState.InputDialog("The inkeeper asks, \"How much gold would you give me to have this key?\"", "Assassin").Trim();
                    int bribe = Int32Helper.Parse(bribeText);
                    int bribeRequired = Functions.GenerateRandomNumber(_selectedUser.Level * 50, _selectedUser.Level * 200);

                    if (bribe > 0 && bribe <= GameState.CurrentUser.GoldOnHand)
                    {
                        GameState.CurrentUser.GoldOnHand -= bribe;
                        if (bribe < bribeRequired)
                        {
                            Functions.AddTextToTextBox(RefToInnPage.TxtInn, "The innkeeper takes your gold and walks away.");
                            GameState.GoBack();
                        }
                        else
                            BattleFromInn("The innkeeper takes your gold and hands you a key. You creep upstairs.");
                    }
                    else if (bribeText.Trim().Length > 0)
                        GameState.DisplayNotification($"Please enter a positive integer value less than {GameState.CurrentUser.GoldOnHand}.", "Assassin");
                    else if (bribe > GameState.CurrentUser.GoldOnHand)
                        Functions.AddTextToTextBox(RefToInnPage.TxtInn, "You don't have that much gold to bribe the innkeeper with.");
                }
                else
                {
                    if (GameState.CurrentUser.Lockpicks > 0)
                    {
                        GameState.CurrentUser.Lockpicks--;

                        if (Functions.GenerateRandomNumber(1, 100) <= GameState.CurrentUser.Stealth)
                            BattleFromInn("You successfully pick the lock! You enter the room...");
                        else
                            GameState.DisplayNotification("You broke your lockpick! Hopefully you didn't make too much noise and awaken your intended victim...", "Assassin");
                        //TODO Consider actually being surprised by your victim if you fail a lockpick.
                    }
                    else
                        GameState.DisplayNotification("You do not have any lockpicks.", "Assassin");
                }
            }
        }

        private async void BtnExpel_Click(object sender, RoutedEventArgs e)
        {
            if (await GameState.DatabaseInteraction.MemberLeavesGuild(_selectedUser, GameState.CurrentGuild))
            {
                await GameState.DatabaseInteraction.SendMessage(new Message(0, GameState.CurrentGuild.Master, _selectedUser.Name, $"You have been expelled from the guild {GameState.CurrentGuild.Name}.", DateTime.UtcNow, true));
                RefreshItemsSource();
            }
        }

        private void BtnMessage_Click(object sender, RoutedEventArgs e) => GameState.Navigate(new NewMessagePage(new List<User> { _selectedUser }));

        private void LstMembers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LstMembers.SelectedIndex >= 0)
            {
                _selectedUser = _users[LstMembers.SelectedIndex];
                switch (_previousPage)
                {
                    case "Game":
                        BtnAttack.IsEnabled = _selectedUser.Alive && _selectedUser.CurrentLocation == SleepLocation.Streets;
                        break;

                    case "Guild":
                        //You shouldn't attack your guildmates, and if you're not the master, you can't expel someone.
                        break;

                    case "Guild Manage":
                        BtnExpel.IsEnabled = true;
                        break;

                    case "Inn":
                        BtnBribe.IsEnabled = _selectedUser.Alive;
                        break;
                }
            }
            else
            {
                BtnAttack.IsEnabled = false;
                BtnExpel.IsEnabled = false;
            }
            BtnMessage.IsEnabled = LstMembers.SelectedIndex >= 0;
        }

        #endregion Click

        #region Page-Manipulation Methods

        public MembersPage(Page previousPage, bool blnBribe = false)
        {
            InitializeComponent();
            if (previousPage is GamePage)
            {
                _previousPage = "Game";
                _users = new List<User>(GameState.AllUsers);
                _users.Remove(GameState.CurrentUser);
            }
            else if (previousPage is GuildPage)
            {
                _previousPage = "Guild";
                _users = GameState.AllUsers.Where(user => GameState.CurrentGuild.Members.Contains(user.Name)).ToList();
            }
            else if (previousPage is GuildManagePage)
            {
                _previousPage = "Guild Manage";
                BtnExpel.Visibility = Visibility.Visible;
                _users = GameState.AllUsers.Where(user => GameState.CurrentGuild.Members.Contains(user.Name)).ToList();
            }
            else if (previousPage is InnPage)
            {
                _previousPage = "Inn";
                _blnBribe = blnBribe;
                _users = GameState.AllUsers.Where(user => user.CurrentLocation == SleepLocation.Inn).ToList();
                BtnBribe.Visibility = Visibility.Visible;
                if (!_blnBribe)
                    BtnBribe.Content = "_Use Lockpick";
                BtnMessage.Visibility = Visibility.Collapsed;
                BtnAttack.Visibility = Visibility.Collapsed;
                RefToInnPage = previousPage as InnPage;
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            RefreshItemsSource();
            if (LstMembers.Items.Count > 0)
                LstMembers.SelectedIndex = 0;
        }

        #endregion Page-Manipulation Methods
    }
}