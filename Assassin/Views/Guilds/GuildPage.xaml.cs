using Assassin.Models;
using Assassin.Views.City;
using Assassin.Views.Shopping;
using Extensions;
using System.Windows;

namespace Assassin.Views.Guilds
{
    /// <summary>Interaction logic for GuildPage.xaml</summary>
    public partial class GuildPage
    {
        public GuildListPage RefToGuildListPage { get; set; }

        #region Click

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            Functions.AddTextToTextBox(RefToGuildListPage.TxtGuild, TxtGuild.Text);
            GameState.GoBack();
        }

        private void BtnBar_Click(object sender, RoutedEventArgs e) => GameState.Navigate(new BarPage());

        private void BtnChallenge_Click(object sender, RoutedEventArgs e)
        {
            //TODO Implement challenging a guildmaster.
        }

        private void BtnDonate_Click(object sender, RoutedEventArgs e)
        {
            //TODO Implement donating to Guilds.
        }

        private void BtnJobs_Click(object sender, RoutedEventArgs e) => GameState.Navigate(new JobsPage(this));

        private void BtnManageGuild_Click(object sender, RoutedEventArgs e)
        {
            //TODO Implement managing a Guild.
        }

        private void BtnHireHenchmen_Click(object sender, RoutedEventArgs e)
        {
            //TODO Implement hiring Henchmen.
        }

        private void BtnMembers_Click(object sender, RoutedEventArgs e) => GameState.Navigate(new MembersPage(this));

        private void BtnPlanRaid_Click(object sender, RoutedEventArgs e)
        {
        }

        private void BtnQuitGuild_Click(object sender, RoutedEventArgs e)
        {
        }

        private void BtnSleep_Click(object sender, RoutedEventArgs e)
        {
        }

        private void BtnTransferItems_Click(object sender, RoutedEventArgs e)
        {
        }

        #endregion Click

        #region Page-Manipulation Methods

        public GuildPage(GuildListPage guildListPage)
        {
            InitializeComponent();
            RefToGuildListPage = guildListPage;
            TxtGuild.Text = $"You enter {GameState.CurrentGuild.Name}.";
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            BtnChallenge.IsEnabled = GameState.CurrentGuild.Master != GameState.CurrentUser.Name;
            BtnManageGuild.IsEnabled = GameState.CurrentGuild.Master == GameState.CurrentUser.Name;
            Title = GameState.CurrentGuild.Name;
        }

        #endregion Page-Manipulation Methods
    }
}