using Assassin.Classes;
using Extensions;
using System.Windows;

namespace Assassin.Pages.Battle
{
    /// <summary>Interaction logic for AssassinationPage.xaml</summary>
    public partial class AssassinationPage
    {
        internal City.GamePage RefToGamePage { get; set; }

        /// <summary>Checks the User's Hunger and Thirst to determine whether or not they can continue.</summary>
        /// <returns>Returns true if player isn't too hungry or thirst to continue.</returns>
        private bool CheckHungerThirst()
        {
            if (GameState.CurrentUser.Hunger >= 24 || GameState.CurrentUser.Thirst >= 24)
            {
                BtnNewVictim.IsEnabled = false;
                BtnAssassinate.IsEnabled = false;

                if (GameState.CurrentUser.Hunger >= 24 && GameState.CurrentUser.Thirst >= 24)
                    Functions.AddTextToTextBox(TxtAssassinate, "You are too hungry and thirsty to continue.");
                else if (GameState.CurrentUser.Hunger >= 24)
                    Functions.AddTextToTextBox(TxtAssassinate, "You are too hungry to continue.");
                else if (GameState.CurrentUser.Thirst >= 24)
                    Functions.AddTextToTextBox(TxtAssassinate, "You are too thirsty to continue.");
                return false;
            }
            BtnNewVictim.IsEnabled = true;
            BtnAssassinate.IsEnabled = true;

            if (GameState.CurrentUser.Hunger > 0 && GameState.CurrentUser.Hunger % 5 == 0)
                Functions.AddTextToTextBox(TxtAssassinate, "You are " + GameState.CurrentUser.HungerToString.ToLower() + ".");
            if (GameState.CurrentUser.Thirst > 0 && GameState.CurrentUser.Thirst % 5 == 0)
                Functions.AddTextToTextBox(TxtAssassinate, "You are " + GameState.CurrentUser.ThirstToString.ToLower() + ".");
            return true;
        }

        /// <summary>Gets an Enemy if Player is able to continue.</summary>
        internal void GetEnemy()
        {
            if (CheckHungerThirst())
            {
                GameState.CurrentUser.Hunger++;
                GameState.CurrentUser.Thirst++;

                GameState.CurrentEnemy = GameState.SelectEnemy();

                Functions.AddTextToTextBox(TxtAssassinate, "You spot a " + GameState.CurrentEnemy + ".");
            }
        }

        #region Button-Click Methods

        private void BtnAssassinate_Click(object sender, RoutedEventArgs e) => GameState.Navigate(new BattlePage());

        private void BtnNewVictim_Click(object sender, RoutedEventArgs e) => GetEnemy();

        private void BtnBack_Click(object sender, RoutedEventArgs e) => ClosePage();

        #endregion Button-Click Methods

        #region Page-Manipulation Methods

        /// <summary>Closes the Page.</summary>
        private void ClosePage() => GameState.GoBack();

        public AssassinationPage()
        {
            InitializeComponent();
            TxtAssassinate.Text = "You go out in search of prey...";
            GetEnemy();
        }

        #endregion Page-Manipulation Methods
    }
}