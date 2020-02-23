using Assassin.Models;
using Extensions;
using Extensions.DataTypeHelpers;
using Extensions.Enums;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Assassin.Views.Guilds
{
    /// <summary>Interaction logic for GuildDonatePage.xaml</summary>
    public partial class GuildDonatePage
    {
        private GuildPage RefToGuildPage { get; set; }

        #region Input Manipulation

        /// <summary>Clears all values from all input TextBoxes.</summary>
        private void Clear()
        {
            TxtLevel1.Clear();
            TxtLevel2.Clear();
            TxtLevel3.Clear();
            TxtLevel4.Clear();
            TxtLevel5.Clear();
            TxtGold.Clear();
        }

        private void Txt_PreviewKeyDown(object sender, KeyEventArgs e) => Functions.PreviewKeyDown(e, KeyType.Integers);

        private void Txt_TextChanged(object sender, TextChangedEventArgs e)
        {
            bool enabled = TxtLevel1.Text.Trim().Length > 0 || TxtLevel2.Text.Trim().Length > 0 || TxtLevel3.Text.Trim().Length > 0 || TxtLevel4.Text.Trim().Length > 0 || TxtLevel5.Text.Trim().Length > 0 || TxtGold.Text.Trim().Length > 0;
            BtnClear.IsEnabled = enabled;
            BtnDonate.IsEnabled = enabled;
        }

        #endregion Input Manipulation

        #region Click

        private async void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            Functions.AddTextToTextBox(RefToGuildPage.TxtGuild, TxtDonate.Text.Trim());
            GameState.GoBack();
            await GameState.DatabaseInteraction.SaveGuild(GameState.CurrentGuild);
            await GameState.DatabaseInteraction.SaveUser(GameState.CurrentUser);
        }

        private void BtnClear_Click(object sender, RoutedEventArgs e) => Clear();

        private void BtnDonate_Click(object sender, RoutedEventArgs e)
        {
            int level1 = Int32Helper.Parse(TxtLevel1.Text.Trim());
            int level2 = Int32Helper.Parse(TxtLevel2.Text.Trim());
            int level3 = Int32Helper.Parse(TxtLevel3.Text.Trim());
            int level4 = Int32Helper.Parse(TxtLevel4.Text.Trim());
            int level5 = Int32Helper.Parse(TxtLevel5.Text.Trim());
            int gold = Int32Helper.Parse(TxtGold.Text.Trim());
            string errorText = "";
            string donateText = "";
            if (level1 > 0 && GameState.CurrentUser.Henchmen.Level1 >= level1)
            {
                GameState.CurrentGuild.Henchmen.Level1 += level1;
                GameState.CurrentUser.Henchmen.Level1 -= level1;
                TxtLevel1.Clear();
                donateText = string.Join("\n", donateText, $"You donate {level1} Level 1 Henchmen to {GameState.CurrentGuild.Name}.");
            }
            else if (GameState.CurrentUser.Henchmen.Level1 < level1)
                errorText = string.Join("\n", errorText, "You do not have that many Level 1 Henchmen to donate.");
            if (level1 > 0 && GameState.CurrentUser.Henchmen.Level1 >= level1)
            {
                GameState.CurrentGuild.Henchmen.Level1 += level1;
                GameState.CurrentUser.Henchmen.Level1 -= level1;
                TxtLevel1.Clear();
                donateText = string.Join("\n", donateText, $"You donate {level2} Level 2 Henchmen to {GameState.CurrentGuild.Name}.");
            }
            else if (GameState.CurrentUser.Henchmen.Level2 < level2)
                errorText = string.Join("\n", errorText, "You do not have that many Level 2 Henchmen to donate.");
            if (level3 > 0 && GameState.CurrentUser.Henchmen.Level3 >= level3)
            {
                GameState.CurrentGuild.Henchmen.Level3 += level3;
                GameState.CurrentUser.Henchmen.Level3 -= level3;
                TxtLevel3.Clear();
                donateText = string.Join("\n", donateText, $"You donate {level3} Level 3 Henchmen to {GameState.CurrentGuild.Name}.");
            }
            else if (GameState.CurrentUser.Henchmen.Level3 < level3)
                errorText = string.Join("\n", errorText, "You do not have that many Level 3 Henchmen to donate.");
            if (level4 > 0 && GameState.CurrentUser.Henchmen.Level4 >= level4)
            {
                GameState.CurrentGuild.Henchmen.Level4 += level4;
                GameState.CurrentUser.Henchmen.Level4 -= level4;
                TxtLevel4.Clear();
                donateText = string.Join("\n", donateText, $"You donate {level4} Level 4 Henchmen to {GameState.CurrentGuild.Name}.");
            }
            else if (GameState.CurrentUser.Henchmen.Level4 < level4)
                errorText = string.Join("\n", errorText, "You do not have that many Level 4 Henchmen to donate.");
            if (level5 > 0 && GameState.CurrentUser.Henchmen.Level5 >= level5)
            {
                GameState.CurrentGuild.Henchmen.Level5 += level5;
                GameState.CurrentUser.Henchmen.Level5 -= level5;
                TxtLevel5.Clear();
                donateText = string.Join("\n", donateText, $"You donate {level5} Level 5 Henchmen to {GameState.CurrentGuild.Name}.");
            }
            else if (GameState.CurrentUser.Henchmen.Level5 < level5)
                errorText = string.Join("\n", errorText, "You do not have that many Level 5 Henchmen to donate.");
            if (gold > 0 && GameState.CurrentUser.GoldOnHand >= gold)
            {
                GameState.CurrentGuild.Gold += gold;
                GameState.CurrentUser.GoldOnHand -= gold;
                TxtGold.Clear();
                donateText = string.Join("\n", donateText, $"You donate {gold} gold to {GameState.CurrentGuild.Name}.");
            }
            else if (GameState.CurrentUser.GoldOnHand < gold)
                errorText = string.Join("\n", errorText, "You do not have that many gold to donate.");
            errorText = errorText.Trim();
            donateText = donateText.Trim();
            if (errorText.Length > 0)
                GameState.DisplayNotification(errorText, "Assassin");
            if (donateText.Length > 0)
                Functions.AddTextToTextBox(TxtDonate, donateText);
        }

        #endregion Click

        #region Page-Manipulation Methods

        public GuildDonatePage(GuildPage guildPage)
        {
            InitializeComponent();
            RefToGuildPage = guildPage;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Title = $"Assassin - Donate to {GameState.CurrentGuild.Name}";
            LblDonate.Text = $"Donate to {GameState.CurrentGuild.Name}";
            DataContext = GameState.CurrentUser.Henchmen;
            LblGold.DataContext = GameState.CurrentUser;
        }

        #endregion Page-Manipulation Methods
    }
}