using System;
using System.ComponentModel;
using System.Windows;

namespace Assassin
{
    /// <summary>
    /// Interaction logic for AssassinationWindow.xaml
    /// </summary>
    public partial class AssassinationWindow : Window
    {
        internal GameWindow RefToGameWindow { get; set; }
        private string nl = Environment.NewLine;

        /// <summary>
        /// Adds text to the txtAssassination TextBox.
        /// </summary>
        /// <param name="newText">Text to be added</param>
        internal void AddTextTT(string newText)
        {
            txtAssassinate.Text += nl + nl + newText;
            txtAssassinate.Focus();
            txtAssassinate.CaretIndex = txtAssassinate.Text.Length;
            txtAssassinate.ScrollToEnd();
        }

        /// <summary>
        /// Checks the User's Hunger and Thirst to determine whether or not they can continue.
        /// </summary>
        /// <returns>Returns true if player isn't too hungry or thirst to continue.</returns>
        private bool CheckHungerThirst()
        {
            if (GameState.CurrentUser.Hunger >= 24 || GameState.CurrentUser.Thirst >= 24)
            {
                btnNewVictim.IsEnabled = false;
                btnAssassinate.IsEnabled = false;

                if (GameState.CurrentUser.Hunger >= 24 && GameState.CurrentUser.Thirst >= 24)
                    AddTextTT("You are too hungry and thirsty to continue.");
                else if (GameState.CurrentUser.Hunger >= 24)
                    AddTextTT("You are too hungry to continue.");
                else if (GameState.CurrentUser.Thirst >= 24)
                    AddTextTT("You are too thirsty to continue.");
                return false;
            }
            else
            {
                btnNewVictim.IsEnabled = true;
                btnAssassinate.IsEnabled = true;

                if (GameState.CurrentUser.Hunger > 0 && GameState.CurrentUser.Hunger % 5 == 0)
                    AddTextTT("You are " + GameState.CurrentUser.HungerToString.ToLower() + ".");
                if (GameState.CurrentUser.Thirst > 0 && GameState.CurrentUser.Thirst % 5 == 0)
                    AddTextTT("You are " + GameState.CurrentUser.ThirstToString.ToLower() + ".");
                return true;
            }
        }

        /// <summary>
        /// Gets an Enemy if Player is able to continue.
        /// </summary>
        internal void GetEnemy()
        {
            if (CheckHungerThirst())
            {
                GameState.CurrentUser.Hunger += 1;
                GameState.CurrentUser.Thirst += 1;

                GameState.CurrentEnemy = GameState.SelectEnemy();

                AddTextTT("You spot a " + GameState.CurrentEnemy + ".");
            }
        }

        #region Button-Click Methods

        private void btnAssassinate_Click(object sender, RoutedEventArgs e)
        {
            btnAssassinate.IsEnabled = false;
            BattleWindow battleWindow = new BattleWindow();
            battleWindow.RefToAssassinationWindow = this;
            battleWindow.Show();
            this.Visibility = Visibility.Hidden;
        }

        private void btnNewVictim_Click(object sender, RoutedEventArgs e)
        {
            GetEnemy();
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            CloseWindow();
        }

        #endregion Button-Click Methods

        #region Window-Manipulation Methods

        private void CloseWindow()
        {
            this.Close();
        }

        public AssassinationWindow()
        {
            InitializeComponent();
            txtAssassinate.Text = "You go out in search of prey...";
        }

        private void windowAssassination_Closing(object sender, CancelEventArgs e)
        {
            RefToGameWindow.Show();
            RefToGameWindow.AddTextTT(txtAssassinate.Text);
        }

        #endregion Window-Manipulation Methods
    }
}