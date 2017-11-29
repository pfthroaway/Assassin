using Assassin.Classes.Entities;
using Assassin.Classes.Items;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Assassin.Classes.Database
{
    internal interface IDatabaseInteraction
    {
        /// <summary>Verifies that the requested database exists and that its file size is greater than zero. If not, it extracts the embedded database file to the local output folder.</summary>
        void VerifyDatabaseIntegrity();

        #region Load

        /// <summary>Loads the administrator password from the database.</summary>
        /// <returns>Admin password</returns>
        Task<string> LoadAdminPassword();

        /// <summary>Loads all Armor from the database.</summary>
        /// <returns>All Armor</returns>
        Task<List<Armor>> LoadArmor();

        /// <summary>Loads all Drinks from the database.</summary>
        /// <returns>All Drinks</returns>
        Task<List<Drink>> LoadDrinks();

        /// <summary>Loads all Enemies from the database.</summary>
        /// <returns>All Enemies</returns>
        Task<List<Enemy>> LoadEnemies();

        /// <summary>Loads all Food from the database.</summary>
        /// <returns>All Food</returns>
        Task<List<Food>> LoadFood();

        /// <summary>Loads all Guilds from the database.</summary>
        /// <returns>All Guilds</returns>
        Task<List<Guild>> LoadGuilds();

        /// <summary>Loads all Potions from the database.</summary>
        /// <returns>All Potions</returns>
        Task<List<Potion>> LoadPotions();

        /// <summary>Loads all Ranks from the database.</summary>
        /// <returns>All Ranks</returns>
        Task<List<string>> LoadRanks();

        /// <summary>Loads a <see cref="User"/> from the database.</summary>
        /// <param name="username"><see cref="User"/> to load from the database</param>
        /// <returns>Requested User</returns>
        Task<User> LoadUser(string username);

        /// <summary>Loads all <see cref="User"/>s from the database.</summary>
        /// <returns>All <see cref="User"/>s</returns>
        Task<List<User>> LoadUsers();

        /// <summary>Loads all Weapons from the database.</summary>
        /// <returns>All Weapons</returns>
        Task<List<Weapon>> LoadWeapons();

        #endregion Load

        #region User Management

        /// <summary>Changes a User's details in the database.</summary>
        /// <param name="oldUser">User to be updated</param>
        /// <param name="newUser">User with new details</param>
        /// <returns>True if successful</returns>
        Task<bool> ChangeUserDetails(User oldUser, User newUser);

        /// <summary>Adds a new User to the database.</summary>
        /// <param name="newUser">User to be added</param>
        /// <returns>True if successful</returns>
        Task<bool> NewUser(User newUser);

        /// <summary>Saves the current User.</summary>
        /// /// <param name="saveUser">User to be saved.</param>
        /// <returns>True if successful</returns>
        Task<bool> SaveUser(User saveUser);

        #endregion User Management
    }
}