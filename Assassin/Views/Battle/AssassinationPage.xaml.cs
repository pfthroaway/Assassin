using Assassin.Models;
using Extensions;
using System.Windows;

namespace Assassin.Views.Battle
{
    /// <summary>Interaction logic for AssassinationPage.xaml</summary>
    public partial class AssassinationPage
    {
        /// <summary>Checks the User's Hunger and Thirst to determine whether or not they can continue.</summary>
        /// <returns>Returns true if player isn't too hungry or thirst to continue.</returns>
        private bool CheckHungerThirst()
        {
            Functions.AddTextToTextBox(TxtAssassinate, GameState.CurrentUser.DisplayHungerThirstText());
            if (GameState.CurrentUser.CanDoAction())
                return true;
            DisableButtons();
            return false;
        }

        /// <summary>Disables the Assassin and New Victim Buttons.</summary>
        private void DisableButtons()
        {
            BtnAssassinate.IsEnabled = false;
            BtnNewVictim.IsEnabled = false;
        }

        /// <summary>Gets an Enemy if Player is able to continue.</summary>
        internal void GetEnemy()
        {
            if (CheckHungerThirst())
            {
                GameState.SelectEnemy();
                GameState.CurrentUser.GainHungerThirst();
                Functions.AddTextToTextBox(TxtAssassinate, $"You spot a {GameState.CurrentEnemy}.");
                BtnAssassinate.IsEnabled = true;
            }
        }

        #region Button-Click Methods

        private void BtnAssassinate_Click(object sender, RoutedEventArgs e)
        {
            BtnAssassinate.IsEnabled = false;
            GameState.Navigate(new BattlePage { RefToAssassinationPage = this });
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            Functions.AddTextToTextBox(GameState.GamePage.TxtGame, TxtAssassinate.Text.Trim());
            GameState.GoBack();
        }

        private void BtnNewVictim_Click(object sender, RoutedEventArgs e) => GetEnemy();

        #endregion Button-Click Methods

        #region Page-Manipulation Methods

        public AssassinationPage()
        {
            InitializeComponent();
            TxtAssassinate.Text = "Thou dost go out in search of prey...";
            GetEnemy();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (GameState.CurrentUser.Alive)
                CheckHungerThirst();
            else
                DisableButtons();
        }

        #endregion Page-Manipulation Methods
    }
}