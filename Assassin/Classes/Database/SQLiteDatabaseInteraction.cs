using Assassin.Classes.Entities;
using Assassin.Classes.Enums;
using Assassin.Classes.Items;
using Extensions;
using Extensions.DatabaseHelp;
using Extensions.DataTypeHelpers;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Assassin.Classes.Database
{
    internal class SQLiteDatabaseInteraction : IDatabaseInteraction
    {
        // ReSharper disable once InconsistentNaming
        private const string _DATABASENAME = "Assassin.sqlite";

        private readonly string _con = $"Data Source = {_DATABASENAME}; foreign keys = TRUE; Version=3";

        /// <summary>Verifies that the requested database exists and that its file size is greater than zero. If not, it extracts the embedded database file to the local output folder.</summary>
        public void VerifyDatabaseIntegrity() => Functions.VerifyFileIntegrity(Assembly.GetExecutingAssembly().GetManifestResourceStream($"Assassin.{_DATABASENAME}"),
                _DATABASENAME);

        #region Load

        /// <summary>Loads the administrator password from the database.</summary>
        /// <returns>Admin password</returns>
        public async Task<string> LoadAdminPassword()
        {
            DataSet ds = await SQLite.FillDataSet(_con, "SELECT * FROM Admin");
            string pass = "";
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                    pass = ds.Tables[0].Rows[0]["AdminPassword"].ToString();
            }

            return pass;
        }

        /// <summary>Loads all <see cref="Armor"/> from the database.</summary>
        /// <returns>All <see cref="Armor"/></returns>
        public async Task<List<Armor>> LoadArmor()
        {
            DataSet ds = await SQLite.FillDataSet(_con, "SELECT * FROM Armor");
            List<Armor> allArmor = new List<Armor>();
            if (ds.Tables[0].Rows.Count > 0)
                allArmor.AddRange(from DataRow dr in ds.Tables[0].Rows select new Armor(dr["ArmorName"].ToString(), Int32Helper.Parse(dr["ArmorDefense"]), Int32Helper.Parse(dr["ArmorValue"]), BoolHelper.Parse(dr["Hidden"])));

            return allArmor.OrderBy(armor => armor.Value).ToList();
        }

        /// <summary>Loads all <see cref="Drink"/>s from the database.</summary>
        /// <returns>All <see cref="Drink"/>s</returns>
        public async Task<List<Drink>> LoadDrinks()
        {
            DataSet ds = await SQLite.FillDataSet(_con, "SELECT * FROM Drinks");
            List<Drink> allDrinks = new List<Drink>();
            if (ds.Tables[0].Rows.Count > 0)
                allDrinks.AddRange(from DataRow dr in ds.Tables[0].Rows select new Drink(new Drink(dr["DrinkName"].ToString(), Int32Helper.Parse(dr["RestoreThirst"]), Int32Helper.Parse(dr["DrinkValue"]))));

            return allDrinks.OrderBy(drink => drink.Value).ToList();
        }

        /// <summary>Loads all <see cref="Enemy"/> from the database.</summary>
        /// <returns>All <see cref="Enemy"/></returns>
        public async Task<List<Enemy>> LoadEnemies()
        {
            DataSet ds = await SQLite.FillDataSet(_con, "SELECT * FROM Enemies");
            List<Enemy> allEnemies = new List<Enemy>();
            if (ds.Tables[0].Rows.Count > 0)
                allEnemies.AddRange(from DataRow dr in ds.Tables[0].Rows select new Enemy(dr["EnemyName"].ToString(), Int32Helper.Parse(dr["Level"]), Int32Helper.Parse(dr["Endurance"]), Int32Helper.Parse(dr["Endurance"]), new Weapon(GameState.AllWeapons.Find(wpn => wpn.Name == dr["Weapon"].ToString())), new Armor(GameState.AllArmor.Find(armr => armr.Name == dr["Armor"].ToString())), Int32Helper.Parse(dr["Gold"]), Int32Helper.Parse(dr["Attack"]), Int32Helper.Parse(dr["Blocking"]), Int32Helper.Parse(dr["Slipping"])));

            return allEnemies.OrderBy(enemy => enemy.Level).ToList();
        }

        /// <summary>Loads all <see cref="Food"/> from the database.</summary>
        /// <returns>All <see cref="Food"/></returns>
        public async Task<List<Food>> LoadFood()
        {
            DataSet ds = await SQLite.FillDataSet(_con, "SELECT * FROM Food");
            List<Food> allFood = new List<Food>();
            if (ds.Tables[0].Rows.Count > 0)
                allFood.AddRange(from DataRow dr in ds.Tables[0].Rows select new Food(new Food(dr["FoodName"].ToString(), Int32Helper.Parse(dr["RestoreHunger"]), Int32Helper.Parse(dr["FoodValue"]))));

            return allFood.OrderBy(food => food.Value).ToList();
        }

        /// <summary>Loads all <see cref="Guild"/>s from the database.</summary>
        /// <returns>All <see cref="Guild"/>s</returns>
        public async Task<List<Guild>> LoadGuilds()
        {
            DataSet ds = await SQLite.FillDataSet(_con, "SELECT * FROM Guilds");
            List<Guild> allGuilds = new List<Guild>();

            if (ds.Tables[0].Rows.Count > 0)
                allGuilds.AddRange(from DataRow dr in ds.Tables[0].Rows select new Guild(Int32Helper.Parse(dr["ID"]), dr["GuildName"].ToString(), dr["Guildmaster"].ToString(), Int32Helper.Parse(dr["GuildFee"]), Int32Helper.Parse(dr["GuildGold"]), new List<string>(), Int32Helper.Parse(dr["HenchmenLevel1"]), Int32Helper.Parse(dr["HenchmenLevel2"]), Int32Helper.Parse(dr["HenchmenLevel3"]), Int32Helper.Parse(dr["HenchmenLevel4"]), Int32Helper.Parse(dr["HenchmenLevel5"])));

            return allGuilds.OrderBy(guild => guild.ID).ToList();
        }

        /// <summary>Loads all <see cref="Potion"/>s from the database.</summary>
        /// <returns>All <see cref="Potion"/>s</returns>
        public async Task<List<Potion>> LoadPotions()
        {
            DataSet ds = await SQLite.FillDataSet(_con, "SELECT * FROM Potions");
            List<Potion> allPotions = new List<Potion>();

            if (ds.Tables[0].Rows.Count > 0)
                allPotions.AddRange(from DataRow dr in ds.Tables[0].Rows select new Potion(dr["PotionName"].ToString(), Int32Helper.Parse(dr["PotionHeal"]), Int32Helper.Parse(dr["PotionValue"])));

            return allPotions.OrderBy(potion => potion.Value).ToList();
        }

        /// <summary>Loads all Ranks from the database.</summary>
        /// <returns>All Ranks</returns>
        public async Task<List<string>> LoadRanks()
        {
            DataSet ds = await SQLite.FillDataSet(_con, "SELECT * FROM Ranks");
            List<string> allRanks = new List<string>();

            if (ds.Tables[0].Rows.Count > 0)
                allRanks.AddRange(from DataRow dr in ds.Tables[0].Rows select dr["RankName"].ToString());

            return allRanks;
        }

        /// <summary>Loads a <see cref="User"/> from the database.</summary>
        /// <param name="username"><see cref="User"/> to load from the database</param>
        /// <returns>Requested <see cref="User"/></returns>
        public async Task<User> LoadUser(string username)
        {
            SQLiteCommand cmd = new SQLiteCommand { CommandText = "SELECT * FROM Users WHERE [Username] = @name" };
            cmd.Parameters.AddWithValue("@name", username);
            DataSet ds = await SQLite.FillDataSet(_con, cmd);
            User user = new User();
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                user = new User
                {
                    Name = dr["Username"].ToString(),
                    Password = dr["Password"].ToString(),
                    Level = Int32Helper.Parse(dr["Level"]),
                    Experience = Int32Helper.Parse(dr["Experience"]),
                    SkillPoints = Int32Helper.Parse(dr["SkillPoints"]),
                    Alive = BoolHelper.Parse(dr["Alive"]),
                    CurrentLocation = dr["Location"].ToString(),
                    CurrentEndurance = Int32Helper.Parse(dr["CurrentEndurance"]),
                    MaximumEndurance = Int32Helper.Parse(dr["MaximumEndurance"]),
                    Hunger = Int32Helper.Parse(dr["Hunger"]),
                    Thirst = Int32Helper.Parse(dr["Thirst"]),
                    CurrentWeapon = EnumHelper.Parse<WeaponType>(dr["CurrentWeapon"].ToString()),
                    LightWeapon = GameState.AllWeapons.Find(weapon => weapon.Name == dr["LightWeapon"].ToString()),
                    HeavyWeapon = GameState.AllWeapons.Find(weapon => weapon.Name == dr["HeavyWeapon"].ToString()),
                    TwoHandedWeapon = GameState.AllWeapons.Find(weapon => weapon.Name == dr["TwoHandedWeapon"].ToString()),
                    Armor = GameState.AllArmor.Find(armor => armor.Name == dr["Armor"].ToString()),
                    Potion = GameState.AllPotions.Find(potion => potion.Name == dr["Potion"].ToString()),
                    Lockpicks = Int32Helper.Parse(dr["Lockpicks"]),
                    GoldOnHand = Int32Helper.Parse(dr["GoldOnHand"]),
                    GoldInBank = Int32Helper.Parse(dr["GoldInBank"]),
                    GoldOnLoan = Int32Helper.Parse(dr["GoldOnLoan"]),
                    Shovel = BoolHelper.Parse(dr["Shovel"]),
                    Lantern = BoolHelper.Parse(dr["Lantern"]),
                    Amulet = BoolHelper.Parse(dr["Amulet"]),
                    LightWeaponSkill = Int32Helper.Parse(dr["LightWeaponSkill"]),
                    HeavyWeaponSkill = Int32Helper.Parse(dr["HeavyWeaponSkill"]),
                    TwoHandedWeaponSkill = Int32Helper.Parse(dr["TwoHandedWeaponSkill"]),
                    Blocking = Int32Helper.Parse(dr["Blocking"]),
                    Slipping = Int32Helper.Parse(dr["Slipping"]),
                    Stealth = Int32Helper.Parse(dr["Stealth"]),
                    HenchmenLevel1 = Int32Helper.Parse(dr["HenchmenLevel1"]),
                    HenchmenLevel2 = Int32Helper.Parse(dr["HenchmenLevel2"]),
                    HenchmenLevel3 = Int32Helper.Parse(dr["HenchmenLevel3"]),
                    HenchmenLevel4 = Int32Helper.Parse(dr["HenchmenLevel4"]),
                    HenchmenLevel5 = Int32Helper.Parse(dr["HenchmenLevel5"])
                };
            }

            return user;
        }

        /// <summary>Loads all <see cref="User"/>s from the database.</summary>
        /// <returns>All <see cref="User"/>s</returns>
        public async Task<List<User>> LoadUsers()
        {
            DataSet ds = await SQLite.FillDataSet(_con, "SELECT * FROM Users");
            List<User> allUsers = new List<User>();
            if (ds.Tables[0].Rows.Count > 0)
            {
                allUsers.AddRange(
                    from DataRow dr in ds.Tables[0].Rows
                    select new User
                    {
                        Name = dr["Username"].ToString(),
                        Password = dr["Password"].ToString(),
                        Level = Int32Helper.Parse(dr["Level"]),
                        Experience = Int32Helper.Parse(dr["Experience"]),
                        SkillPoints = Int32Helper.Parse(dr["SkillPoints"]),
                        Alive = BoolHelper.Parse(dr["Alive"]),
                        CurrentLocation = dr["Location"].ToString(),
                        CurrentEndurance = Int32Helper.Parse(dr["CurrentEndurance"]),
                        MaximumEndurance = Int32Helper.Parse(dr["MaximumEndurance"]),
                        Hunger = Int32Helper.Parse(dr["Hunger"]),
                        Thirst = Int32Helper.Parse(dr["Thirst"]),
                        CurrentWeapon = EnumHelper.Parse<WeaponType>(dr["CurrentWeapon"].ToString()),
                        LightWeapon = GameState.AllWeapons.Find(weapon => weapon.Name == dr["LightWeapon"].ToString()),
                        HeavyWeapon = GameState.AllWeapons.Find(weapon => weapon.Name == dr["HeavyWeapon"].ToString()),
                        TwoHandedWeapon = GameState.AllWeapons.Find(weapon => weapon.Name == dr["TwoHandedWeapon"].ToString()),
                        Armor = GameState.AllArmor.Find(armor => armor.Name == dr["Armor"].ToString()),
                        Potion = GameState.AllPotions.Find(potion => potion.Name == dr["Potion"].ToString()),
                        Lockpicks = Int32Helper.Parse(dr["Lockpicks"]),
                        GoldOnHand = Int32Helper.Parse(dr["GoldOnHand"]),
                        GoldInBank = Int32Helper.Parse(dr["GoldInBank"]),
                        GoldOnLoan = Int32Helper.Parse(dr["GoldOnLoan"]),
                        Shovel = BoolHelper.Parse(dr["Shovel"]),
                        Lantern = BoolHelper.Parse(dr["Lantern"]),
                        Amulet = BoolHelper.Parse(dr["Amulet"]),
                        LightWeaponSkill = Int32Helper.Parse(dr["LightWeaponSkill"]),
                        HeavyWeaponSkill = Int32Helper.Parse(dr["HeavyWeaponSkill"]),
                        TwoHandedWeaponSkill = Int32Helper.Parse(dr["TwoHandedWeaponSkill"]),
                        Blocking = Int32Helper.Parse(dr["Blocking"]),
                        Slipping = Int32Helper.Parse(dr["Slipping"]),
                        Stealth = Int32Helper.Parse(dr["Stealth"]),
                        HenchmenLevel1 = Int32Helper.Parse(dr["HenchmenLevel1"]),
                        HenchmenLevel2 = Int32Helper.Parse(dr["HenchmenLevel2"]),
                        HenchmenLevel3 = Int32Helper.Parse(dr["HenchmenLevel3"]),
                        HenchmenLevel4 = Int32Helper.Parse(dr["HenchmenLevel4"]),
                        HenchmenLevel5 = Int32Helper.Parse(dr["HenchmenLevel5"])
                    });
            }

            return allUsers.OrderBy(user => user.Name).ToList();
        }

        /// <summary>Loads all <see cref="Weapon"/>s from the database.</summary>
        /// <returns>All <see cref="Weapon"/>s</returns>
        public async Task<List<Weapon>> LoadWeapons()
        {
            DataSet ds = await SQLite.FillDataSet(_con, "SELECT * FROM Weapons");
            List<Weapon> allWeapons = new List<Weapon>();

            if (ds.Tables[0].Rows.Count > 0)
            {
                allWeapons.AddRange(from DataRow dr in ds.Tables[0].Rows select new Weapon(dr["WeaponName"].ToString(), EnumHelper.Parse<WeaponType>(dr["WeaponType"].ToString()), Int32Helper.Parse(dr["WeaponDamage"]), Int32Helper.Parse(dr["WeaponValue"]), BoolHelper.Parse(dr["Hidden"])));
            }

            return allWeapons.OrderBy(weapon => weapon.Value).ToList();
        }

        #endregion Load

        #region User Management

        /// <summary>Changes a <see cref="User"/>'s details in the database.</summary>
        /// <param name="oldUser"><see cref="User"/> to be updated</param>
        /// <param name="newUser"><see cref="User"/> with new details</param>
        /// <returns>True if successful</returns>
        public async Task<bool> ChangeUserDetails(User oldUser, User newUser)
        {
            SQLiteCommand cmd = new SQLiteCommand
            {
                CommandText =
                    "UPDATE Users SET [Username] = @name, [Password] = @password WHERE [Username] = @oldName"
            };

            cmd.Parameters.AddWithValue("@name", newUser.Name);
            cmd.Parameters.AddWithValue("@password", newUser.Password);
            cmd.Parameters.AddWithValue("@oldName", oldUser.Name);

            return await SQLite.ExecuteCommand(_con, cmd);
        }

        /// <summary>Adds a new <see cref="User"/> to the database.</summary>
        /// <param name="newUser"><see cref="User"/> to be added</param>
        /// <returns>True if successful</returns>
        public async Task<bool> NewUser(User newUser)
        {
            SQLiteCommand cmd = new SQLiteCommand
            {
                CommandText =
                    "INSERT INTO Users([Username], [Password], [Level], [Experience], [SkillPoints], [Alive], [Location], [CurrentEndurance], [MaximumEndurance], [Hunger], [Thirst], [CurrentWeapon], [LightWeapon], [HeavyWeapon], [TwoHandedWeapon], [Armor], [Potion], [Lockpicks], [GoldOnHand], [GoldInBank], [GoldOnLoan], [Shovel], [Lantern], [Amulet], [LightWeaponSkill], [HeavyWeaponSkill], [TwoHandedWeaponSkill], [Blocking], [Slipping], [Stealth], [HenchmenLevel1], [HenchmenLevel2], [HenchmenLevel3], [HenchmenLevel4], [HenchmenLevel5])VALUES(@name, @password, @level, @experience, @skillPoints, @alive, @location, @currentEndurance, @maximumEndurance, @hunger, @thirst, @currentWeapon, @lightWeapon, @heavyWeapon, @twoHandedWeapon, @armor, @potion, @lockpicks, @goldOnHand, @goldInBank, @goldOnLoan, @shovel, @lantern, @amulet, @lightWeaponSkill, @heavyWeaponSkill, @twoHandedWeaponSkill, @blocking, @slipping, @stealth, @henchmenLevel1, @henchmenLevel2, @henchmenLevel3, @henchmenLevel4, @henchmenLevel5)"
            };

            cmd.Parameters.AddWithValue("@name", newUser.Name);
            cmd.Parameters.AddWithValue("@password", newUser.Password);
            cmd.Parameters.AddWithValue("@level", newUser.Level);
            cmd.Parameters.AddWithValue("@experience", newUser.Experience.ToString());
            cmd.Parameters.AddWithValue("@skillPoints", newUser.SkillPoints.ToString());
            cmd.Parameters.AddWithValue("@alive", Int32Helper.Parse(newUser.Alive));
            cmd.Parameters.AddWithValue("@location", newUser.CurrentLocation);
            cmd.Parameters.AddWithValue("@currentEndurance", newUser.CurrentEndurance.ToString());
            cmd.Parameters.AddWithValue("@maximumEndurance", newUser.MaximumEndurance.ToString());
            cmd.Parameters.AddWithValue("@hunger", newUser.Hunger.ToString());
            cmd.Parameters.AddWithValue("@thirst", newUser.Thirst.ToString());
            cmd.Parameters.AddWithValue("@currentWeapon", newUser.CurrentWeapon);
            cmd.Parameters.AddWithValue("@lightWeapon", newUser.LightWeapon.Name);
            cmd.Parameters.AddWithValue("@heavyWeapon", newUser.HeavyWeapon.Name);
            cmd.Parameters.AddWithValue("@twoHandedWeapon", newUser.TwoHandedWeapon.Name);
            cmd.Parameters.AddWithValue("@armor", newUser.Armor.Name);
            cmd.Parameters.AddWithValue("@potion", newUser.Potion.Name);
            cmd.Parameters.AddWithValue("@lockpicks", newUser.Lockpicks.ToString());
            cmd.Parameters.AddWithValue("@goldOnHand", newUser.GoldOnHand.ToString());
            cmd.Parameters.AddWithValue("@goldInBank", newUser.GoldInBank.ToString());
            cmd.Parameters.AddWithValue("@goldOnLoan", newUser.GoldOnLoan.ToString());
            cmd.Parameters.AddWithValue("@shovel", Int32Helper.Parse(newUser.Shovel));
            cmd.Parameters.AddWithValue("@lantern", Int32Helper.Parse(newUser.Lantern));
            cmd.Parameters.AddWithValue("@amulet", Int32Helper.Parse(newUser.Amulet));
            cmd.Parameters.AddWithValue("@lightWeaponSkill", newUser.LightWeaponSkill.ToString());
            cmd.Parameters.AddWithValue("@heavyWeaponSkill", newUser.HeavyWeaponSkill.ToString());
            cmd.Parameters.AddWithValue("@twoHandedWeaponSkill", newUser.TwoHandedWeaponSkill.ToString());
            cmd.Parameters.AddWithValue("@blocking", newUser.Blocking.ToString());
            cmd.Parameters.AddWithValue("@slipping", newUser.Slipping.ToString());
            cmd.Parameters.AddWithValue("@stealth", newUser.Stealth.ToString());
            cmd.Parameters.AddWithValue("@henchmenLevel1", newUser.HenchmenLevel1.ToString());
            cmd.Parameters.AddWithValue("@henchmenLevel2", newUser.HenchmenLevel2.ToString());
            cmd.Parameters.AddWithValue("@henchmenLevel3", newUser.HenchmenLevel3.ToString());
            cmd.Parameters.AddWithValue("@henchmenLevel4", newUser.HenchmenLevel4.ToString());
            cmd.Parameters.AddWithValue("@henchmenLevel5", newUser.HenchmenLevel5.ToString());

            return await SQLite.ExecuteCommand(_con, cmd);
        }

        /// <summary>Saves the current <see cref="User"/>.</summary>
        /// <param name="saveUser"><see cref="User"/> to be saved.</param>
        /// <returns>True if successful</returns>
        public async Task<bool> SaveUser(User saveUser)
        {
            SQLiteCommand cmd = new SQLiteCommand { CommandText = "UPDATE Users SET [Level] = @level, [Experience] = @experience, [SkillPoints] = @skillPoints, [Alive] = @alive, [Location] = location, [CurrentEndurance] = @currentEndurance, [MaximumEndurance] = @maximumEndurance, [Hunger] = @hunger, [Thirst] = @thirst, [CurrentWeapon] = @currentWeapon, [LightWeapon] = @lightWeapon, [HeavyWeapon] = @heavyWeapon, [TwoHandedWeapon] = @twoHandedWeapon, [Armor] = @armor, [Potion] = @potion, [Lockpicks] = @lockpicks, [GoldOnHand] = @goldOnHand, [GoldInBank] = @goldInBank, [GoldOnLoan] = @goldOnLoan, [Shovel] = @shovel, [Lantern] = @lantern, [Amulet] = @amulet, [LightWeaponSkill] = @lightWeaponSkill, [HeavyWeaponSkill] = @heavyWeaponSkill, [TwoHandedWeaponSkill] = @twoHandedWeaponSkill, [Blocking] = @blocking, [Slipping] = @slipping, [Stealth] = @stealth, [HenchmenLevel1] = @henchmenLevel1, [HenchmenLevel2] = @henchmenLevel2, [HenchmenLevel3] = @henchmenLevel3, [HenchmenLevel4] = @henchmenLevel4, [HenchmenLevel5] = @henchmenLevel5 WHERE [Username] = @name" };

            cmd.Parameters.AddWithValue("@level", saveUser.Level);
            cmd.Parameters.AddWithValue("@experience", saveUser.Experience.ToString());
            cmd.Parameters.AddWithValue("@skillPoints", saveUser.SkillPoints.ToString());
            cmd.Parameters.AddWithValue("@alive", Int32Helper.Parse(saveUser.Alive));
            cmd.Parameters.AddWithValue("@location", saveUser.CurrentLocation);
            cmd.Parameters.AddWithValue("@currentEndurance", saveUser.CurrentEndurance.ToString());
            cmd.Parameters.AddWithValue("@maximumEndurance", saveUser.MaximumEndurance.ToString());
            cmd.Parameters.AddWithValue("@hunger", saveUser.Hunger.ToString());
            cmd.Parameters.AddWithValue("@thirst", saveUser.Thirst.ToString());
            cmd.Parameters.AddWithValue("@currentWeapon", saveUser.CurrentWeapon);
            cmd.Parameters.AddWithValue("@lightWeapon", saveUser.LightWeapon.Name);
            cmd.Parameters.AddWithValue("@heavyWeapon", saveUser.HeavyWeapon.Name);
            cmd.Parameters.AddWithValue("@twoHandedWeapon", saveUser.TwoHandedWeapon.Name);
            cmd.Parameters.AddWithValue("@armor", saveUser.Armor.Name);
            cmd.Parameters.AddWithValue("@potion", saveUser.Potion.Name);
            cmd.Parameters.AddWithValue("@lockpicks", saveUser.Lockpicks.ToString());
            cmd.Parameters.AddWithValue("@goldOnHand", saveUser.GoldOnHand.ToString());
            cmd.Parameters.AddWithValue("@goldInBank", saveUser.GoldInBank.ToString());
            cmd.Parameters.AddWithValue("@goldOnLoan", saveUser.GoldOnLoan.ToString());
            cmd.Parameters.AddWithValue("@shovel", Int32Helper.Parse(saveUser.Shovel));
            cmd.Parameters.AddWithValue("@lantern", Int32Helper.Parse(saveUser.Lantern));
            cmd.Parameters.AddWithValue("@amulet", Int32Helper.Parse(saveUser.Amulet));
            cmd.Parameters.AddWithValue("@lightWeaponSkill", saveUser.LightWeaponSkill.ToString());
            cmd.Parameters.AddWithValue("@heavyWeaponSkill", saveUser.HeavyWeaponSkill.ToString());
            cmd.Parameters.AddWithValue("@twoHandedWeaponSkill", saveUser.TwoHandedWeaponSkill.ToString());
            cmd.Parameters.AddWithValue("@blocking", saveUser.Blocking.ToString());
            cmd.Parameters.AddWithValue("@slipping", saveUser.Slipping.ToString());
            cmd.Parameters.AddWithValue("@stealth", saveUser.Stealth.ToString());
            cmd.Parameters.AddWithValue("@henchmenLevel1", saveUser.HenchmenLevel1.ToString());
            cmd.Parameters.AddWithValue("@henchmenLevel2", saveUser.HenchmenLevel2.ToString());
            cmd.Parameters.AddWithValue("@henchmenLevel3", saveUser.HenchmenLevel3.ToString());
            cmd.Parameters.AddWithValue("@henchmenLevel4", saveUser.HenchmenLevel4.ToString());
            cmd.Parameters.AddWithValue("@henchmenLevel5", saveUser.HenchmenLevel5.ToString());
            cmd.Parameters.AddWithValue("@name", saveUser.Name);

            return await SQLite.ExecuteCommand(_con, cmd);
        }

        #endregion User Management
    }
}