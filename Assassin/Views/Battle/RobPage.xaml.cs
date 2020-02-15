using Assassin.Models;
using Assassin.Models.Enums;
using Assassin.Views.City;
using Extensions;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Threading;

namespace Assassin.Views.Battle
{
    /// <summary>Interaction logic for RobPage.xaml</summary>
    public partial class RobPage
    {
        private bool _blnCourt;
        private Crime _reason;
        private readonly DispatcherTimer Timer1 = new DispatcherTimer();
        private int _index;
        private readonly List<string> _arrText = new List<string>();

        /// <summary>Checks the User's Hunger and Thirst to determine whether or not they can continue.</summary>
        /// <returns>Returns true if player isn't too hungry or thirst to continue.</returns>
        private bool CheckHungerThirst()
        {
            Functions.AddTextToTextBox(TxtRob, GameState.CurrentUser.DisplayHungerThirstText());
            if (GameState.CurrentUser.CanDoAction())
                return true;
            ToggleButtons(false);
            BtnBack.IsEnabled = true;
            return false;
        }

        /// <summary>Loads the Robbery Page.</summary>
        private void LoadRobbery()
        {
            if (CheckHungerThirst())
            {
                TxtRob.Text = "You proceed to look for someone to rob.";
                GameState.SelectEnemy();
            }
        }

        /// <summary>Displays text while the Timer is active.</summary>
        private void Display()
        {
            if (_index < _arrText.Count)
            {
                Functions.AddTextToTextBox(TxtRob, _arrText[_index]);
                _index++;
                if (_index == _arrText.Count)
                {
                    _arrText.Clear();
                    Timer1.Stop();
                    _index = 0;
                    if (!_blnCourt)
                    {
                        ToggleBackNewVictim(true);
                        CheckHungerThirst();
                    }
                    else
                    {
                        BtnBack.IsEnabled = true;
                        BtnBack.Content = "_Court";
                    }
                }
            }
        }

        /// <summary>Toggles the Back and New Victim Buttons.</summary>
        private void ToggleBackNewVictim(bool enabled)
        {
            BtnNewVictim.IsEnabled = enabled;
            BtnBack.IsEnabled = enabled;
        }

        /// <summary>Toggles all the Buttons.</summary>
        /// <param name="enabled">Should the Buttons be enabled?</param>
        private void ToggleButtons(bool enabled)
        {
            ToggleBackNewVictim(enabled);
            BtnPickpocket.IsEnabled = enabled;
            BtnWaylay.IsEnabled = enabled;
        }

        #region Robbery

        /// <summary>The <see cref="User"/> failed in attemping to rob an <see cref="Enemy"/>.</summary>
        private void FailedRobbery()
        {
            Functions.AddTextToTextBox(TxtRob, "You have failed miserably!");
            int noticed = Functions.GenerateRandomNumber(1, 100);
            if (noticed <= GameState.CurrentUser.Stealth) //not spotted
            {
                Functions.AddTextToTextBox(TxtRob, "You got away without being noticed.");
                ToggleBackNewVictim(true);
            }
            else //spotted
            {
                _arrText.Add("You have been noticed!");
                _arrText.Add($"The {GameState.CurrentEnemy.Name} cries out, \"Guards, a thief!\"");
                _arrText.Add("You hide. . .");
                int spot = Functions.GenerateRandomNumber(1, 100);
                if (spot <= GameState.CurrentUser.Stealth) //not caught
                {
                    _arrText.Add("It appears you have escaped!");
                    Timer1.Start();
                }
                else //caught
                {
                    _blnCourt = true;
                    _arrText.Add("You have been caught!");
                    Timer1.Start();
                }
            }
        }

        /// <summary>Starts a robbery.</summary>
        private void StartRobbery()
        {
            ToggleButtons(false);
            GameState.CurrentUser.GainHungerThirst();
        }

        #endregion Robbery

        #region Click

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            if (!_blnCourt)
                GameState.GoBack();
            else
            {
                GameState.Navigate(new CourtPage(_reason));
            }
        }

        private async void BtnPickpocket_Click(object sender, RoutedEventArgs e)
        {
            if (CheckHungerThirst())
            {
                StartRobbery();
                int success = Functions.GenerateRandomNumber(1, 100);
                if (success <= GameState.CurrentUser.Stealth)  //successful pickpocket
                {
                    int gold = Functions.GenerateRandomNumber(1, GameState.CurrentEnemy.MaximumEndurance);
                    Functions.AddTextToTextBox(TxtRob, $"You manage to steal a pouch containing {gold} gold!");
                    GameState.CurrentUser.GoldOnHand += gold;
                    await GameState.DatabaseInteraction.SaveUser(GameState.CurrentUser);
                    ToggleBackNewVictim(true);
                }
                else //failure pickpocket
                {
                    _reason = Crime.Pickpocket;
                    FailedRobbery();
                }
            }
        }

        private void BtnNewVictim_Click(object sender, RoutedEventArgs e)
        {
            GameState.SelectEnemy();
            Functions.AddTextToTextBox(TxtRob, "You search for a new victim.");
            ToggleButtons(true);
        }

        private async void BtnWaylay_Click(object sender, RoutedEventArgs e)
        {
            if (CheckHungerThirst())
            {
                StartRobbery();
                int success = Functions.GenerateRandomNumber(1, 100);
                if (success <= GameState.CurrentUser.Stealth) //successful waylay
                {
                    int type = Functions.GenerateRandomNumber(1, 3);
                    int gold = Functions.GenerateRandomNumber(1, GameState.CurrentEnemy.MaximumEndurance);
                    if (type == 1) //gold only
                    {
                        Functions.AddTextToTextBox(TxtRob, $"You manage to steal a pouch containing {gold} gold!");
                        GameState.CurrentUser.GoldOnHand += gold;
                        ToggleBackNewVictim(true);
                    }
                    else if (type == 2) //weapon only
                    {
                        Functions.AddTextToTextBox(TxtRob, $"You manage to pickpocket their {GameState.CurrentEnemy.Weapon.Name}!");
                        _arrText.Add($"You bring it to the weaponsmith and sell it for {GameState.CurrentEnemy.Weapon.SellValue} gold!");
                        GameState.CurrentUser.GoldOnHand += GameState.CurrentEnemy.Weapon.SellValue;
                        Timer1.Start();
                    }
                    else if (type == 3) //weapon and gold
                    {
                        Functions.AddTextToTextBox(TxtRob, $"You manage to steal a pouch containing {gold} gold!");
                        _arrText.Add($"You also manage to pickpocket their {GameState.CurrentEnemy.Weapon.Name}!");
                        _arrText.Add($"You bring it to the weapon shop and sell it for {GameState.CurrentEnemy.Weapon.SellValue} gold!");
                        GameState.CurrentUser.GoldOnHand += GameState.CurrentEnemy.Weapon.SellValue + gold;
                        Timer1.Start();
                    }

                    await GameState.DatabaseInteraction.SaveUser(GameState.CurrentUser);
                }
                else //failure waylay
                {
                    _reason = Crime.Assault;
                    FailedRobbery();
                }
            }
        }

        #endregion Click

        #region Page-Manipulation Methods

        public RobPage()
        {
            InitializeComponent();
            Timer1.Tick += Timer1_Tick;
            Timer1.Interval = new TimeSpan(0, 0, 1);
            LoadRobbery();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void Timer1_Tick(object sender, EventArgs e) => Display();

        #endregion Page-Manipulation Methods
    }
}