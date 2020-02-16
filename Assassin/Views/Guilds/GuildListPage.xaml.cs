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
            if (GameState.YesNoNotification($"Are you sure you want to apply for this guild? It will cost {GameState.CurrentGuild.Fee} gold.", "Assassin"))
            {
                if (GameState.CurrentGuild.Master == "Computer")
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

        private async void LstGuilds_SelectionChanged(object sender, SelectionChangedEventArgs e)
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

        #endregion Click

        #region Page-Manipulation Methods

        public GuildListPage() => InitializeComponent();

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            RefreshItemsSource();
            LstGuilds.SelectedIndex = 0;
        }

        #endregion Page-Manipulation Methods
    }
}