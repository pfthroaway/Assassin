using Assassin.Models;
using Assassin.Models.Entities;
using Extensions;
using Extensions.DataTypeHelpers;
using Extensions.Enums;
using System.Windows;
using System.Windows.Input;

namespace Assassin.Views.Guilds
{
    /// <summary>Interaction logic for GuildOptionsPage.xaml</summary>
    public partial class GuildOptionsPage
    {
        private Guild _copyOfGuild = new Guild();
        private GuildManagePage RefToGuildManagePage { get; set; }

        /// <summary>Disable controls if the <see cref="User"/> gives up control of the <see cref="Guild"/>.</summary>
        private void DisableControls()
        {
            TxtEntranceFee.IsEnabled = false;
            TxtGuildName.IsEnabled = false;
            CmbMaster.IsEnabled = false;
            BtnSave.IsEnabled = false;
            RefToGuildManagePage.DisableControls();
        }

        #region Click

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            Functions.AddTextToTextBox(RefToGuildManagePage.TxtManage, TxtOptions.Text.Trim());
            GameState.GoBack();
        }

        private async void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            _copyOfGuild.Name = TxtGuildName.Text.Trim();
            _copyOfGuild.Fee = Int32Helper.Parse(TxtEntranceFee.Text.Trim());
            _copyOfGuild.Master = CmbMaster.Text;
            if (GameState.CurrentGuild != _copyOfGuild)
            {
                if (_copyOfGuild.Name != GameState.CurrentGuild.Name)
                {
                    GameState.CurrentGuild.Name = _copyOfGuild.Name;
                    Functions.AddTextToTextBox(TxtOptions, $"You change the name of the guild to {GameState.CurrentGuild.Name}.");
                }
                if (_copyOfGuild.Fee != GameState.CurrentGuild.Fee)
                {
                    GameState.CurrentGuild.Fee = _copyOfGuild.Fee;
                    Functions.AddTextToTextBox(TxtOptions, $"You change the entrance fee of the guild to {GameState.CurrentGuild.FeeToString}.");
                }
                if (_copyOfGuild.Master != GameState.CurrentGuild.Master)
                {
                    GameState.CurrentGuild.Master = _copyOfGuild.Master;
                    Functions.AddTextToTextBox(TxtOptions, $"You resign as the guildmaster of {GameState.CurrentGuild.Name}, and hand leadership over to {GameState.CurrentGuild.Master}.");
                    DisableControls();
                }

                await GameState.DatabaseInteraction.SaveGuild(GameState.CurrentGuild);
            }
        }

        #endregion Click

        #region Page-Manipulation Methods

        public GuildOptionsPage(GuildManagePage guildManagePage)
        {
            InitializeComponent();
            _copyOfGuild = new Guild(GameState.CurrentGuild);
            RefToGuildManagePage = guildManagePage;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            TxtGuildName.Text = GameState.CurrentGuild.Name;
            TxtEntranceFee.Text = GameState.CurrentGuild.Fee.ToString();
            foreach (string member in GameState.CurrentGuild.Members)
                CmbMaster.Items.Add(member);
            CmbMaster.Items.Add("Computer");
            CmbMaster.Text = GameState.CurrentGuild.Master;
        }

        private void TxtEntranceFee_PreviewKeyDown(object sender, KeyEventArgs e) => Functions.PreviewKeyDown(e, KeyType.Integers);

        #endregion Page-Manipulation Methods
    }
}