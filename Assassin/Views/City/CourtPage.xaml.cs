using Assassin.Models;
using Assassin.Models.Entities;
using Assassin.Models.Enums;
using Extensions;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Threading;

namespace Assassin.Views.City
{
    /// <summary>Interaction logic for CourtPage.xaml</summary>
    public partial class CourtPage
    {
        private readonly List<string> _courtText = new List<string>();
        private int _index, _fine;
        private readonly DispatcherTimer _timer = new DispatcherTimer();
        private bool _blnGuilty, _blnFinished;
        private Crime _reason;

        ///<summary>Administers justice!</summary>
        private void Justice()
        {
            if (_blnGuilty)
            {
                if (GameState.CurrentUser.GoldOnHand >= _fine)
                    BtnPayFine.IsEnabled = true;
                BtnJail.IsEnabled = true;
            }
            else
                BtnFreedom.IsEnabled = true;
        }

        /// <summary>Loads the Page and assigns the text to be displayed.</summary>
        /// <param name="arrestReason">Reason User was arrested</param>
        internal void LoadPage(Crime arrestReason)
        {
            _reason = arrestReason;
            _courtText.Add("You are dragged to the courts of justice.");
            _courtText.Add("The judge stares at you. . .");
            if (GameState.CurrentUser.Level > 6)
            {
                _courtText.Add($"\"Well, if it isn't the {GameState.CurrentUser.Rank}, {GameState.CurrentUser.Name}.\"");
                _courtText.Add("\"Don't worry, I will be impartial,\" he laughs.");
            }
            _courtText.Add("The trial begins. . .");
            switch (_reason)
            {
                case Crime.Pickpocket:
                    _courtText.Add("You are charged with the crime of attempted theft of property.");
                    _fine = 50;
                    break;

                case Crime.Assault:
                    _courtText.Add("You are charged with the crime of attempted assault and robbery.");
                    _fine = 100;
                    break;

                case Crime.AttemptedMurder:
                    _courtText.Add("You are charged with the crime of attemped murder.");
                    _fine = 250;
                    break;
            }

            _courtText.Add("Prosecution. . .");
            _courtText.Add("Defense. . .");
            int guilty = Functions.GenerateRandomNumber(1, 100);
            _courtText.Add("The judge finds you. . .");
            if (guilty <= 100 - (GameState.CurrentUser.Level * 5))
                _courtText.Add("innocent!");
            else
            {
                _courtText.Add("guilty!");
                _courtText.Add($"\"You are to pay {_fine} gold as a fine. Pay it or you will be jailed for the night,\" the judge says.");
                _blnGuilty = true;
            }

            _timer.Start();
        }

        #region Button-Click Methods

        private void BtnFreedom_Click(object sender, RoutedEventArgs e)
        {
            Functions.AddTextToTextBox(TxtCourt, "You beat a hasty retreat and return to the streets.");
            _blnFinished = true;
            ClosePage();
        }

        private async void BtnGoToJail_Click(object sender, RoutedEventArgs e)
        {
            Functions.AddTextToTextBox(TxtCourt, GameState.CurrentUser.GoldOnHand < _fine
                ? "You don't have the money required to pay the fine."
                : "You decide it is best to spend the night in jail.");

            GameState.CurrentUser.CurrentLocation = SleepLocation.Jail;
            JailedUser jailedUser = new JailedUser(GameState.CurrentUser.Name, _reason, _fine, DateTime.UtcNow);
            if (await GameState.DatabaseInteraction.SendToJail(jailedUser))
            {
                BtnPayFine.IsEnabled = false;
                BtnJail.IsEnabled = false;
                GameState.CurrentUser.CurrentLocation = SleepLocation.Jail;
                GameState.AllJailedUsers.Add(jailedUser);
                _blnFinished = true;
                BtnFreedom.IsEnabled = true;
                BtnFreedom.Content = "_Back";
            }
            _blnFinished = true;
            ClosePage();
        }

        private void BtnPayFine_Click(object sender, RoutedEventArgs e)
        {
            Functions.AddTextToTextBox(TxtCourt, "You decide it is best to pay the fine and depart.");
            GameState.CurrentUser.GoldOnHand -= _fine;
            _blnFinished = true;
            ClosePage();
        }

        #endregion Button-Click Methods

        #region Page-Manipulation Methods

        /// <summary>Closes the Page.</summary>
        private async void ClosePage()
        {
            if (_blnFinished)
            {
                GameState.MainWindow.MainFrame.RemoveBackEntry();
                GameState.GoBack();
                await GameState.DatabaseInteraction.SaveUser(GameState.CurrentUser);
            }
            else
                GameState.DisplayNotification("You must first make a decision.", "Assassin");
        }

        public CourtPage(Crime reason)
        {
            InitializeComponent();
            LoadPage(reason);
            _timer.Tick += Timer_Tick;
            _timer.Interval = new TimeSpan(0, 0, 0, 1, 500);
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (_index < _courtText.Count)
            {
                Functions.AddTextToTextBox(TxtCourt, _courtText[_index]);
                _index++;
            }

            if (_index == _courtText.Count)
            {
                _timer.Stop();
                Justice();
                _courtText.Clear();
                _index = 0;
            }
        }

        #endregion Page-Manipulation Methods
    }
}