using Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Threading;

namespace Assassin
{
    /// <summary>Interaction logic for CourtWindow.xaml</summary>
    public partial class CourtWindow
    {
        internal GameWindow RefToGameWindow { get; set; }
        private readonly string nl = Environment.NewLine;
        private readonly List<string> courtText = new List<string>();
        private int index, fine;
        private readonly DispatcherTimer timer = new DispatcherTimer();
        private bool blnGuilty, blnFinished;
        private string reason = "";

        /// <summary>
        /// Adds text to the txtCourt Textbox.
        /// </summary>
        /// <param name="newText">Text to be added</param>
        internal void AddTextTT(string newText)
        {
            txtCourt.Text += nl + nl + newText;
            txtCourt.Focus();
            txtCourt.CaretIndex = txtCourt.Text.Length;
            txtCourt.ScrollToEnd();
        }

        /// <summary>
        /// Loads the Window and assigns the text to be displayed.
        /// </summary>
        /// <param name="arrestReason">Reason User was arrested</param>
        internal void LoadWindow(string arrestReason)
        {
            reason = arrestReason;
            courtText.Add("You are dragged to the courts of justice.");
            courtText.Add("The judge stares at you. . .");
            if (GameState.CurrentUser.Level > 6)
            {
                courtText.Add("\"Well, if it isn't the " + GameState.CurrentUser.Rank + " , " + GameState.CurrentUser.Name + ".\"");
                courtText.Add("\"Don't worry, I will be impartial,\" he laughs.");
            }
            courtText.Add("The trial begins. . .");
            switch (reason)
            {
                case "Robbery":
                    courtText.Add("You are charged with the crime of attempted theft of property.");
                    fine = 50;
                    break;

                case "Assault":
                    courtText.Add("You are charged with the crime of attempted assault and robbery.");
                    fine = 100;
                    break;

                case "Assassinate":
                    courtText.Add("You are charged with the crime of attemped murder.");
                    fine = 250;
                    break;
            }

            courtText.Add("Prosecution. . .");
            courtText.Add("Defense. . .");
            int guilty = GameState.GenerateRandomNumber(1, 100);
            courtText.Add("The judge finds you. . .");
            if (guilty <= (100 - (GameState.CurrentUser.Level * 5)))
                courtText.Add("innocent!");
            else
            {
                courtText.Add("guilty!");
                courtText.Add("\"You are to pay " + fine + " gold as a fine. Pay it or you will be jailed for the night,\" the judge says.");
                blnGuilty = true;
            }

            timer.Start();
        }

        #region Button-Click Methods

        private void btnPayFine_Click(object sender, RoutedEventArgs e)
        {
            AddTextTT("You decide it is best to pay the fine and depart.");
            GameState.CurrentUser.GoldOnHand -= fine;
            blnFinished = true;
            CloseWindow();
        }

        private void btnGoToJail_Click(object sender, RoutedEventArgs e)
        {
            AddTextTT(GameState.CurrentUser.GoldOnHand < fine
                ? "You don't have the money required to pay the fine."
                : "You decide it is best to spend the night in jail.");

            GameState.CurrentUser.CurrentLocation = "Jail";
            blnFinished = true;
            CloseWindow();
        }

        private void btnFreedom_Click(object sender, RoutedEventArgs e)
        {
            AddTextTT("You beat a hasty retreat and return to the streets.");
            blnFinished = true;
            CloseWindow();
        }

        #endregion Button-Click Methods

        private void Justice()
        {
            if (blnGuilty)
            {
                if (GameState.CurrentUser.GoldOnHand >= fine)
                    btnPayFine.IsEnabled = true;
                btnGoToJail.IsEnabled = true;
            }
            else
                btnFreedom.IsEnabled = true;
        }

        #region Window-Manipulation Methods

        /// <summary>
        /// Closes the Window.
        /// </summary>
        private void CloseWindow()
        {
            this.Close();
        }

        public CourtWindow()
        {
            InitializeComponent();
            timer.Tick += timer_Tick;
            timer.Interval = new TimeSpan(0, 0, 2);
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if (index < courtText.Count)
            {
                AddTextTT(courtText[index]);
                index += 1;
            }

            if (index == courtText.Count)
            {
                timer.Stop();
                Justice();
                courtText.Clear();
                index = 0;
            }
        }

        private async void windowCourt_Closing(object sender, CancelEventArgs e)
        {
            if (blnFinished)
            {
                await GameState.SaveUser(GameState.CurrentUser);
                RefToGameWindow.AddTextTT(txtCourt.Text);
                RefToGameWindow.Show();
            }
            else
            {
                new Notification("You must first make a decision.", "Assassin", NotificationButtons.OK, this).ShowDialog();
                e.Cancel = true;
            }
        }

        #endregion Window-Manipulation Methods
    }
}