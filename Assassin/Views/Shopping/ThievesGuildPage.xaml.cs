using Assassin.Models;
using Extensions;
using System.Windows;

namespace Assassin.Views.Shopping
{
    /// <summary>Interaction logic for ThievesGuildPage.xaml</summary>
    public partial class ThievesGuildPage
    {
        /// <summary>Checks whether the Purchase button should be enabled.</summary>
        private void CheckPurchase() => BtnPurchase.IsEnabled = GameState.CurrentUser.GoldOnHand >= 300 && GameState.CurrentUser.Lockpicks < 99;

        #region Click Methods

        private async void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            GameState.GoBack();
            await GameState.DatabaseInteraction.SaveUser(GameState.CurrentUser);
        }

        private void BtnPurchase_Click(object sender, RoutedEventArgs e)
        {
            GameState.CurrentUser.Lockpicks++;
            GameState.CurrentUser.GoldOnHand -= 300;
            Functions.AddTextToTextBox(TxtThieves, "Thou hast purchased a lockpick for 300 gold. Use it wisely.");
        }

        #endregion Click Methods

        #region Page-Manipulation Methods

        public ThievesGuildPage()
        {
            InitializeComponent();
            TxtThieves.Text = "As you enter, a hooded figure emerges from the shadow.\n\n" +
    "\"Sorry to disappoint you, but after a raid by the King's Men, all I have left is a few lockpicks. I can sell them to you for 300 gold each. Are you interested?\"";
            CheckPurchase();
            DataContext = GameState.CurrentUser;
        }

        #endregion Page-Manipulation Methods
    }
}