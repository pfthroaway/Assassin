using Assassin.Models;
using Assassin.Models.Entities;
using Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Assassin.Views.Guilds
{
    /// <summary>Interaction logic for ManageApplicationsPage.xaml</summary>
    public partial class ManageApplicationsPage
    {
        private List<User> _applicants = new List<User>();
        private User _selectedUser = new User();

        private GuildManagePage RefToGuildManagePage { get; set; }

        /// <summary>Reloads all <see cref="User"/> applications and refreshes the ItemsSource.</summary>
        private async void RefreshItemsSource()
        {
            List<string> applicants = await GameState.DatabaseInteraction.LoadGuildApplicants(GameState.CurrentGuild);
            _applicants = GameState.AllUsers.Where(user => applicants.Contains(user.Name)).OrderBy(user => user.Name).ToList();
            LstApplications.ItemsSource = _applicants;
        }

        #region Click

        private async void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            GameState.GoBack();
            await GameState.DatabaseInteraction.SaveGuild(GameState.CurrentGuild);
        }

        private async void BtnApprove_Click(object sender, RoutedEventArgs e)
        {
            if (await GameState.DatabaseInteraction.ApproveGuildApplication(_selectedUser, GameState.CurrentGuild))
            {
                Functions.AddTextToTextBox(RefToGuildManagePage.TxtManage, $"You approved {_selectedUser}'s application to join {GameState.CurrentGuild.Name}.");
                RefreshItemsSource();
            }
        }

        private async void BtnDeny_Click(object sender, RoutedEventArgs e)
        {
            if (await GameState.DatabaseInteraction.DenyGuildApplication(_selectedUser, GameState.CurrentGuild))
            {
                Functions.AddTextToTextBox(RefToGuildManagePage.TxtManage, $"You denied {_selectedUser}'s application to join {GameState.CurrentGuild.Name}.");
                RefreshItemsSource();
            }
        }

        private void LstApplications_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedUser = LstApplications.SelectedIndex >= 0 ? _applicants[LstApplications.SelectedIndex] : new User();
            BtnApprove.IsEnabled = LstApplications.SelectedIndex >= 0;
            BtnDeny.IsEnabled = LstApplications.SelectedIndex >= 0;
        }

        #endregion Click

        #region Page-Manipulation Methods

        public ManageApplicationsPage(GuildManagePage guildManagePage)
        {
            InitializeComponent();
            RefToGuildManagePage = guildManagePage;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e) => RefreshItemsSource();

        #endregion Page-Manipulation Methods
    }
}