using Assassin.Classes.Database;
using Assassin.Classes.Entities;
using Assassin.Classes.Items;
using Assassin.Pages;
using Extensions;
using Extensions.Encryption;
using Extensions.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Assassin.Classes
{
    internal static class GameState
    {
        internal static User CurrentUser = new User();
        internal static User MaxStatsUsers = new User();
        internal static Enemy CurrentEnemy = new Enemy();
        internal static List<User> AllUsers = new List<User>();
        internal static List<Enemy> AllEnemies = new List<Enemy>();
        internal static List<Weapon> AllWeapons = new List<Weapon>();
        internal static List<Armor> AllArmor = new List<Armor>();
        internal static List<Guild> AllGuilds = new List<Guild>();
        internal static List<string> AllRanks = new List<string>();
        internal static List<Potion> AllPotions = new List<Potion>();
        internal static List<Food> AllFood = new List<Food>();
        internal static List<Drink> AllDrinks = new List<Drink>();
        internal static string AdminPassword = "";
        private static readonly SQLiteDatabaseInteraction DatabaseInteraction = new SQLiteDatabaseInteraction();
        internal static MainWindow MainWindow { get; set; }

        #region Navigation

        internal static double CurrentPageWidth { get; set; }
        internal static double CurrentPageHeight { get; set; }

        /// <summary>Calculates the scale needed for the MainWindow.</summary>
        /// <param name="grid">Grid of current Page</param>
        internal static void CalculateScale(Grid grid)
        {
            CurrentPageHeight = grid.ActualHeight;
            CurrentPageWidth = grid.ActualWidth;
            MainWindow.CalculateScale();

            Page newPage = MainWindow.MainFrame.Content as Page;
            if (newPage != null)
                newPage.Style = (Style)MainWindow.FindResource("PageStyle");
        }

        /// <summary>Navigates to selected Page.</summary>
        /// <param name="newPage">Page to navigate to.</param>
        internal static void Navigate(Page newPage) => MainWindow.MainFrame.Navigate(newPage);

        /// <summary>Navigates to the previous Page.</summary>
        internal static void GoBack()
        {
            if (MainWindow.MainFrame.CanGoBack)
                MainWindow.MainFrame.GoBack();
        }

        #endregion Navigation

        /// <summary>Loads almost everything necessary for the game to function correctly.</summary>
        internal static async void LoadAll()
        {
            DatabaseInteraction.VerifyDatabaseIntegrity();
            AdminPassword = await DatabaseInteraction.LoadAdminPassword();
            AllArmor = await DatabaseInteraction.LoadArmor();
            AllWeapons = await DatabaseInteraction.LoadWeapons();
            AllDrinks = await DatabaseInteraction.LoadDrinks();
            AllFood = await DatabaseInteraction.LoadFood();
            AllGuilds = await DatabaseInteraction.LoadGuilds();
            AllPotions = await DatabaseInteraction.LoadPotions();
            AllRanks = await DatabaseInteraction.LoadRanks();
            AllUsers = await DatabaseInteraction.LoadUsers();
            AllEnemies = await DatabaseInteraction.LoadEnemies();
        }

        /// <summary>Checks whether a valid login has occurred.</summary>
        /// <param name="username">Username</param>
        /// <param name="password">Password</param>
        /// <returns>True if valid</returns>
        internal static bool CheckLogin(string username, string password)
        {
            try
            {
                User checkUser = AllUsers.Find(hero => string.Equals(hero.Name, username, StringComparison.OrdinalIgnoreCase));
                if (PBKDF2.ValidatePassword(password, checkUser.Password))
                {
                    CurrentUser = checkUser;
                    return true;
                }
                Application.Current.Dispatcher.Invoke(delegate
                    { DisplayNotification("Invalid login.", "Assassin"); });
                return false;
            }
            catch (Exception)
            {
                Application.Current.Dispatcher.Invoke(delegate
                    { DisplayNotification("Invalid login.", "Assassin"); });
                return false;
            }
        }

        #region User Management

        /// <summary>Changes a User's details in the database.</summary>
        /// <param name="oldUser">User to be updated</param>
        /// <param name="newUser">User with new details</param>
        /// <returns>True if successful</returns>
        internal static async Task<bool> ChangeUserDetails(User oldUser, User newUser) => await DatabaseInteraction.ChangeUserDetails(oldUser, newUser);

        /// <summary>Adds a new User to the database.</summary>
        /// <param name="newUser">User to be added</param>
        /// <returns>True if successful</returns>
        internal static async Task<bool> NewUser(User newUser)
        {
            bool success = false;
            if (await DatabaseInteraction.NewUser(newUser))
            {
                AllUsers.Add(newUser);
                AllUsers = AllUsers.OrderBy(user => user.Name).ToList();
                success = true;
            }

            return success;
        }

        /// <summary>Saves the current User.</summary>
        /// /// <param name="saveUser">User to be saved.</param>
        /// <returns>True if successful</returns>
        internal static async Task<bool> SaveUser(User saveUser) => await DatabaseInteraction.SaveUser(saveUser);

        #endregion User Management

        /// <summary>Selects an Enemy based on the User's current level.</summary>
        /// <returns>Selected Enemy</returns>
        internal static Enemy SelectEnemy()
        {
            int index = 0;
            int random = Functions.GenerateRandomNumber(1, 100);
            switch (CurrentUser.Level)
            {
                case 1:
                    index = random <= 65 ? 0 : 1;
                    break;

                case 2:
                    if (random <= 40)
                        index = 0;
                    else if (random <= 80)
                        index = 1;
                    else if (random <= 95)
                        index = 2;
                    else
                        index = 3;
                    break;

                case 3:
                    if (random <= 20)
                        index = 0;
                    else if (random <= 40)
                        index = 1;
                    else if (random <= 80)
                        index = 2;
                    else if (random <= 95)
                        index = 3;
                    else
                        index = 4;
                    break;

                case 4:
                    if (random <= 10)
                        index = 0;
                    else if (random <= 20)
                        index = 1;
                    else if (random <= 40)
                        index = 2;
                    else if (random <= 70)
                        index = 3;
                    else if (random <= 90)
                        index = 4;
                    else
                        index = 5;
                    break;

                case 5:
                    if (random <= 5)
                        index = 0;
                    else if (random <= 10)
                        index = 1;
                    else if (random <= 20)
                        index = 2;
                    else if (random <= 50)
                        index = 3;
                    else if (random <= 75)
                        index = 4;
                    else if (random <= 90)
                        index = 5;
                    else
                        index = 6;
                    break;

                case 6:
                    if (random <= 2)
                        index = 0;
                    else if (random <= 4)
                        index = 1;
                    else if (random <= 10)
                        index = 2;
                    else if (random <= 25)
                        index = 3;
                    else if (random <= 50)
                        index = 4;
                    else if (random <= 75)
                        index = 5;
                    else if (random <= 90)
                        index = 6;
                    else
                        index = 7;
                    break;

                case 7:
                    if (random <= 5)
                        index = 2;
                    else if (random <= 10)
                        index = 3;
                    else if (random <= 35)
                        index = 4;
                    else if (random <= 60)
                        index = 5;
                    else if (random <= 85)
                        index = 6;
                    else if (random <= 95)
                        index = 7;
                    else
                        index = 8;
                    break;

                case 8:
                    if (random <= 5)
                        index = 3;
                    else if (random <= 15)
                        index = 4;
                    else if (random <= 30)
                        index = 5;
                    else if (random <= 55)
                        index = 6;
                    else if (random <= 85)
                        index = 7;
                    else if (random <= 95)
                        index = 8;
                    else
                        index = 9;
                    break;

                case 9:
                    if (random <= 5)
                        index = 4;
                    else if (random <= 15)
                        index = 5;
                    else if (random <= 30)
                        index = 6;
                    else if (random <= 50)
                        index = 7;
                    else if (random <= 85)
                        index = 8;
                    else
                        index = 9;
                    break;

                case 10:
                    if (random <= 5)
                        index = 5;
                    else if (random <= 15)
                        index = 6;
                    else if (random <= 35)
                        index = 7;
                    else if (random <= 65)
                        index = 8;
                    else
                        index = 9;
                    break;

                case 11:
                    if (random <= 15)
                        index = 6;
                    else if (random <= 30)
                        index = 7;
                    else if (random <= 45)
                        index = 8;
                    else
                        index = 9;
                    break;
            }

            Enemy newEnemy = new Enemy(AllEnemies[index]);
            newEnemy.GoldOnHand = Functions.GenerateRandomNumber(newEnemy.GoldOnHand / 2, newEnemy.GoldOnHand);
            return newEnemy;
        }

        #region Notification Management

        /// <summary>Displays a new Notification in a thread-safe way.</summary>
        /// <param name="message">Message to be displayed</param>
        /// <param name="title">Title of the Notification window</param>
        internal static void DisplayNotification(string message, string title) => Application.Current.Dispatcher.Invoke(delegate
                                                                                {
                                                                                    new Notification(message, title, NotificationButtons.OK, MainWindow).ShowDialog();
                                                                                });

        /// <summary>Displays a new Notification in a thread-safe way and retrieves a boolean result upon its closing.</summary>
        /// <param name="message">Message to be displayed</param>
        /// <param name="title">Title of the Notification window</param>
        /// <returns>Returns value of clicked button on Notification.</returns>
        internal static bool YesNoNotification(string message, string title)
        {
            bool result = false;
            Application.Current.Dispatcher.Invoke(delegate
            {
                if (new Notification(message, title, NotificationButtons.YesNo, MainWindow).ShowDialog() == true)
                    result = true;
            });
            return result;
        }

        #endregion Notification Management
    }
}