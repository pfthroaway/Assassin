using Assassin.Models;
using Extensions;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Assassin.Views.Shopping
{
    /// <summary>Interaction logic for ThievesGuildPage.xaml</summary>
    public partial class ThievesGuildPage : INotifyPropertyChanged
    {
        #region Data-Binding

        /// <summary>Event that executes if a Property value has changed so that the UI can properly be updated.</summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>Notifys the PropertyChanged event alerting the WPF Framework to update the UI.</summary>
        /// <param name="propertyNames">The names of the properties to update in the UI.</param>
        protected void NotifyPropertyChanged(params string[] propertyNames)
        {
            if (PropertyChanged != null)
            {
                foreach (string propertyName in propertyNames)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                }
            }
        }

        /// <summary>Notifys the PropertyChanged event alerting the WPF Framework to update the UI.</summary>
        /// <param name="propertyName">The optional name of the property to update in the UI. If this is left blank, the name will be taken from the calling member via the CallerMemberName attribute.</param>
        protected virtual void NotifyPropertyChanged([CallerMemberName]string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

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