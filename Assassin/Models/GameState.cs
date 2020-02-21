using Assassin.Models.Database;
using Assassin.Models.Entities;
using Assassin.Models.Items;
using Assassin.Views;
using Assassin.Views.City;
using Extensions;
using Extensions.Encryption;
using Extensions.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Assassin.Models
{
    /// <summary><summary>Represents the current state of the game.</summary>
    internal static class GameState
    {
        internal static string AdminPassword = "";
        internal static List<Armor> AllArmor = new List<Armor>();
        internal static List<Drink> AllDrinks = new List<Drink>();
        internal static List<Enemy> AllEnemies = new List<Enemy>();
        internal static List<Food> AllFood = new List<Food>();
        internal static List<Guild> AllGuilds = new List<Guild>();
        internal static List<JailedUser> AllJailedUsers = new List<JailedUser>();
        internal static List<Potion> AllPotions = new List<Potion>();
        internal static List<string> AllRanks = new List<string>();
        internal static List<User> AllUsers = new List<User>();
        internal static List<Weapon> AllWeapons = new List<Weapon>();
        internal static Enemy CurrentEnemy = new Enemy();
        internal static Guild CurrentGuild = new Guild();
        internal static User CurrentUser = new User();
        internal static User MaxStatsUsers = new User();
        internal static readonly SQLiteDatabaseInteraction DatabaseInteraction = new SQLiteDatabaseInteraction();

        #region Navigation

        internal static MainWindow MainWindow { get; set; }
        internal static GamePage GamePage { get; set; }

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

        /// <summary>Checks the database version so the table can be updated if necessary.</summary>
        private async static void CheckDatabaseVersion()
        {
            if (await DatabaseInteraction.LoadUserVersion() == 0)
            {
                await DatabaseInteraction.UpdateUserVersion(1);
                await DatabaseInteraction.ExecuteCommand("ALTER TABLE Guilds ADD COLUMN DefaultGuildmaster TEXT");
                await DatabaseInteraction.ExecuteCommand("Update Guilds SET [DefaultGuildmaster] = \"The Master\" WHERE [ID] = 1");
                await DatabaseInteraction.ExecuteCommand("Update Guilds SET [DefaultGuildmaster] = \"Rathskeller\" WHERE [ID] = 2");
            }
        }

        /// <summary>Handles verification of required files.</summary>
        internal static void FileManagement()
        {
            if (!Directory.Exists(AppData.Location))
                Directory.CreateDirectory(AppData.Location);
            DatabaseInteraction.VerifyDatabaseIntegrity();
            CheckDatabaseVersion();
        }

        /// <summary>Loads almost everything necessary for the game to function correctly.</summary>
        internal static async Task LoadAll()
        {
            FileManagement();
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
            AllJailedUsers = await DatabaseInteraction.LoadJailedUsers();
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

        #region Display Management

        /// <summary>Gets the hunger of a user and returns it as a String.</summary>
        /// <param name="hunger">Current hunger</param>
        /// <returns>String regarding hunger</returns>
        public static string GetHunger(int hunger)
        {
            if (hunger < 5)
                return "Full";
            else if (hunger < 10)
                return "Hungry";
            else if (hunger < 15)
                return "Very Hungry";
            else if (hunger < 20)
                return "Famished";
            else if (hunger < 25)
                return "Starving";
            return "BROKEN";
        }

        /// <summary>Gets the thirst of a user and returns it as a String.</summary>
        /// <param name="thirst">Current thirst</param>
        /// <returns>String regarding thirst</returns>
        public static string GetThirst(int thirst)
        {
            if (thirst < 5)
                return "Quenched";
            else if (thirst < 10)
                return "Thirsty";
            else if (thirst < 15)
                return "Very Thirsty";
            else if (thirst < 20)
                return "Parched";
            else if (thirst < 25)
                return "Dehydrated";
            return "BROKEN";
        }

        /// <summary>Selects an Enemy based on the User's current level.</summary>
        /// <returns>Selected Enemy</returns>
        internal static void SelectEnemy()
        {
            int level = 0;
            int enemy = Functions.GenerateRandomNumber(1, 100);
            switch (CurrentUser.Level)
            {
                case 1:
                    level = enemy <= 65 ? 1 : 2;
                    break;

                case 2:
                    if (enemy <= 40)
                        level = 1;
                    else if (enemy <= 80)
                        level = 2;
                    else if (enemy <= 95)
                        level = 3;
                    else
                        level = 4;
                    break;

                case 3:
                    if (enemy <= 20)
                        level = 1;
                    else if (enemy <= 40)
                        level = 2;
                    else if (enemy <= 80)
                        level = 3;
                    else if (enemy <= 95)
                        level = 4;
                    else
                        level = 5;
                    break;

                case 4:
                    if (enemy <= 10)
                        level = 1;
                    else if (enemy <= 20)
                        level = 2;
                    else if (enemy <= 40)
                        level = 3;
                    else if (enemy <= 70)
                        level = 4;
                    else if (enemy <= 90)
                        level = 5;
                    else
                        level = 6;
                    break;

                case 5:
                    if (enemy <= 5)
                        level = 1;
                    else if (enemy <= 10)
                        level = 2;
                    else if (enemy <= 20)
                        level = 3;
                    else if (enemy <= 50)
                        level = 4;
                    else if (enemy <= 75)
                        level = 5;
                    else if (enemy <= 90)
                        level = 6;
                    else
                        level = 7;
                    break;

                case 6:
                    if (enemy <= 2)
                        level = 1;
                    else if (enemy <= 4)
                        level = 2;
                    else if (enemy <= 10)
                        level = 3;
                    else if (enemy <= 25)
                        level = 4;
                    else if (enemy <= 50)
                        level = 5;
                    else if (enemy <= 75)
                        level = 6;
                    else if (enemy <= 90)
                        level = 7;
                    else
                        level = 8;
                    break;

                case 7:
                    if (enemy <= 5)
                        level = 3;
                    else if (enemy <= 10)
                        level = 4;
                    else if (enemy <= 35)
                        level = 5;
                    else if (enemy <= 60)
                        level = 6;
                    else if (enemy <= 85)
                        level = 7;
                    else if (enemy <= 95)
                        level = 8;
                    else
                        level = 9;
                    break;

                case 8:
                    if (enemy <= 5)
                        level = 4;
                    else if (enemy <= 15)
                        level = 5;
                    else if (enemy <= 30)
                        level = 6;
                    else if (enemy <= 55)
                        level = 7;
                    else if (enemy <= 85)
                        level = 8;
                    else if (enemy <= 95)
                        level = 9;
                    else
                        level = 10;
                    break;

                case 9:
                    if (enemy <= 5)
                        level = 5;
                    else if (enemy <= 15)
                        level = 6;
                    else if (enemy <= 30)
                        level = 7;
                    else if (enemy <= 50)
                        level = 8;
                    else if (enemy <= 85)
                        level = 9;
                    else
                        level = 10;
                    break;

                case 10:
                    if (enemy <= 5)
                        level = 6;
                    else if (enemy <= 15)
                        level = 7;
                    else if (enemy <= 35)
                        level = 8;
                    else if (enemy <= 65)
                        level = 9;
                    else
                        level = 10;
                    break;

                case 11:
                    if (enemy <= 15)
                        level = 7;
                    else if (enemy <= 30)
                        level = 8;
                    else if (enemy <= 45)
                        level = 9;
                    else
                        level = 10;
                    break;
            }

            List<Enemy> availableEnemies = AllEnemies.Where(availEnemy => availEnemy.Level == level).ToList();
            if (availableEnemies.Count == 0)
            {
                int counter = 1;
                while (availableEnemies.Count == 0)
                {
                    availableEnemies = AllEnemies.Where(availEnemy => level - counter >= availEnemy.Level && availEnemy.Level <= level + counter).ToList();
                    counter++;
                }
            }
            CurrentEnemy = new Enemy(availableEnemies[Functions.GenerateRandomNumber(0, availableEnemies.Count - 1)]);
            CurrentEnemy.GoldOnHand = Functions.GenerateRandomNumber(CurrentEnemy.GoldOnHand / 2, CurrentEnemy.GoldOnHand);
        }

        #endregion Display Management

        #region Guild Management

        /// <summary>Member of a Guild gains membership with that Guild, applied to database.</summary>
        /// <param name="joinUser">User joining the Guild.</param>
        /// <param name="joinGuild">Guild being joined</param>
        /// <returns>True if successful</returns>
        public static async Task<bool> MemberJoinsGuild(User joinUser, Guild joinGuild)
        {
            if (await DatabaseInteraction.MemberJoinsGuild(joinUser, joinGuild))
            {
                joinGuild.Members.Add(joinUser.Name);
                return true;
            }

            return false;
        }

        /// <summary>Member of a Guild terminates membership with that Guild, applied to database.</summary>
        /// <param name="leaveUser">User leaving the Guild.</param>
        /// <param name="leaveGuild">Guild being left</param>
        /// <returns>True if successful</returns>
        public static async Task<bool> MemberLeavesGuild(User leaveUser, Guild leaveGuild)
        {
            if (await DatabaseInteraction.MemberLeavesGuild(leaveUser, leaveGuild))
            {
                leaveGuild.Members.Remove(leaveUser.Name);
                return true;
            }
            return false;
        }

        #endregion Guild Management

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
            if (await DatabaseInteraction.NewUser(newUser) && await MemberJoinsGuild(newUser, AllGuilds[0]))
            {
                AllUsers.Add(newUser);
                AllUsers = AllUsers.OrderBy(user => user.Name).ToList();
                success = true;
            }

            return success;
        }

        #endregion User Management

        #region Notification Management

        /// <summary>Displays a new Notification in a thread-safe way.</summary>
        /// <param name="message">Message to be displayed</param>
        /// <param name="title">Title of the Notification window</param>
        internal static void DisplayNotification(string message, string title) => Application.Current.Dispatcher.Invoke(() => new Notification(message, title, NotificationButton.OK, MainWindow).ShowDialog());

        /// <summary>Displays a new <see cref="InputNotification"/> in a thread-safe way.</summary>
        /// <param name="message">Message to be displayed</param>
        /// <param name="title">Title of the <see cref="InputNotification"/> window</param>
        /// <param name="defaultText">Text to be displayed in the TxtInput TextBox by default.</param>
        internal static string InputDialog(string message, string title, string defaultText = "")
        {
            TextBox txtInput = new TextBox();
            InputNotification newIn = new InputNotification(message, title, MainWindow, defaultText);
            return Application.Current.Dispatcher.Invoke(() => newIn.ShowDialog()) == true ? newIn.TxtInput.Text : "";
        }

        /// <summary>Displays a new Notification in a thread-safe way and retrieves a boolean result upon its closing.</summary>
        /// <param name="message">Message to be displayed</param>
        /// <param name="title">Title of the Notification window</param>
        /// <returns>Returns value of clicked button on Notification.</returns>
        internal static bool YesNoNotification(string message, string title)
        {
            bool result = false;
            Application.Current.Dispatcher.Invoke(delegate
            {
                if (new Notification(message, title, NotificationButton.YesNo, MainWindow).ShowDialog() == true)
                    result = true;
            });
            return result;
        }

        #endregion Notification Management
    }
}