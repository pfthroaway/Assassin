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

        #region Click

        private void BtnApplications_Click(object sender, RoutedEventArgs e) => GameState.Navigate(new ManageApplicationsPage(this));

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            Functions.AddTextToTextBox(RefToGuildPage.TxtGuild, TxtManage.Text);
            GameState.GoBack();
        }

        private void BtnHireHenchmen_Click(object sender, RoutedEventArgs e)
        {
            //TODO Implement hiring Henchmen for the Guild.
        }

        private void BtnMembers_Click(object sender, RoutedEventArgs e) => GameState.Navigate(new MembersPage(this));

        private void BtnOptions_Click(object sender, RoutedEventArgs e)
        {
            //TODO Implement allowing the Guild name, fee, and master to be changed.
        }

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