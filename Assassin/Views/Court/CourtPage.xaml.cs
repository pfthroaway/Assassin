using Assassin.Models;
using Assassin.Views.City;
using Extensions;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Threading;

namespace Assassin.Views.Court
{
    /// <summary>Interaction logic for CourtPage.xaml</summary>
    public partial class CourtPage
    {
        internal GamePage RefToGamePage { get; set; }

        private readonly List<string> _courtText = new List<string>();
        private int _index, _fine;
        private readonly DispatcherTimer _timer = new DispatcherTimer();
        private bool _blnGuilty, _blnFinished;
        private string _reason = "";

        /// <summary>Loads the Page and assigns the text to be displayed.</summary>
        /// <param name="arrestReason">Reason User was arrested</param>
        internal void LoadPage(string arrestReason)
        {
            _reason = arrestReason;
            _courtText.Add("You are dragged to the courts of justice.");
            _courtText.Add("The judge stares at you. . .");
            if (GameState.CurrentUser.Level > 6)
            {
                _courtText.Add("\"Well, if it isn't the " + GameState.CurrentUser.Rank + " , " + GameState.CurrentUser.Name + ".\"");
                _courtText.Add("\"Don't worry, I will be impartial,\" he laughs.");
            }
            _courtText.Add("The trial begins. . .");
            switch (_reason)
            {
                case "Robbery":
                    _courtText.Add("You are charged with the crime of attempted theft of property.");
                    _fine = 50;
                    break;

                case "Assault":
                    _courtText.Add("You are charged with the crime of attempted assault and robbery.");
                    _fine = 100;
                    break;

                case "Assassinate":
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
                _courtText.Add("\"You are to pay " + _fine + " gold as a fine. Pay it or you will be jailed for the night,\" the judge says.");
                _blnGuilty = true;
            }

            _timer.Start();
        }

        #region Button-Click Methods

        private void BtnPayFine_Click(object sender, RoutedEventArgs e)
        {
            Functions.AddTextToTextBox(TxtCourt, "You decide it is best to pay the fine and depart.");
            GameState.CurrentUser.GoldOnHand -= _fine;
            _blnFinished = true;
            ClosePage();
        }

        private void BtnGoToJail_Click(object sender, RoutedEventArgs e)
        {
            Functions.AddTextToTextBox(TxtCourt, GameState.CurrentUser.GoldOnHand < _fine
                ? "You don't have the money required to pay the fine."
                : "You decide it is best to spend the night in jail.");

            GameState.CurrentUser.CurrentLocation = "Jail";
            _blnFinished = true;
            ClosePage();
        }

        private void BtnFreedom_Click(object sender, RoutedEventArgs e)
        {
            Functions.AddTextToTextBox(TxtCourt, "You beat a hasty retreat and return to the streets.");
            _blnFinished = true;
            ClosePage();
        }

        #endregion Button-Click Methods

        private void Justice()
        {
            if (_blnGuilty)
            {
                if (GameState.CurrentUser.GoldOnHand >= _fine)
                    BtnPayFine.IsEnabled = true;
                BtnGoToJail.IsEnabled = true;
            }
            else
                BtnFreedom.IsEnabled = true;
        }

        #region Page-Manipulation Methods

        /// <summary>Closes the Page.</summary>
        private async void ClosePage()
        {
            if (_blnFinished)
            {
                GameState.GoBack();
                await GameState.SaveUser(GameState.CurrentUser);
            }
            else
                GameState.DisplayNotification("You must first make a decision.", "Assassin");
        }

        public CourtPage()
        {
            InitializeComponent();
            _timer.Tick += timer_Tick;
            _timer.Interval = new TimeSpan(0, 0, 2);
        }

        private void timer_Tick(object sender, EventArgs e)
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