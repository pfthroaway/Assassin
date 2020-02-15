﻿using Assassin.Models;
using Assassin.Models.Entities;
using Assassin.Views.Bank;
using Assassin.Views.Battle;
using Assassin.Views.Player;
using Assassin.Views.Shopping;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace Assassin.Views.City
{
    /// <summary>Interaction logic for GamePage.xaml</summary>
    public partial class GamePage : INotifyPropertyChanged
    {
        /// <summary>If the character is newly created, display this text.</summary>
        internal void NewUser() => TxtGame.Text =
                $"Creare An Vita, {GameState.CurrentUser.Name}!\n\nYou enter the city of thieves to take your place among the legends!";

        #region Data-Binding

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>Notifys the PropertyChanged event alerting the WPF Framework to update the UI.</summary>
        /// <param name="propertyNames">The names of the properties to update in the UI.</param>
        protected void NotifyPropertyChanged(params string[] propertyNames)
        {
            if (PropertyChanged != null)
            {
                foreach (string propertyName in propertyNames)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                }
            }
        }

        /// <summary>Notifys the PropertyChanged event alerting the WPF Framework to update the UI.</summary>
        /// <param name="propertyName">The optional name of the property to update in the UI. If this is left blank, the name will be taken from the calling member via the CallerMemberName attribute.</param>
        protected virtual void NotifyPropertyChanged([CallerMemberName]string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion Data-Binding

        #region Button-Click Methods

        private void BtnExit_Click(object sender, RoutedEventArgs e) => ClosePage();

        private void BtnMessages_Click(object sender, RoutedEventArgs e)
        {
            //TODO Implement messages.
        }

        private void BtnOptions_Click(object sender, RoutedEventArgs e)
        {
            //TODO Implement Options.
        }

        private void BtnPub_Click(object sender, RoutedEventArgs e)
        {
            //TODO Implement Pub
        }

        private void BtnInn_Click(object sender, RoutedEventArgs e)
        {
            //TODO Implement Inn
        }

        private void BtnJail_Click(object sender, RoutedEventArgs e)
        {
            //TODO Implement Jail
        }

        private void BtnGuild_Click(object sender, RoutedEventArgs e)
        {
            //TODO Implement Guilds.
        }

        private void BtnOthers_Click(object sender, RoutedEventArgs e)
        {
            //TODO Implement Others
        }

        private void BtnMystic_Click(object sender, RoutedEventArgs e)
        {
            //TODO Implement Mystic
        }

        private void BtnChapel_Click(object sender, RoutedEventArgs e)
        {
            //TODO Implement Chapel
        }

        private void BtnRob_Click(object sender, RoutedEventArgs e)
        {
            //TODO Implement Robbing
        }

        private void BtnAssassinate_Click(object sender, RoutedEventArgs e) => GameState.Navigate(new AssassinationPage());

        private void BtnInventory_Click(object sender, RoutedEventArgs e) => GameState.Navigate(new InventoryPage());

        private void BtnShops_Click(object sender, RoutedEventArgs e) => GameState.Navigate(new ShopsPage());

        private void BtnTrain_Click(object sender, RoutedEventArgs e) => GameState.Navigate(new TrainPage());

        private void BtnBank_Click(object sender, RoutedEventArgs e) => GameState.Navigate(new BankPage());

        #endregion Button-Click Methods

        #region Page-Manipulation Methods

        /// <summary>Closes the Page and saves the current <see cref="User"/>.</summary>
        private async void ClosePage()
        {
            GameState.GoBack();
            await GameState.DatabaseInteraction.SaveUser(GameState.CurrentUser);
        }

        public GamePage() => InitializeComponent();

        private void GamePage_OnLoaded(object sender, RoutedEventArgs e) => DataContext = GameState.CurrentUser;

        #endregion Page-Manipulation Methods
    }
}