using Assassin.Models;
using Extensions;
using System.Windows.Controls;

namespace Assassin.Views.Shopping
{
    /// <summary>Interaction logic for GeneralStorePage.xaml</summary>
    public partial class GeneralStorePage : Page
    {
        /// <summary>Checks whether the purchase Buttons should be enabled.</summary>
        private void CheckButtons()
        {
            BtnShovel.IsEnabled = !GameState.CurrentUser.Shovel && GameState.CurrentUser.GoldOnHand >= 500;
            BtnLantern.IsEnabled = !GameState.CurrentUser.Lantern && GameState.CurrentUser.GoldOnHand >= 350;
        }

        #region Click

        private async void BtnBack_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Functions.AddTextToTextBox(GameState.ShopsPage.TxtShops, TxtGeneralStore.Text.Trim());
            Functions.AddTextToTextBox(GameState.ShopsPage.TxtShops, "Thou overhears Grandpa Joe snoring as thou leaves.");
            GameState.GoBack();
            await GameState.DatabaseInteraction.SaveUser(GameState.CurrentUser);
        }

        private void BtnLantern_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            GameState.CurrentUser.GoldOnHand -= 350;
            GameState.CurrentUser.Lantern = true;
            Functions.AddTextToTextBox(TxtGeneralStore, "\"Here thou goes.  May a little more light enter thy life.\"\n\nYou purchase a lantern for 350 gold.");
            CheckButtons();
        }

        private void BtnShovel_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            GameState.CurrentUser.GoldOnHand -= 500;
            GameState.CurrentUser.Shovel = true;
            Functions.AddTextToTextBox(TxtGeneralStore, "\"Here thou goes. Enjoy thy digging.\"\n\nYou purchase a shovel for 500 gold.");
            CheckButtons();
        }

        #endregion Click

        public GeneralStorePage()
        {
            InitializeComponent();
            Functions.AddTextToTextBox(TxtGeneralStore, "You enter The General Store. Grandpa Joe sits in his rocking chair.\n\n" +
                "\"Looking for supplies, eh? Wells, we just been cleaned out, except for some shovels and a few lanterns. I'll sell the shovels for 500 gold each, and the latnerns for 350 gold each.\"");
            CheckButtons();
            DataContext = GameState.CurrentUser;
        }
    }
}