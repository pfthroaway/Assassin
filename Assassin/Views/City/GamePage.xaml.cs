using Assassin.Models;
using Assassin.Models.Entities;
using Assassin.Models.Enums;
using Assassin.Views.Battle;
using Assassin.Views.Guilds;
using Assassin.Views.Player;
using Assassin.Views.Shopping;
using Extensions;
using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

namespace Assassin.Views.City
{
    /// <summary>Interaction logic for GamePage.xaml</summary>
    public partial class GamePage
    {
        private bool blnAwake;
        private TimeSpan jailTimeSpan;
        private JailedUser jailedUser;
        private DispatcherTimer Timer1 = new DispatcherTimer();
        private readonly SolidColorBrush defaultBrush = (SolidColorBrush)new BrushConverter().ConvertFrom("#CCCCCC");

        /// <summary>Displays the appropriate text when a user awakens.</summary>
        internal async void Awaken()
        {
            if (GameState.CurrentUser.Alive)
            {
                switch (GameState.CurrentUser.CurrentLocation)
                {
                    case SleepLocation.Streets:
                        int loseEnd = Functions.GenerateRandomNumber(1, 5);
                        Functions.AddTextToTextBox(TxtGame, $"You awaken on the streets. After a rough night sleeping on the ground, you have lost {loseEnd} Endurance.");
                        GameState.CurrentUser.CurrentEndurance -= loseEnd;
                        if (GameState.CurrentUser.CurrentEndurance < 1)
                            GameState.CurrentUser.CurrentEndurance = 1;
                        break;

                    case SleepLocation.Jail:
                        CheckJailed();
                        break;

                    case SleepLocation.Inn:
                        Functions.AddTextToTextBox(TxtGame, "You awaken in the inn. You feel refreshed. You exit to the streets.");
                        GameState.CurrentUser.CurrentEndurance += 10;
                        break;

                    case SleepLocation.Guild:
                        Functions.AddTextToTextBox(TxtGame, "You awaken in the guild. You exit to the streets.");
                        break;
                }
            }
            else
            {
                Functions.AddTextToTextBox(TxtGame, "You were slain. You have been resurrected by the gods.");
                GameState.CurrentUser.Alive = true;
                GameState.CurrentUser.CurrentEndurance = 1;
            }

            GameState.CurrentUser.CurrentLocation = SleepLocation.Streets;
            await GameState.DatabaseInteraction.SaveUser(GameState.CurrentUser);
            blnAwake = true;
            Display();
        }

        /// <summary>Checks whether a <see cref="JailedUser"/> has served their time.</summary>
        public async void CheckJailed()
        {
            jailedUser = GameState.AllJailedUsers.Find(user => user.Name == GameState.CurrentUser.Name);
            jailTimeSpan = DateTime.UtcNow - jailedUser.DateJailed;
            if (jailTimeSpan.Seconds >= jailedUser.Fine / 10)
            {
                Functions.AddTextToTextBox(TxtGame, "You awaken in a jail cell. A guard looms over you.\nHe barks at you, \"You're free to go!\"\nYou briskly leave the jail.");
                await GameState.DatabaseInteraction.FreeFromJail(jailedUser);
                GameState.CurrentUser.CurrentLocation = SleepLocation.Streets;
                ToggleButtons(true);

                Display();

                await GameState.DatabaseInteraction.SaveUser(GameState.CurrentUser);
            }
            else
            {
                ToggleButtons(false);
                Functions.AddTextToTextBox(TxtGame, $"You awaken in a jail cell. You have not yet finished your sentence. You have {(jailedUser.Fine / 10) - jailTimeSpan.Seconds} seconds remaining.");
                Timer1.Start();
            }
        }

        /// <summary>Sets the foreground color of some TextBlocks based on <see cref="User"/>'s stats.</summary>
        private void Display()
        {
            LblHunger.Foreground = GameState.CurrentUser.Hunger < 20 ? defaultBrush : Brushes.Red;
            LblThirst.Foreground = GameState.CurrentUser.Thirst < 20 ? defaultBrush : Brushes.Red;
            LblWeaponName.Foreground = GameState.CurrentUser.CurrentWeapon.Name == "Hands" ? Brushes.Red : defaultBrush;
            LblArmorName.Foreground = GameState.CurrentUser.Armor.Name == "Clothes" ? Brushes.Red : defaultBrush;
            LblEndAmt.Foreground = GameState.CurrentUser.EnduranceRatio <= 0.2m ? Brushes.Red : defaultBrush;
        }

        /// <summary>If the character is newly created, display this text.</summary>
        internal void NewUser() => TxtGame.Text = $"Creare An Vita, {GameState.CurrentUser.Name}!\n\nYou enter the city of thieves to take your place among the legends!";

        /// <summary>Toggles all the Buttons on the Form.</summary>
        /// <param name="enabled">Should the Buttons be enabled?</param>
        public void ToggleButtons(bool enabled)
        {
            BtnAssassinate.IsEnabled = enabled;
            BtnBank.IsEnabled = enabled;
            BtnChapel.IsEnabled = enabled;
            BtnGuild.IsEnabled = enabled;
            BtnInn.IsEnabled = enabled;
            BtnInventory.IsEnabled = enabled;
            BtnJail.IsEnabled = enabled;
            BtnMessages.IsEnabled = enabled;
            BtnMystic.IsEnabled = enabled;
            BtnOptions.IsEnabled = enabled;
            BtnOthers.IsEnabled = enabled;
            BtnPub.IsEnabled = enabled;
            BtnRob.IsEnabled = enabled;
            BtnShops.IsEnabled = enabled;
            BtnTrain.IsEnabled = enabled;
        }

        #region Button-Click Methods

        private void BtnAssassinate_Click(object sender, RoutedEventArgs e) => GameState.Navigate(new AssassinationPage());

        private void BtnBank_Click(object sender, RoutedEventArgs e) => GameState.Navigate(new BankPage());

        private async void BtnChapel_Click(object sender, RoutedEventArgs e)
        {
            if (GameState.CurrentUser.EnduranceRatio <= 0.2m)
            {
                Functions.AddTextToTextBox(TxtGame, "The priest welcomes you into his chapel. He sees your grievous injuries and blesses you. You have been healed!");
                GameState.CurrentUser.CurrentEndurance = GameState.CurrentUser.MaximumEndurance;
                Display();
                await GameState.DatabaseInteraction.SaveUser(GameState.CurrentUser);
            }
            else
                Functions.AddTextToTextBox(TxtGame, "Sorry, the priest is currently holding mass. Please come again later.");
        }

        private void BtnExit_Click(object sender, RoutedEventArgs e) => ClosePage();

        private void BtnGuild_Click(object sender, RoutedEventArgs e) => GameState.Navigate(new GuildListPage());

        private void BtnInn_Click(object sender, RoutedEventArgs e)
        {
            //TODO Implement Inn
        }

        private void BtnInventory_Click(object sender, RoutedEventArgs e) => GameState.Navigate(new InventoryPage());

        private void BtnJail_Click(object sender, RoutedEventArgs e)
        {
            //TODO Implement Jail
        }

        private void BtnMessages_Click(object sender, RoutedEventArgs e)
        {
            //TODO Implement messages.
        }

        private void BtnMystic_Click(object sender, RoutedEventArgs e)
        {
            //TODO Implement Mystic
        }

        private void BtnOptions_Click(object sender, RoutedEventArgs e)
        {
            //TODO Implement Options.
        }

        private void BtnOthers_Click(object sender, RoutedEventArgs e)
        {
            //TODO Implement Others
        }

        private void BtnPub_Click(object sender, RoutedEventArgs e)
        {
            //TODO Implement Pub
        }

        private void BtnRob_Click(object sender, RoutedEventArgs e) => GameState.Navigate(new RobPage());

        private void BtnShops_Click(object sender, RoutedEventArgs e) => GameState.Navigate(new ShopsPage());

        private void BtnTrain_Click(object sender, RoutedEventArgs e) => GameState.Navigate(new TrainPage());

        #endregion Button-Click Methods

        #region Page-Manipulation Methods

        /// <summary>Closes the Page and saves the current <see cref="User"/>.</summary>
        private async void ClosePage()
        {
            GameState.GoBack();
            await GameState.DatabaseInteraction.SaveUser(GameState.CurrentUser);
        }

        public GamePage()
        {
            InitializeComponent();
            Timer1.Tick += Timer1_Tick;
        }

        private void GamePage_OnLoaded(object sender, RoutedEventArgs e)
        {
            DataContext = GameState.CurrentUser;
            GameState.GamePage = this;
            if (!blnAwake)
                Awaken();
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            jailTimeSpan = DateTime.UtcNow - jailedUser.DateJailed;
            if (jailTimeSpan.Seconds >= jailedUser.Fine / 10)
            {
                CheckJailed();
                Timer1.Stop();
            }
        }

        #endregion Page-Manipulation Methods
    }
}