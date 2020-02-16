using Assassin.Models;
using Assassin.Models.Enums;
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

        /// <summary>Closes the Page.</summary>
        private async void ClosePage()
        {
            Functions.AddTextToTextBox(RefToGuildListPage.TxtGuild, TxtGuild.Text);
            GameState.GoBack();
            await GameState.DatabaseInteraction.SaveUser(GameState.CurrentUser);
        }

        #region Click

        private void BtnBack_Click(object sender, RoutedEventArgs e) => ClosePage();

        private void BtnBar_Click(object sender, RoutedEventArgs e) => GameState.Navigate(new BarPage());

        private void BtnChallenge_Click(object sender, RoutedEventArgs e)
        {
            //TODO Implement challenging a guildmaster.
        }

        private void BtnDonate_Click(object sender, RoutedEventArgs e) => GameState.Navigate(new GuildDonatePage(this));

        private void BtnJobs_Click(object sender, RoutedEventArgs e) => GameState.Navigate(new JobsPage(this));

        private void BtnManageGuild_Click(object sender, RoutedEventArgs e) => GameState.Navigate(new GuildManagePage(this));

        private void BtnHireHenchmen_Click(object sender, RoutedEventArgs e)
        {
            //TODO Implement hiring Henchmen.
        }

        private void BtnMembers_Click(object sender, RoutedEventArgs e) => GameState.Navigate(new MembersPage(this));

        private void BtnPlanRaid_Click(object sender, RoutedEventArgs e)
        {
            //TODO Implement raiding.
        }

        private async void BtnQuitGuild_Click(object sender, RoutedEventArgs e)
        {
            if (GameState.YesNoNotification($"Are you sure you want to quit {GameState.CurrentGuild}? You will have to reapply if you want to join again.", "Assassin") && await GameState.MemberLeavesGuild(GameState.CurrentUser, GameState.CurrentGuild))
            {
                Functions.AddTextToTextBox(TxtGuild, $"You quit {GameState.CurrentGuild.Name}");
                RefToGuildListPage.CheckEnterApply();
                ClosePage();
            }
        }

        private void BtnSleep_Click(object sender, RoutedEventArgs e)
        {
            Functions.AddTextToTextBox(TxtGuild, "You find an empty room and sleep.");
            GameState.CurrentUser.CurrentLocation = SleepLocation.Guild;
            GameState.GamePage.ToggleButtons(false);
            RefToGuildListPage.BtnEnter.IsEnabled = false;
            RefToGuildListPage.LstGuilds.IsEnabled = false;
            ClosePage();
        }

        private void BtnTransferItems_Click(object sender, RoutedEventArgs e)
        {
            //TODO Implement transferring items to the Guild and other members.
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