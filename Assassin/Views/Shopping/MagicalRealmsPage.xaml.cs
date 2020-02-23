using Assassin.Models;
using Extensions;
using System.Windows;
using System.Windows.Controls;

namespace Assassin.Views.Shopping
{
    /// <summary>Interaction logic for MagicalRealmsPage.xaml</summary>
    public partial class MagicalRealmsPage : Page
    {
        #region Click

        private async void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            Functions.AddTextToTextBox(GameState.ShopsPage.TxtShops, TxtMagicalRealms.Text.Trim());
            GameState.GoBack();
            await GameState.DatabaseInteraction.SaveUser(GameState.CurrentUser);
        }

        private void BtnPurchase_Click(object sender, RoutedEventArgs e)
        {
            GameState.CurrentUser.Amulet = true;
            GameState.CurrentUser.GoldOnHand -= 1000;
            Functions.AddTextToTextBox(TxtMagicalRealms, "\"Here thou goes. May it protect thee from that which bumps in the night.\"\n\nThou hast purchased an amulet for 1,000 gold.");
            BtnPurchase.IsEnabled = false;
        }

        #endregion Click

        public MagicalRealmsPage()
        {
            InitializeComponent();
            Functions.AddTextToTextBox(TxtMagicalRealms, "An elderly Mage sits behind the counter.\n\n" +
                 "\"Greetings. I am sorry to inform thee that all I can sell thee is a magic amulet to ward off ghosts and The Shades. Anything else is beyond my powers, as I am recovering from a back-fired Mind Blast spell.\nI shall sell thee an amulet for 1,000 gold.\"");
            BtnPurchase.IsEnabled = !GameState.CurrentUser.Amulet && GameState.CurrentUser.GoldOnHand >= 1000;
            DataContext = GameState.CurrentUser;
        }
    }
}