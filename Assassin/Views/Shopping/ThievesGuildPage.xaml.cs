using Assassin.Models;
using Extensions;
using System.ComponentModel;

namespace Assassin.Views.Shopping
{
    /// <summary>Interaction logic for ThievesGuildPage.xaml</summary>
    public partial class ThievesGuildPage : INotifyPropertyChanged
    {
        #region Data-Binding

        /// <summary>Event that executes if a Property value has changed so that the UI can properly be updated.</summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>Invokes <see cref="PropertyChangedEventHandler"/> to update the UI when a Property value changes.</summary>
        /// <param name="property">Name of Property whose value has changed</param>
        private void OnPropertyChanged(string property) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));

        #endregion Data-Binding

        /// <summary>Checks whether the Purchase button should be enabled.</summary>
        private void CheckPurchase() => BtnPurchase.IsEnabled = GameState.CurrentUser.GoldOnHand >= 300;

        #region Click Methods

        private async void BtnBack_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            GameState.GoBack();
            await GameState.SaveUser(GameState.CurrentUser);
        }

        private void BtnPurchase_Click(object sender, System.Windows.RoutedEventArgs e)
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
    "Sorry to disappoint you, but after a raid by the King's Men, all I have left is a few lockpicks. I can sell them to you for 300 gold each. Are you interested?";
            CheckPurchase();
            DataContext = GameState.CurrentUser;
        }

        #endregion Page-Manipulation Methods
    }
}