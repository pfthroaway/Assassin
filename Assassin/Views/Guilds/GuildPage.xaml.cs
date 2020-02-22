using Assassin.Models;
using Assassin.Models.Entities;
using Assassin.Models.Enums;
using Assassin.Models.Items;
using Assassin.Views.Battle;
using Assassin.Views.City;
using Assassin.Views.Shopping;
using Extensions;
using System.Collections.Generic;
using System.Linq;
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
            Functions.AddTextToTextBox(RefToGuildListPage.TxtGuild, TxtGuild.Text.Trim());
            GameState.GoBack();
            await GameState.DatabaseInteraction.SaveUser(GameState.CurrentUser);
        }

        /// <summary>Disables all the Buttons on the Page.</summary>
        public void DisableButtons()
        {
            BtnBar.IsEnabled = false;
            BtnChallenge.IsEnabled = false;
            BtnDonate.IsEnabled = false;
            BtnHireHenchmen.IsEnabled = false;
            BtnJobs.IsEnabled = false;
            BtnManageGuild.IsEnabled = false;
            BtnMembers.IsEnabled = false;
            BtnPlanRaid.IsEnabled = false;
            BtnQuitGuild.IsEnabled = false;
            BtnSleep.IsEnabled = false;
            BtnTransferItems.IsEnabled = false;
        }

        #region Click

        private void BtnBack_Click(object sender, RoutedEventArgs e) => ClosePage();

        private void BtnBar_Click(object sender, RoutedEventArgs e) => GameState.Navigate(new BarPage());

        private void BtnChallenge_Click(object sender, RoutedEventArgs e)
        {
            if (GameState.YesNoNotification("Are you sure you want to challenge the guildmaster? If you fail, you will be expelled from the guild.", "Assassin"))
            {
                if (GameState.CurrentGuild.Master == GameState.CurrentGuild.DefaultMaster)
                {
                    List<Weapon> weapons = GameState.AllWeapons.Where(wpn => wpn.Value >= GameState.CurrentGuild.ID * 100 && wpn.Value <= GameState.CurrentGuild.ID * 500).ToList();
                    List<Armor> armor = GameState.AllArmor.Where(armr => armr.Value >= GameState.CurrentGuild.ID * 100 && armr.Value <= GameState.CurrentGuild.ID * 500).ToList();

                    GameState.CurrentEnemy = new Enemy(GameState.CurrentGuild.Master, GameState.CurrentGuild.ID * 2, GameState.CurrentGuild.ID * 100, GameState.CurrentGuild.ID * 100, weapons[Functions.GenerateRandomNumber(0, weapons.Count - 1)], armor[Functions.GenerateRandomNumber(0, armor.Count - 1)], Functions.GenerateRandomNumber(1, GameState.CurrentGuild.Gold), GameState.CurrentGuild.ID * 15, GameState.CurrentGuild.ID * 15, GameState.CurrentGuild.ID * 15);
                    GameState.Navigate(new BattlePage(false, true) { RefToGuildPage = this });
                }
                else
                {
                    GameState.CurrentEnemy = new Enemy(GameState.AllUsers.Find(user => user.Name == GameState.CurrentGuild.Master));
                    GameState.Navigate(new BattlePage(true, true) { RefToGuildPage = this });
                }
            }
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
            BtnChallenge.IsEnabled = GameState.CurrentGuild.ID != 1 && GameState.CurrentGuild.Master != GameState.CurrentUser.Name && GameState.CurrentGuild.HasMember(GameState.CurrentUser);
            BtnManageGuild.IsEnabled = GameState.CurrentGuild.Master == GameState.CurrentUser.Name;
            Title = GameState.CurrentGuild.Name;
        }

        #endregion Page-Manipulation Methods
    }
}