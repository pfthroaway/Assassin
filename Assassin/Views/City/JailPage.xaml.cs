using Assassin.Models;
using Assassin.Models.Entities;
using Assassin.Models.Enums;
using Extensions;
using System.Windows;
using System.Windows.Controls;

namespace Assassin.Views.City
{
    /// <summary>Interaction logic for JailPage.xaml</summary>
    public partial class JailPage
    {
        private JailedUser _selectedUser = new JailedUser();

        /// <summary>Refreshes the LstJail ItemsSource.</summary>
        private void RefreshItemsSource()
        {
            LstJail.ItemsSource = GameState.AllJailedUsers;
            LstJail.Items.Refresh();
        }

        #region Click

        private void BtnBack_Click(object sender, RoutedEventArgs e) => GameState.GoBack();

        private async void BtnBailOut_Click(object sender, RoutedEventArgs e)
        {
            Functions.AddTextToTextBox(GameState.GamePage.TxtGame, $"You bail out {_selectedUser.Name} for {_selectedUser.FineToString} gold.");
            GameState.CurrentUser.GoldOnHand -= _selectedUser.Fine;
            await GameState.DatabaseInteraction.FreeFromJail(_selectedUser);
            GameState.AllJailedUsers.Remove(_selectedUser);
            GameState.AllUsers.Find(user => user.Name == _selectedUser.Name).CurrentLocation = SleepLocation.Streets;
            await GameState.DatabaseInteraction.SaveUser(GameState.AllUsers.Find(user => user.Name == _selectedUser.Name));
            await GameState.DatabaseInteraction.SaveUser(GameState.CurrentUser);
            _selectedUser = new JailedUser();
            DataContext = _selectedUser;
            RefreshItemsSource();
        }

        private void LstJail_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LstJail.SelectedIndex >= 0)
            {
                _selectedUser = GameState.AllJailedUsers[LstJail.SelectedIndex];
                DataContext = _selectedUser;
            }
            BtnBailOut.IsEnabled = LstJail.SelectedIndex >= 0 && GameState.CurrentUser.GoldOnHand >= _selectedUser.Fine;
        }

        #endregion Click

        #region Page-Manipulation Methods

        public JailPage() => InitializeComponent();

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            RefreshItemsSource();
            if (LstJail.Items.Count > 0)
                LstJail.SelectedIndex = 0;
        }

        #endregion Page-Manipulation Methods
    }
}