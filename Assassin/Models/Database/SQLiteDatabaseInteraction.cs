using Assassin.Models.Entities;
using Assassin.Models.Enums;
using Assassin.Models.Items;
using Extensions;
using Extensions.DatabaseHelp;
using Extensions.DataTypeHelpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Assassin.Models.Database
{
    /// <summary><summary>Represents all SQLite database interactions required by the game.</summary>
    internal class SQLiteDatabaseInteraction : IDatabaseInteraction
    {
        private const string _DATABASENAME = "Assassin.sqlite";
        private static readonly string DatabaseLocation = Path.Combine(AppData.Location, _DATABASENAME);
        private readonly string _con = $"Data Source = {DatabaseLocation}; foreign keys = TRUE; Version=3";

        /// <summary>Verifies that the requested database exists and that its file size is greater than zero. If not, it extracts the embedded database file to the local output folder.</summary>
        public void VerifyDatabaseIntegrity() => Functions.VerifyFileIntegrity(Assembly.GetExecutingAssembly().GetManifestResourceStream($"Assassin.{_DATABASENAME}"), _DATABASENAME, AppData.Location);

        /// <summary>Changes the admin password in the database.</summary>
        /// <param name="newPass">New password</param>
        /// <returns>True if successful</returns>
        public async Task<bool> ChangeAdminPassword(string newPass)
        {
            SQLiteCommand cmd = new SQLiteCommand { CommandText = "UPDATE Admin SET AdminPassword = @newPass" };
            cmd.Parameters.AddWithValue("@newPass", newPass);
            return await SQLiteHelper.ExecuteCommand(_con, cmd);
        }

        #region Enemy Management

        /// <summary>Deletes an <see cref="Enemy"/> from the database.</summary>
        /// <param name="enemyDelete"><see cref="Enemy"/> to be deleted from the database</param>
        /// <returns>True if successful</returns>
        public async Task<bool> DeleteEnemy(Enemy enemyDelete)
        {
            SQLiteCommand cmd = new SQLiteCommand { CommandText = "DELETE FROM Enemies WHERE [EnemyName] = @name" };
            cmd.Parameters.AddWithValue("@name", enemyDelete.Name);
            return await SQLiteHelper.ExecuteCommand(_con, cmd);
        }

        /// <summary>Adds a new <see cref="Enemy"/> to the database.</summary>
        /// <param name="enemyNew"><see cref="Enemy"/> to be added to the database</param>
        /// <returns>True if successful</returns>
        public async Task<bool> NewEnemy(Enemy enemyNew)
        {
            SQLiteCommand cmd = new SQLiteCommand { CommandText = "INSERT INTO Enemies([EnemyName], [Level], [Endurance], [Weapon], [Armor], [Gold], [WeaponSkill], [Blocking], [Slipping])VALUES(@name, @level, @endurance, @weapon, @armor, @gold, @weaponSkill, @blocking, @slipping)" };

            cmd.Parameters.AddWithValue("@name", enemyNew.Name);
            cmd.Parameters.AddWithValue("@level", enemyNew.Level);
            cmd.Parameters.AddWithValue("@endurance", enemyNew.MaximumEndurance);
            cmd.Parameters.AddWithValue("@weapon", enemyNew.Weapon.Name);
            cmd.Parameters.AddWithValue("@armor", enemyNew.Armor.Name);
            cmd.Parameters.AddWithValue("@gold", enemyNew.GoldOnHand);
            cmd.Parameters.AddWithValue("@weaponSkill", enemyNew.WeaponSkill);
            cmd.Parameters.AddWithValue("@blocking", enemyNew.Blocking);
            cmd.Parameters.AddWithValue("@slipping", enemyNew.Slipping);

            return await SQLiteHelper.ExecuteCommand(_con, cmd);
        }

        /// <summary>Saves an <see cref="Enemy"/> to the database.</summary>
        /// <param name="oldEnemy">Enemy to be replaced</param>
        /// <param name="newEnemy">Enemy to be saved</param>
        /// <returns>True if successful</returns>
        public async Task<bool> SaveEnemy(Enemy newEnemy, Enemy oldEnemy)
        {
            SQLiteCommand cmd = new SQLiteCommand { CommandText = "UPDATE Enemies SET [EnemyName] = @name, [Level] = @level, [Endurance] = @endurance, [Weapon] = @weapon, [Armor] = @armor, [Gold] = @gold, [WeaponSkill] = @weaponSkill, [Blocking] = @blocking, [Slipping] = @slipping WHERE [EnemyName] = @oldName" };

            cmd.Parameters.AddWithValue("@name", newEnemy.Name);
            cmd.Parameters.AddWithValue("@level", newEnemy.Level);
            cmd.Parameters.AddWithValue("@endurance", newEnemy.MaximumEndurance);
            cmd.Parameters.AddWithValue("@weapon", newEnemy.Weapon.Name);
            cmd.Parameters.AddWithValue("@armor", newEnemy.Armor.Name);
            cmd.Parameters.AddWithValue("@gold", newEnemy.GoldOnHand);
            cmd.Parameters.AddWithValue("@weaponSkill", newEnemy.WeaponSkill);
            cmd.Parameters.AddWithValue("@blocking", newEnemy.Blocking);
            cmd.Parameters.AddWithValue("@slipping", newEnemy.Slipping);
            cmd.Parameters.AddWithValue("@oldName", oldEnemy.Name);

            return await SQLiteHelper.ExecuteCommand(_con, cmd);
        }

        #endregion Enemy Management

        #region Guild Management

        /// <summary><see cref="User"/> applies for membership with a <see cref="Guild"/>.</summary>
        /// <param name="joinUser"><see cref="User"/> applying to join the <see cref="Guild"/>.</param>
        /// <param name="joinGuild"><see cref="Guild"/> being applied to</param>
        /// <returns>True if successful</returns>
        public async Task<bool> ApplyToGuild(User joinUser, Guild joinGuild)
        {
            string guildID = $"Guild{joinGuild.ID}Members";
            SQLiteCommand cmd = new SQLiteCommand { CommandText = "INSERT INTO Applications([Username], [Guild])VALUES(@name, @guild)" };
            cmd.Parameters.AddWithValue("@name", joinUser.Name);
            cmd.Parameters.AddWithValue("@guild", joinGuild.ID);
            return await SQLiteHelper.ExecuteCommand(_con, cmd);
        }

        /// <summary><see cref="User"/> is approved for membership with a <see cref="Guild"/>.</summary>
        /// <param name="approveUser"><see cref="User"/> approved to join the <see cref="Guild"/>.</param>
        /// <param name="approveGuild"><see cref="Guild"/> being joined</param>
        /// <returns>True if successful</returns>
        public async Task<bool> ApproveGuildApplication(User approveUser, Guild approveGuild)
        {
            return await DeleteGuildApplication(approveUser, approveGuild) && await SendMessage(new Message(await SQLiteHelper.GetNextIndex(_con, "Messages"), approveGuild.Name, approveUser.Name, $"Your application to join the {approveGuild.Name} guild has been approved. Welcome!", DateTime.UtcNow, true)) && await MemberJoinsGuild(approveUser, approveGuild);
        }

        /// <summary>Deletes a <see cref="User"/>'s application to a<see cref= "Guild" />.</ summary >
        /// <param name="deleteUser"><see cref="User"/> whose application is deleted</param>
        /// <param name="deleteGuild"><see cref="Guild"/> from which the <see cref="User"/>'s application was deleted</param>
        /// <returns>True if successful</returns>
        public async Task<bool> DeleteGuildApplication(User deleteUser, Guild deleteGuild)
        {
            SQLiteCommand cmd = new SQLiteCommand { CommandText = "DELETE FROM Applications WHERE [Username] = @name AND [Guild] = @guild" };
            cmd.Parameters.AddWithValue("@name", deleteUser.Name);
            cmd.Parameters.AddWithValue("@guild", deleteGuild.ID);
            return await SQLiteHelper.ExecuteCommand(_con, cmd);
        }

        /// <summary>Denies a <see cref="User"/>'s application to a<see cref= "Guild" />.</ summary >
        /// <param name="denyUser"><see cref="User"/> whose application is denied</param>
        /// <param name="denyGuild"><see cref="Guild"/> from which the <see cref="User"/>'s application was denied</param>
        /// <returns>True if successful</returns>
        public async Task<bool> DenyGuildApplication(User denyUser, Guild denyGuild)
        {
            return await DeleteGuildApplication(denyUser, denyGuild) && await SendMessage(new Message(await SQLiteHelper.GetNextIndex(_con, "Messages"), denyGuild.Name, denyUser.Name, $"Your application to join the {denyGuild.Name} guild has been denied.", DateTime.UtcNow, true));
        }

        /// <summary>Checks whether the <see cref="User"/> has applied to the selected <see cref="Guild"/>.</summary>
        /// <param name="checkUser"><see cref="User"/> to check if has applied to the <see cref="Guild"/>.</param>
        /// <param name="checkGuild"><see cref="Guild"/> being joined</param>
        /// <returns>True if has applied</returns>
        public async Task<bool> HasAppliedToGuild(User checkUser, Guild checkGuild)
        {
            SQLiteCommand cmd = new SQLiteCommand { CommandText = "SELECT * FROM Applications WHERE [Username] = @name AND [Guild] = @guild" };
            cmd.Parameters.AddWithValue("@name", checkUser.Name);
            cmd.Parameters.AddWithValue("@guild", checkGuild.ID);
            DataSet ds = await SQLiteHelper.FillDataSet(_con, cmd);
            return ds.Tables[0].Rows.Count > 0;
        }

        /// <summary>Member of a <see cref="Guild"/> gains membership with that <see cref="Guild"/>, applied to database.</summary>
        /// <param name="joinUser"><see cref="User"/> joining the <see cref="Guild"/>.</param>
        /// <param name="joinGuild"><see cref="Guild"/> being joined</param>
        /// <returns>True if successful</returns>
        public async Task<bool> MemberJoinsGuild(User joinUser, Guild joinGuild)
        {
            string guildID = $"Guild{joinGuild.ID}Members";
            SQLiteCommand cmd = new SQLiteCommand { CommandText = $"INSERT INTO {guildID}([Username])VALUES(@name)" };
            cmd.Parameters.AddWithValue("@name", joinUser.Name);
            return await SQLiteHelper.ExecuteCommand(_con, cmd);
        }

        /// <summary>Member of a <see cref="Guild"/> terminates membership with that <see cref="Guild"/>, applied to database.</summary>
        /// <param name="leaveUser"><see cref="User"/> leaving the <see cref="Guild"/>.</param>
        /// <param name="leaveGuild"><see cref="Guild"/> being left</param>
        /// <returns>True if successful</returns>
        public async Task<bool> MemberLeavesGuild(User leaveUser, Guild leaveGuild)
        {
            string guildID = $"Guild{leaveGuild.ID}Members";
            SQLiteCommand cmd = new SQLiteCommand { CommandText = $"DELETE FROM {guildID} WHERE [Username] = @name" };
            cmd.Parameters.AddWithValue("@name", leaveUser.Name);
            return await SQLiteHelper.ExecuteCommand(_con, cmd);
        }

        /// <summary>Saves a <see cref="Guild"/>.</summary>
        /// <param name="guildSave"><see cref="Guild"/> to be saved</param>
        public async Task<bool> SaveGuild(Guild guildSave)
        {
            SQLiteCommand cmd = new SQLiteCommand { CommandText = "UPDATE Guilds SET [GuildName] = @guildName, [Guildmaster] = @guildmaster, [GuildFee] = @guildFee, [GuildGold] = @guildGold, [HenchmenLevel1] = @henchmenLevel1, [HenchmenLevel2] = @henchmenLevel2, [HenchmenLevel3] = @henchmenLevel3, [HenchmenLevel4] = @henchmenLevel4, [HenchmenLevel5] = @henchmenLevel5 WHERE [ID] = @id" };

            cmd.Parameters.AddWithValue("@guildName", guildSave.Name);
            cmd.Parameters.AddWithValue("@guildmaster", guildSave.Master);
            cmd.Parameters.AddWithValue("@guildFee", guildSave.Fee);
            cmd.Parameters.AddWithValue("@guildGold", guildSave.Gold);
            cmd.Parameters.AddWithValue("@henchmenLevel1", guildSave.Henchmen.Level1.ToString());
            cmd.Parameters.AddWithValue("@henchmenLevel2", guildSave.Henchmen.Level2.ToString());
            cmd.Parameters.AddWithValue("@henchmenLevel3", guildSave.Henchmen.Level3.ToString());
            cmd.Parameters.AddWithValue("@henchmenLevel4", guildSave.Henchmen.Level4.ToString());
            cmd.Parameters.AddWithValue("@henchmenLevel5", guildSave.Henchmen.Level5.ToString());
            cmd.Parameters.AddWithValue("@id", guildSave.ID);

            return await SQLiteHelper.ExecuteCommand(_con, cmd);
        }

        #endregion Guild Management

        #region Jail Management

        /// <summary>Frees a <see cref="JailedUser"/> from Jail.</summary>
        /// <param name="jailUser"><see cref="JailedUser"/> to be freed</param>
        /// <returns>True if successful</returns>
        public async Task<bool> FreeFromJail(JailedUser jailUser)
        {
            SQLiteCommand cmd = new SQLiteCommand { CommandText = "DELETE FROM Jail WHERE [Username] = @name" };
            cmd.Parameters.AddWithValue("@name", jailUser.Name);
            return await SQLiteHelper.ExecuteCommand(_con, cmd);
        }

        /// <summary>Sends a <see cref="JailedUser"/> to Jail.</summary>
        /// <param name="jailUser"><see cref="JailedUser"/> to be jailed</param>
        /// <returns>True if successful</returns>
        public async Task<bool> SendToJail(JailedUser jailUser)
        {
            SQLiteCommand cmd = new SQLiteCommand { CommandText = "INSERT INTO Jail([Username], [Reason], [Fine], [DateJailed])VALUES(@name, @reason, @fine, @dateJailed)" };
            cmd.Parameters.AddWithValue("@name", jailUser.Name);
            cmd.Parameters.AddWithValue("@reason", jailUser.Reason);
            cmd.Parameters.AddWithValue("@fine", jailUser.Fine);
            cmd.Parameters.AddWithValue("@dateJailed", jailUser.DateJailed);
            return await SQLiteHelper.ExecuteCommand(_con, cmd);
        }

        #endregion Jail Management

        #region Load

        /// <summary>Loads the administrator password from the database.</summary>
        /// <returns>Admin password</returns>
        public async Task<string> LoadAdminPassword()
        {
            DataSet ds = await SQLiteHelper.FillDataSet(_con, "SELECT * FROM Admin");
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
            DataSet ds = await SQLiteHelper.FillDataSet(_con, "SELECT * FROM Armor");
            List<Armor> allArmor = new List<Armor>();
            if (ds.Tables[0].Rows.Count > 0)
                allArmor.AddRange(from DataRow dr in ds.Tables[0].Rows select new Armor(dr["ArmorName"].ToString(), Int32Helper.Parse(dr["ArmorDefense"]), Int32Helper.Parse(dr["ArmorValue"]), BoolHelper.Parse(dr["Hidden"])));

            return allArmor.OrderBy(armor => armor.Value).ToList();
        }

        /// <summary>Loads all <see cref="Drink"/>s from the database.</summary>
        /// <returns>All <see cref="Drink"/>s</returns>
        public async Task<List<Drink>> LoadDrinks()
        {
            DataSet ds = await SQLiteHelper.FillDataSet(_con, "SELECT * FROM Drinks");
            List<Drink> allDrinks = new List<Drink>();
            if (ds.Tables[0].Rows.Count > 0)
                allDrinks.AddRange(from DataRow dr in ds.Tables[0].Rows select new Drink(new Drink(dr["DrinkName"].ToString(), Int32Helper.Parse(dr["RestoreThirst"]), Int32Helper.Parse(dr["DrinkValue"]))));

            return allDrinks.OrderBy(drink => drink.Value).ToList();
        }

        /// <summary>Loads all <see cref="Enemy"/> from the database.</summary>
        /// <returns>All <see cref="Enemy"/></returns>
        public async Task<List<Enemy>> LoadEnemies()
        {
            DataSet ds = await SQLiteHelper.FillDataSet(_con, "SELECT * FROM Enemies");
            List<Enemy> allEnemies = new List<Enemy>();
            if (ds.Tables[0].Rows.Count > 0)
                allEnemies.AddRange(from DataRow dr in ds.Tables[0].Rows select new Enemy(dr["EnemyName"].ToString(), Int32Helper.Parse(dr["Level"]), Int32Helper.Parse(dr["Endurance"]), Int32Helper.Parse(dr["Endurance"]), new Weapon(GameState.AllWeapons.Find(wpn => wpn.Name == dr["Weapon"].ToString())), new Armor(GameState.AllArmor.Find(armr => armr.Name == dr["Armor"].ToString())), Int32Helper.Parse(dr["Gold"]), Int32Helper.Parse(dr["WeaponSkill"]), Int32Helper.Parse(dr["Blocking"]), Int32Helper.Parse(dr["Slipping"])));

            return allEnemies.OrderBy(enemy => enemy.Level).ToList();
        }

        /// <summary>Loads all <see cref="Food"/> from the database.</summary>
        /// <returns>All <see cref="Food"/></returns>
        public async Task<List<Food>> LoadFood()
        {
            DataSet ds = await SQLiteHelper.FillDataSet(_con, "SELECT * FROM Food");
            List<Food> allFood = new List<Food>();
            if (ds.Tables[0].Rows.Count > 0)
                allFood.AddRange(from DataRow dr in ds.Tables[0].Rows select new Food(new Food(dr["FoodName"].ToString(), Int32Helper.Parse(dr["RestoreHunger"]), Int32Helper.Parse(dr["FoodValue"]))));

            return allFood.OrderBy(food => food.Value).ToList();
        }

        /// <summary>Loads all applicants to a specific <see cref="Guild"/>.</summary>
        /// <param name="loadGuild"><see cref="Guild"/> whose applicants are to be loaded</param>
        /// <returns>List of applicant names</returns>
        public async Task<List<string>> LoadGuildApplicants(Guild loadGuild)
        {
            DataSet ds = await SQLiteHelper.FillDataSet(_con, $"SELECT * FROM Applications WHERE [Guild] = {loadGuild.ID}");
            List<string> allApplicants = new List<string>();

            if (ds.Tables[0].Rows.Count > 0)
            {
                allApplicants.AddRange(from DataRow dr in ds.Tables[0].Rows
                                       select dr["Username"].ToString());
            }
            return allApplicants;
        }

        /// <summary>Loads all <see cref="Guild"/>s from the database.</summary>
        /// <returns>All <see cref="Guild"/>s</returns>
        public async Task<List<Guild>> LoadGuilds()
        {
            DataSet ds = await SQLiteHelper.FillDataSet(_con, "SELECT * FROM Guilds");
            List<Guild> allGuilds = new List<Guild>();

            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    Guild newGuild = new Guild(Int32Helper.Parse(dr["ID"]), dr["GuildName"].ToString(), dr["Guildmaster"].ToString(), Int32Helper.Parse(dr["GuildFee"]), Int32Helper.Parse(dr["GuildGold"]), new List<string>(), new Henchmen(Int32Helper.Parse(dr["HenchmenLevel1"]), Int32Helper.Parse(dr["HenchmenLevel2"]), Int32Helper.Parse(dr["HenchmenLevel3"]), Int32Helper.Parse(dr["HenchmenLevel4"]), Int32Helper.Parse(dr["HenchmenLevel5"])));
                    string members = $"Guild{Int32Helper.Parse(dr["ID"])}Members";
                    DataSet membersDS = await SQLiteHelper.FillDataSet(_con, $"SELECT * FROM {members}");
                    if (membersDS.Tables[0].Rows.Count > 0)
                        foreach (DataRow drM in membersDS.Tables[0].Rows)
                            newGuild.Members.Add(drM["Username"].ToString());
                    allGuilds.Add(newGuild);
                }
            }
            return allGuilds.OrderBy(guild => guild.ID).ToList();
        }

        /// <summary>Loads all <see cref="JailedUser"/>s.</summary>
        ///<returns>All <see cref="JailedUser"/>s</returns>
        public async Task<List<JailedUser>> LoadJailedUsers()
        {
            List<JailedUser> jailedUsers = new List<JailedUser>();
            DataSet ds = await SQLiteHelper.FillDataSet(_con, "SELECT * FROM Jail");

            if (ds.Tables[0].Rows.Count > 0)
                foreach (DataRow dr in ds.Tables[0].Rows)
                    jailedUsers.Add(new JailedUser(dr["Username"].ToString(), EnumHelper.Parse<Crime>(dr["Reason"].ToString()), Int32Helper.Parse(dr["Fine"]), DateTimeHelper.Parse(dr["DateJailed"])));

            return jailedUsers;
        }

        /// <summary>Loads all <see cref="Message"/>s for specified <see cref="User"/>.</summary>
        /// <param name="loadUser"><see cref="User"/> whose <see cref="Message"/>s are to be loaded</param>
        /// <returns>List of all <see cref="Message"/>s for the specified <see cref="User"/></returns>
        public async Task<List<Message>> LoadMessages(User loadUser)
        {
            List<Message> messages = new List<Message>();
            DataSet ds = await SQLiteHelper.FillDataSet(_con, "SELECT * FROM Messages WHERE UserTo={loadUser.Name}");

            if (ds.Tables[0].Rows.Count > 0)
                foreach (DataRow dr in ds.Tables[0].Rows)
                    messages.Add(new Message(Int32Helper.Parse(dr["ID"].ToString()), dr["UserFrom"].ToString(), loadUser.Name, dr["Message"].ToString(), DateTimeHelper.Parse(dr["DateSent"].ToString()), BoolHelper.Parse(dr["GuildMessage"])));
            return messages;
        }

        /// <summary>Loads all <see cref="Potion"/>s from the database.</summary>
        /// <returns>All <see cref="Potion"/>s</returns>
        public async Task<List<Potion>> LoadPotions()
        {
            DataSet ds = await SQLiteHelper.FillDataSet(_con, "SELECT * FROM Potions");
            List<Potion> allPotions = new List<Potion>();

            if (ds.Tables[0].Rows.Count > 0)
                allPotions.AddRange(from DataRow dr in ds.Tables[0].Rows select new Potion(dr["PotionName"].ToString(), Int32Helper.Parse(dr["PotionHeal"]), Int32Helper.Parse(dr["PotionValue"])));

            return allPotions.OrderBy(potion => potion.Value).ToList();
        }

        /// <summary>Loads all Ranks from the database.</summary>
        /// <returns>All Ranks</returns>
        public async Task<List<string>> LoadRanks()
        {
            DataSet ds = await SQLiteHelper.FillDataSet(_con, "SELECT * FROM Ranks");
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
            DataSet ds = await SQLiteHelper.FillDataSet(_con, cmd);
            User user = new User();
            if (ds.Tables[0].Rows.Count > 0)
                user = AssignUserFromDataRow(ds.Tables[0].Rows[0]);

            return user;
        }

        /// <summary>Loads all <see cref="User"/>s from the database.</summary>
        /// <returns>All <see cref="User"/>s</returns>
        public async Task<List<User>> LoadUsers()
        {
            DataSet ds = await SQLiteHelper.FillDataSet(_con, "SELECT * FROM Users");
            List<User> allUsers = new List<User>();
            if (ds.Tables[0].Rows.Count > 0)
                allUsers.AddRange(from DataRow dr in ds.Tables[0].Rows
                                  select AssignUserFromDataRow(dr));

            return allUsers.OrderBy(user => user.Name).ToList();
        }

        /// <summary>Loads all <see cref="Weapon"/>s from the database.</summary>
        /// <returns>All <see cref="Weapon"/>s</returns>
        public async Task<List<Weapon>> LoadWeapons()
        {
            DataSet ds = await SQLiteHelper.FillDataSet(_con, "SELECT * FROM Weapons");
            List<Weapon> allWeapons = new List<Weapon>();

            if (ds.Tables[0].Rows.Count > 0)
            {
                allWeapons.AddRange(from DataRow dr in ds.Tables[0].Rows select new Weapon(dr["WeaponName"].ToString(), EnumHelper.Parse<WeaponType>(dr["WeaponType"].ToString()), Int32Helper.Parse(dr["WeaponDamage"]), Int32Helper.Parse(dr["WeaponValue"]), BoolHelper.Parse(dr["Hidden"])));
            }

            return allWeapons.OrderBy(weapon => weapon.Value).ToList();
        }

        #endregion Load

        #region Message Management

        /// <summary>Deletes a <see cref="Message"/> from the database.</summary>
        /// <param name="message"><see cref="Message"/> to be deleted</param>
        /// <returns>True if successful</returns>
        public async Task<bool> DeleteMessage(Message message)
        {
            SQLiteCommand cmd = new SQLiteCommand { CommandText = "DELETE FROM Messages WHERE [ID] = @id" };
            cmd.Parameters.AddWithValue("@id", message.ID);
            return await SQLiteHelper.ExecuteCommand(_con, cmd);
        }

        /// <summary>Sends a <see cref="Message"/> between <see cref="User"/>s.</summary>
        /// <param name="message"><see cref="Message"/> sent</param>
        /// <returns>True if successful</returns>
        public async Task<bool> SendMessage(Message message)
        {
            SQLiteCommand cmd = new SQLiteCommand { CommandText = "INSERT INTO Messages([UserTo], [UserFrom], [Message], [DateSent], [GuildMessage])VALUES(@userTo, @userFrom, @message, @dateSent, @guildMessage)" };
            cmd.Parameters.AddWithValue("@userTo", message.UserTo);
            cmd.Parameters.AddWithValue("@userFrom", message.UserFrom);
            cmd.Parameters.AddWithValue("@message", message.Contents);
            cmd.Parameters.AddWithValue("@dateSent", message.DateSent);
            cmd.Parameters.AddWithValue("@guildMessage", Int32Helper.Parse(message.GuildMessage));
            return await SQLiteHelper.ExecuteCommand(_con, cmd);
        }

        #endregion Message Management

        #region User Management

        /// <summary>Assigns a <see cref="User"/> from a DataRow.</summary>
        /// <param name="dr">DataRow containing <see cref="User"/></param>
        /// <returns>Assigned <see cref="User"/></returns>
        private User AssignUserFromDataRow(DataRow dr)
        {
            User newUser = new User
            {
                Name = dr["Username"].ToString(),
                Password = dr["Password"].ToString(),
                Level = Int32Helper.Parse(dr["Level"].ToString()),
                Experience = Int32Helper.Parse(dr["Experience"].ToString()),
                SkillPoints = Int32Helper.Parse(dr["SkillPoints"].ToString()),
                Alive = BoolHelper.Parse(dr["Alive"]),
                CurrentEndurance = Int32Helper.Parse(dr["CurrentEndurance"].ToString()),
                CurrentLocation = EnumHelper.Parse<SleepLocation>(dr["Location"].ToString()),
                MaximumEndurance = Int32Helper.Parse(dr["MaximumEndurance"].ToString()),
                Hunger = Int32Helper.Parse(dr["Hunger"].ToString()),
                Thirst = Int32Helper.Parse(dr["Thirst"].ToString()),
                CurrentWeaponType = EnumHelper.Parse<WeaponType>(dr["CurrentWeapon"].ToString()),
                LightWeapon = GameState.AllWeapons.Find(newWeapon => newWeapon.Name == dr["LightWeapon"].ToString()),
                HeavyWeapon = GameState.AllWeapons.Find(newWeapon => newWeapon.Name == dr["HeavyWeapon"].ToString()),
                TwoHandedWeapon = GameState.AllWeapons.Find(newWeapon => newWeapon.Name == dr["TwoHandedWeapon"].ToString()),
                Armor = GameState.AllArmor.Find(newArmor => newArmor.Name == dr["Armor"].ToString()),
                Potion = GameState.AllPotions.Find(newPotion => newPotion.Name == dr["Potion"].ToString()),
                Lockpicks = Int32Helper.Parse(dr["Lockpicks"].ToString()),
                GoldOnHand = Int32Helper.Parse(dr["GoldOnHand"].ToString()),
                GoldInBank = Int32Helper.Parse(dr["GoldInBank"].ToString()),
                GoldOnLoan = Int32Helper.Parse(dr["GoldOnLoan"].ToString()),
                Shovel = BoolHelper.Parse(dr["Shovel"]),
                Lantern = BoolHelper.Parse(dr["Lantern"]),
                Amulet = BoolHelper.Parse(dr["Amulet"]),
                LightWeaponSkill = Int32Helper.Parse(dr["LightWeaponSkill"].ToString()),
                HeavyWeaponSkill = Int32Helper.Parse(dr["HeavyWeaponSkill"].ToString()),
                TwoHandedWeaponSkill = Int32Helper.Parse(dr["TwoHandedWeaponSkill"].ToString()),
                Blocking = Int32Helper.Parse(dr["Blocking"].ToString()),
                Slipping = Int32Helper.Parse(dr["Slipping"].ToString()),
                Stealth = Int32Helper.Parse(dr["Stealth"].ToString()),
                Henchmen = new Henchmen(Int32Helper.Parse(dr["HenchmenLevel1"].ToString()),
                Int32Helper.Parse(dr["HenchmenLevel2"].ToString()),
                Int32Helper.Parse(dr["HenchmenLevel3"].ToString()),
                Int32Helper.Parse(dr["HenchmenLevel4"].ToString()),
                Int32Helper.Parse(dr["HenchmenLevel5"].ToString()))
            };

            return newUser;
        }

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

            return await SQLiteHelper.ExecuteCommand(_con, cmd);
        }

        /// <summary>Deletes a <see cref="User"/> from the database.</summary>
        /// <param name="userDelete"><see cref="User"/> to be deleted.</param>
        /// <returns>True if successful</returns>
        public async Task<bool> DeleteUser(User userDelete)
        {
            SQLiteCommand cmd = new SQLiteCommand { CommandText = "DELETE FROM Users WHERE [Username] = @name" };
            cmd.Parameters.AddWithValue("@name", userDelete.Name);
            return await SQLiteHelper.ExecuteCommand(_con, cmd);
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
            cmd.Parameters.AddWithValue("@currentWeapon", newUser.CurrentWeaponType);
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
            cmd.Parameters.AddWithValue("@henchmenLevel1", newUser.Henchmen.Level1.ToString());
            cmd.Parameters.AddWithValue("@henchmenLevel2", newUser.Henchmen.Level2.ToString());
            cmd.Parameters.AddWithValue("@henchmenLevel3", newUser.Henchmen.Level3.ToString());
            cmd.Parameters.AddWithValue("@henchmenLevel4", newUser.Henchmen.Level4.ToString());
            cmd.Parameters.AddWithValue("@henchmenLevel5", newUser.Henchmen.Level5.ToString());

            return await SQLiteHelper.ExecuteCommand(_con, cmd);
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
            cmd.Parameters.AddWithValue("@currentWeapon", saveUser.CurrentWeaponType.ToString());
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
            cmd.Parameters.AddWithValue("@henchmenLevel1", saveUser.Henchmen.Level1.ToString());
            cmd.Parameters.AddWithValue("@henchmenLevel2", saveUser.Henchmen.Level2.ToString());
            cmd.Parameters.AddWithValue("@henchmenLevel3", saveUser.Henchmen.Level3.ToString());
            cmd.Parameters.AddWithValue("@henchmenLevel4", saveUser.Henchmen.Level4.ToString());
            cmd.Parameters.AddWithValue("@henchmenLevel5", saveUser.Henchmen.Level5.ToString());
            cmd.Parameters.AddWithValue("@name", saveUser.Name);

            return await SQLiteHelper.ExecuteCommand(_con, cmd);
        }

        /// <summary>Changes an <see cref="User"/>'s name and then saves the<see cref= "User" /> to the database.</summary>
        /// <param name="userSave"><see cref="User"/> to be saved</param>
        /// <param name="newName">New name for <see cref="User"/></param>
        /// <returns>True if successful</returns>
        public async Task<bool> SaveUser(User userSave, string newName)
        {
            if (userSave.Name != newName)
            {
                SQLiteCommand cmd = new SQLiteCommand { CommandText = "UPDATE Users SET [Username] = @newName WHERE [Username] = @oldName" };
                cmd.Parameters.AddWithValue("@newName", newName);
                cmd.Parameters.AddWithValue("@oldName", userSave.Name);
                if (await SQLiteHelper.ExecuteCommand(_con, cmd))
                {
                    userSave.Name = newName;
                    return await SaveUser(userSave);
                }
                return false;
            }
            return true;
        }

        #endregion User Management
    }
}