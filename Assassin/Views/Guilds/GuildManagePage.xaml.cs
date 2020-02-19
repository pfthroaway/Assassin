using Assassin.Models;
using Assassin.Views.City;
using Extensions;
using System.Windows;

namespace Assassin.Views.Guilds
{
    /// <summary>Interaction logic for GuildManagePage.xaml</summary>
    public partial class GuildManagePage
    {
        private GuildPage RefToGuildPage { get; set; }

        /// <summary>Disable controls if the <see cref="User"/> gives up control of the <see cref="Guild"/>.</summary>
        public void DisableControls()
        {
            BtnApplications.IsEnabled = false;
            BtnHireHenchmen.IsEnabled = false;
            BtnMembers.IsEnabled = false;
            BtnOptions.IsEnabled = false;
            BtnTransfer.IsEnabled = false;
        }

        #region Click

        private void BtnApplications_Click(object sender, RoutedEventArgs e) => GameState.Navigate(new ManageApplicationsPage(this));

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            Functions.AddTextToTextBox(RefToGuildPage.TxtGuild, TxtManage.Text.Trim());
            GameState.GoBack();
        }

        private void BtnHireHenchmen_Click(object sender, RoutedEventArgs e)
        {
            //TODO Implement hiring Henchmen for the Guild.
        }

        private void BtnMembers_Click(object sender, RoutedEventArgs e) => GameState.Navigate(new MembersPage(this));

        private void BtnOptions_Click(object sender, RoutedEventArgs e) => GameState.Navigate(new GuildOptionsPage(this));

        private void BtnTransfer_Click(object sender, RoutedEventArgs e)
        {
            //TODO Implement allowing the transfer of henchmen and gold to the Guild by its master.
        }

        #endregion Click

        public GuildManagePage(GuildPage guildPage)
        {
            InitializeComponent();
            RefToGuildPage = guildPage;
            TxtManage.Text = $"You begin to manage {GameState.CurrentGuild.Name}.";
        }
    }
}