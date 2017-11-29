using Assassin.Classes;
using Assassin.Classes.Entities;
using Assassin.Pages.Guilds;
using Assassin.Pages.Shopping;
using Extensions;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;

namespace Assassin.Pages.City
{
    /// <summary>Interaction logic for JobsPage.xaml</summary>
    public partial class JobsPage
    {
        private readonly List<Employer> _employers = new List<Employer>();
        private Employer _currentEmployer = new Employer();
        private int _bounty;

        internal GuildPage RefToGuildPage { get; set; }
        internal PubPage RefToTavernPage { get; set; }

        /// <summary>Checks the User's Hunger and Thirst to determine whether or not they can continue.</summary>
        /// <returns>Returns true if player isn't too hungry or thirst to continue.</returns>
        private bool CheckHungerThirst()
        {
            if (GameState.CurrentUser.Hunger >= 24 || GameState.CurrentUser.Thirst >= 24)
            {
                BtnAccept.IsEnabled = false;
                BtnDecline.IsEnabled = false;

                if (GameState.CurrentUser.Hunger >= 24 && GameState.CurrentUser.Thirst >= 24)
                    Functions.AddTextToTextBox(TxtJob, "You are too hungry and thirsty to continue.");
                else if (GameState.CurrentUser.Hunger >= 24)
                    Functions.AddTextToTextBox(TxtJob, "You are too hungry to continue.");
                else if (GameState.CurrentUser.Thirst >= 24)
                    Functions.AddTextToTextBox(TxtJob, "You are too thirsty to continue.");
                return false;
            }

            BtnAccept.IsEnabled = true;
            BtnDecline.IsEnabled = true;

            if (GameState.CurrentUser.Hunger > 0 && GameState.CurrentUser.Hunger % 5 == 0)
                Functions.AddTextToTextBox(TxtJob, "You are " + GameState.CurrentUser.HungerToString.ToLower() + ".");
            if (GameState.CurrentUser.Thirst > 0 && GameState.CurrentUser.Thirst % 5 == 0)
                Functions.AddTextToTextBox(TxtJob, "You are " + GameState.CurrentUser.ThirstToString.ToLower() + ".");
            return true;
        }

        /// <summary>Generates list of Employers who offer jobs.</summary>
        private void GenerateEmployers()
        {
            _employers.Add(new Employer("A", "youthful", "man"));
            _employers.Add(new Employer("A", "middle-aged", "man"));
            _employers.Add(new Employer("An", "old", "man"));
            _employers.Add(new Employer("A", "youthful", "woman"));
            _employers.Add(new Employer("A", "middle-aged", "woman"));
            _employers.Add(new Employer("An", "old", "woman"));
        }

        /// <summary>Generates a job for the User.</summary>
        private void GenerateJob()
        {
            int selectedEmployer = Functions.GenerateRandomNumber(0, _employers.Count - 1);
            _currentEmployer = _employers[selectedEmployer];
            Functions.AddTextToTextBox(TxtJob, _currentEmployer + " approaches you.");
            GameState.CurrentEnemy = GameState.SelectEnemy();
            _bounty = Functions.GenerateRandomNumber(GameState.CurrentEnemy.GoldOnHand, GameState.CurrentEnemy.GoldOnHand * 2);
            Functions.AddTextToTextBox(TxtJob,
                $"\"Greetings, {GameState.CurrentUser.Name}.\"\n" +
                $"I have an opportunity for you.\"\nIt concerns the elimination of a {GameState.CurrentEnemy.Name}.\"\n" +
                $"I will pay you {_bounty} gold.\"\n" +
                $"Are you interested?\"");
        }

        internal void ReceivePayment()
        {
        }

        /// <summary>Increases the current User's Hunger and Thirst.</summary>
        private static void IncreaseHungerThirst()
        {
            GameState.CurrentUser.Hunger++;
            GameState.CurrentUser.Thirst++;
        }

        #region Button Manipulation

        /// <summary>Toggles all Buttons on the Page.</summary>
        private void ToggleButtons(bool enabled)
        {
            BtnAccept.IsEnabled = enabled;
            BtnDecline.IsEnabled = enabled;
            BtnLeaveTable.IsEnabled = enabled;
        }

        #endregion Button Manipulation

        #region Button-Click Methods

        private void BtnAccept_Click(object sender, RoutedEventArgs e) => IncreaseHungerThirst();

        private void BtnDecline_Click(object sender, RoutedEventArgs e)
        {
            IncreaseHungerThirst();
            if (CheckHungerThirst())
                GenerateJob();
        }

        private void BtnLeaveTable_Click(object sender, RoutedEventArgs e)
        {
        }

        #endregion Button-Click Methods

        #region Page-Manipulation Methods

        public JobsPage()
        {
            InitializeComponent();
            GenerateEmployers();
            TxtJob.Text = "You sit at a table and wait.";
            GenerateJob();
        }

        private void windowJobs_Closing(object sender, CancelEventArgs e)
        {
        }

        #endregion Page-Manipulation Methods
    }
}