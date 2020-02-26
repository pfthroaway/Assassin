using Assassin.Models;
using Extensions;
using System.Windows;
using System.Windows.Controls;

namespace Assassin.Views.Guilds
{
    /// <summary>Interaction logic for GuildListPage.xaml</summary>
    public partial class GuildListPage
    {
        #region Data-Binding

        /// <summary>Checks whether the Enter and Apply Buttons should be enabled.</summary>
        public async void CheckEnterApply()
        {
            if (LstGuilds.SelectedIndex >= 0)
            {
                GameState.CurrentGuild = GameState.AllGuilds[LstGuilds.SelectedIndex];
                RefreshItemsSource();
                BtnApply.Content = "_Apply";

                if (GameState.CurrentGuild.HasMember(GameState.CurrentUser))
                {
                    BtnEnter.IsEnabled = true;
                    BtnApply.IsEnabled = false;
                }
                else if (await GameState.DatabaseInteraction.HasAppliedToGuild(GameState.CurrentUser, GameState.CurrentGuild))
                {
                    BtnEnter.IsEnabled = false;
                    BtnApply.IsEnabled = false;
                    BtnApply.Content = "_Applied";
                }
                else
                {
                    BtnEnter.IsEnabled = false;
                    BtnApply.IsEnabled = (GameState.CurrentGuild.ID == 1 && GameState.CurrentUser.Level < 5) || GameState.CurrentUser.GoldOnHand >= GameState.CurrentGuild.Fee;
                }
            }
        }

        /// <summary>Refreshed the list of Guilds.</summary>
        private void RefreshItemsSource()
        {
            LstGuilds.ItemsSource = GameState.AllGuilds;
            DataContext = GameState.CurrentGuild;
            LblGoldOnHand.DataContext = GameState.CurrentUser;
        }

        #endregion Data-Binding

        #region Click

        private async void BtnApply_Click(object sender, RoutedEventArgs e)
        {
            //TODO Use old text for guild applications.
            if (GameState.CurrentGuild.ID == 1 && GameState.CurrentUser.Level > 5)
                Functions.AddTextToTextBox(TxtGuild, "This guild is for beginners and novices.\n\nThou art too experienced to be a member.");
            else if (GameState.YesNoNotification($"Are you sure you want to apply for this guild? It will cost {GameState.CurrentGuild.Fee} gold.", "Assassin"))
            {
                if (GameState.CurrentGuild.Master == GameState.CurrentGuild.DefaultMaster)
                {
                    if (await GameState.MemberJoinsGuild(GameState.CurrentUser, GameState.CurrentGuild))
                    {
                        Functions.AddTextToTextBox(TxtGuild, $"You paid {GameState.CurrentGuild.Fee} gold to join the {GameState.CurrentGuild.Name} guild, and have been accepted!");
                        BtnEnter.IsEnabled = true;
                    }
                }
                else
                {
                    await GameState.DatabaseInteraction.ApplyToGuild(GameState.CurrentUser, GameState.CurrentGuild);
                    Functions.AddTextToTextBox(TxtGuild, $"You paid {GameState.CurrentGuild.Fee} gold and applied for the {GameState.CurrentGuild.Name} guild.");
                }

                BtnApply.IsEnabled = false;
                GameState.CurrentUser.GoldOnHand -= GameState.CurrentGuild.Fee;
                GameState.CurrentGuild.Gold += GameState.CurrentGuild.Fee;
                await GameState.DatabaseInteraction.SaveGuild(GameState.CurrentGuild);
                await GameState.DatabaseInteraction.SaveUser(GameState.CurrentUser);
            }
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            Functions.AddTextToTextBox(GameState.GamePage.TxtGame, TxtGuild.Text.Trim());
            GameState.GoBack();
        }

        private void BtnEnter_Click(object sender, RoutedEventArgs e) => GameState.Navigate(new GuildPage(this));

        private void LstGuilds_SelectionChanged(object sender, SelectionChangedEventArgs e) => CheckEnterApply();

        #endregion Click

        #region Page-Manipulation Methods

        public GuildListPage() => InitializeComponent();

        private void Page_Loaded(object sender, RoutedEventArgs e) => RefreshItemsSource();

        #endregion Page-Manipulation Methods
    }
}