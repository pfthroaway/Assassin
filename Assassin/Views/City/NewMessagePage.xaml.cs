using Assassin.Models;
using Assassin.Models.Entities;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Assassin.Views.City
{
    /// <summary>Interaction logic for NewMessagePage.xaml</summary>
    public partial class NewMessagePage : Page
    {
        private readonly List<User> _allUsers = new List<User>();

        #region Click

        private void BtnBack_Click(object sender, RoutedEventArgs e) => GameState.GoBack();

        private async void BtnSend_Click(object sender, RoutedEventArgs e)
        {
            Message newMessage = new Message(0, GameState.CurrentUser.Name, CmbRecipients.Text, TxtMessage.Text.Trim(), DateTime.UtcNow, false);

            if (await GameState.DatabaseInteraction.SendMessage(newMessage))
            {
                GameState.DisplayNotification("Message successfully sent.", "Assassin");
                GameState.GoBack();
            }
        }

        private void CmbRecipients_SelectionChanged(object sender, SelectionChangedEventArgs e) => BtnSend.IsEnabled = CmbRecipients.SelectedIndex >= 0 && TxtMessage.Text.Trim().Length > 0;

        #endregion Click

        #region Page-Manipulation Methods

        public NewMessagePage(List<User> users)
        {
            InitializeComponent();
            _allUsers = users;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            CmbRecipients.ItemsSource = _allUsers;
            if (_allUsers.Count > 0)
                CmbRecipients.SelectedIndex = 0;
            TxtMessage.Focus();
        }

        private void TxtMessage_TextChanged(object sender, TextChangedEventArgs e) => BtnSend.IsEnabled = CmbRecipients.SelectedIndex >= 0 && TxtMessage.Text.Trim().Length > 0;

        #endregion Page-Manipulation Methods
    }
}