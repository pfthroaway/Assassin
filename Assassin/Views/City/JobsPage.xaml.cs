using Assassin.Models;
using Assassin.Models.Entities;
using Assassin.Views.Battle;
using Assassin.Views.Guilds;
using Assassin.Views.Shopping;
using Extensions;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;

namespace Assassin.Views.City
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
            Functions.AddTextToTextBox(TxtJob, GameState.CurrentUser.DisplayHungerThirstText());
            if (GameState.CurrentUser.CanDoAction())
                return true;
            BtnAccept.IsEnabled = false;
            BtnDecline.IsEnabled = false;
            return false;
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
            GameState.SelectEnemy();
            _bounty = Functions.GenerateRandomNumber(GameState.CurrentEnemy.GoldOnHand, GameState.CurrentEnemy.GoldOnHand * 2);
            Functions.AddTextToTextBox(TxtJob,
                $"\"Greetings, {GameState.CurrentUser.Name}.\"\n" +
                $"\"I have an opportunity for you.\"\n" +
                $"\"It concerns the elimination of a {GameState.CurrentEnemy.Name}.\"\n" +
                $"\"I will pay you {_bounty} gold.\"\n" +
                "\"Are you interested?\"");
        }

        /// <summary>Handles getting paid.</summary>
        public async void GetPaid()
        {
            ToggleButtons(false);
            BtnLeaveTable.IsEnabled = true;
            Functions.AddTextToTextBox(TxtJob, $"The {_currentEmployer.Age} {_currentEmployer.Gender} welcomes your return.\n\"Well done. Here is your payment.\"\nYou are handed {_bounty} gold.");
            GameState.CurrentUser.GoldOnHand += _bounty;

            await GameState.DatabaseInteraction.SaveUser(GameState.CurrentUser);
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

        private void BtnAccept_Click(object sender, RoutedEventArgs e)
        {
            if (CheckHungerThirst())
            {
                GameState.CurrentUser.GainHungerThirst();
                BattlePage battlePage = new BattlePage() { RefToJobsPage = this, BlnJob = true };
                battlePage.TxtBattle.Text = "You stalk your opponent.";
                GameState.Navigate(battlePage);
            }
            else
                BtnLeaveTable.IsEnabled = true;
        }

        private void BtnDecline_Click(object sender, RoutedEventArgs e)
        {
            if (CheckHungerThirst())
            {
                GameState.CurrentUser.GainHungerThirst();
                GenerateJob();
            }
            else
                BtnLeaveTable.IsEnabled = true;
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