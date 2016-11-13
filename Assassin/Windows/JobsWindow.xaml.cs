using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Assassin
{
    /// <summary>
    /// Interaction logic for JobsWindow.xaml
    /// </summary>
    public partial class JobsWindow : Window
    {
        private string nl = Environment.NewLine;
        private List<Employer> employers = new List<Employer>();
        private Employer currentEmployer = new Employer();
        private int bounty = 0;

        internal GuildWindow RefToGuildWindow { get; set; }
        internal PubWindow RefToTavernWindow { get; set; }

        /// <summary>
        /// Adds text to the txtCourt Textbox.
        /// </summary>
        /// <param name="newText">Text to be added</param>
        internal void AddTextTT(string newText)
        {
            txtJob.Text += nl + nl + newText;
            txtJob.Focus();
            txtJob.CaretIndex = txtJob.Text.Length;
            txtJob.ScrollToEnd();
        }

        /// <summary>
        /// Checks the User's Hunger and Thirst to determine whether or not they can continue.
        /// </summary>
        /// <returns>Returns true if player isn't too hungry or thirst to continue.</returns>
        private bool CheckHungerThirst()
        {
            if (GameState.CurrentUser.Hunger >= 24 || GameState.CurrentUser.Thirst >= 24)
            {
                btnAccept.IsEnabled = false;
                btnDecline.IsEnabled = false;

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
                btnAccept.IsEnabled = true;
                btnDecline.IsEnabled = true;

                if (GameState.CurrentUser.Hunger > 0 && GameState.CurrentUser.Hunger % 5 == 0)
                    AddTextTT("You are " + GameState.CurrentUser.HungerToString.ToLower() + ".");
                if (GameState.CurrentUser.Thirst > 0 && GameState.CurrentUser.Thirst % 5 == 0)
                    AddTextTT("You are " + GameState.CurrentUser.ThirstToString.ToLower() + ".");
                return true;
            }
        }

        /// <summary>
        /// Generates list of Employers who offer jobs.
        /// </summary>
        private void GenerateEmployers()
        {
            employers.Add(new Employer("A", "youthful", "man"));
            employers.Add(new Employer("A", "middle-aged", "man"));
            employers.Add(new Employer("An", "old", "man"));
            employers.Add(new Employer("A", "youthful", "woman"));
            employers.Add(new Employer("A", "middle-aged", "woman"));
            employers.Add(new Employer("An", "old", "woman"));
        }

        /// <summary>
        /// Generates a job for the User.
        /// </summary>
        private void GenerateJob()
        {
            int selectedEmployer = GameState.GenerateRandomNumber(0, employers.Count - 1);
            currentEmployer = employers[selectedEmployer];
            AddTextTT(currentEmployer + " approaches you.");
            GameState.CurrentEnemy = GameState.SelectEnemy();
            bounty = GameState.GenerateRandomNumber(GameState.CurrentEnemy.GoldOnHand, GameState.CurrentEnemy.GoldOnHand * 2);
            AddTextTT("\"Greetings, " + GameState.CurrentUser.Name + ".\"" + nl +
                "I have an opportunity for you.\"" + nl +
                    "It concerns the elimination of a " + GameState.CurrentEnemy.Name + ".\"" + nl +
                    "I will pay you " + bounty + " gold.\"" + nl +
                    "Are you interested?\"");
        }

        internal void ReceivePayment()
        {
        }

        /// <summary>
        /// Increases the current User's Hunger and Thirst.
        /// </summary>
        private void IncreaseHungerThirst()
        {
            GameState.CurrentUser.Hunger += 1;
            GameState.CurrentUser.Thirst += 1;
        }

        #region Button Manipulation

        /// <summary>
        /// Disables all Buttons on the Window.
        /// </summary>
        private void DisableButtons()
        {
            btnAccept.IsEnabled = false;
            btnDecline.IsEnabled = false;
            btnLeaveTable.IsEnabled = false;
        }

        /// <summary>
        /// Enables all Buttons on the Window.
        /// </summary>
        private void EnableButtons()
        {
            btnAccept.IsEnabled = true;
            btnDecline.IsEnabled = true;
            btnLeaveTable.IsEnabled = true;
        }

        #endregion Button Manipulation

        #region Button-Click Methods

        private void btnAccept_Click(object sender, RoutedEventArgs e)
        {
            IncreaseHungerThirst();
        }

        private void btnDecline_Click(object sender, RoutedEventArgs e)
        {
            IncreaseHungerThirst();
            if (CheckHungerThirst())
                GenerateJob();
        }

        private void btnLeaveTable_Click(object sender, RoutedEventArgs e)
        {
        }

        #endregion Button-Click Methods

        #region Window-Manipulation Methods

        public JobsWindow()
        {
            InitializeComponent();
            GenerateEmployers();
            txtJob.Text = "You sit at a table and wait.";
            GenerateJob();
        }

        private void windowJobs_Closing(object sender, CancelEventArgs e)
        {
        }

        #endregion Window-Manipulation Methods
    }
}