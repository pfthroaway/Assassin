using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Assassin
{
    internal static class GameState
    {
        private const string _DBPROVIDERANDSOURCE = "Data Source = Assassin.sqlite;Version=3;foreign keys=true;";

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

        internal async static void LoadAll()
        {
            SQLiteConnection con = new SQLiteConnection();
            SQLiteDataAdapter da = new SQLiteDataAdapter();
            con.ConnectionString = _DBPROVIDERANDSOURCE;

            await Task.Factory.StartNew(() =>
            {
                try
                {
                    string sql = "SELECT * FROM Armor";
                    string table = "LoadStuff";
                    DataSet ds = new DataSet();
                    da = new SQLiteDataAdapter(sql, con);
                    da.Fill(ds, table);

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            Armor newArmor = new Armor(ds.Tables[0].Rows[i]["ArmorName"].ToString(), Int32Helper.Parse(ds.Tables[0].Rows[i]["ArmorDefense"]), Int32Helper.Parse(ds.Tables[0].Rows[i]["ArmorValue"]), BoolHelper.Parse(ds.Tables[0].Rows[i]["Hidden"]));

                            AllArmor.Add(newArmor);
                        }
                    }

                    sql = "SELECT * FROM Admin";
                    ds = new DataSet();
                    da = new SQLiteDataAdapter(sql, con);
                    da.Fill(ds, table);

                    if (ds.Tables[0].Rows.Count > 0)
                        AdminPassword = ds.Tables[0].Rows[0]["AdminPassword"].ToString();

                    sql = "SELECT * FROM Weapons";
                    ds = new DataSet();
                    da = new SQLiteDataAdapter(sql, con);
                    da.Fill(ds, table);

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            WeaponType newWeaponType;
                            Enum.TryParse(ds.Tables[0].Rows[i]["WeaponType"].ToString(), out newWeaponType);
                            Weapon newWeapon = new Weapon(ds.Tables[0].Rows[i]["WeaponName"].ToString(), newWeaponType, Int32Helper.Parse(ds.Tables[0].Rows[i]["WeaponDamage"]), Int32Helper.Parse(ds.Tables[0].Rows[i]["WeaponValue"]), BoolHelper.Parse(ds.Tables[0].Rows[i]["Hidden"]));

                            AllWeapons.Add(newWeapon);
                        }
                    }

                    sql = "SELECT * FROM Guilds";
                    ds = new DataSet();
                    da = new SQLiteDataAdapter(sql, con);
                    da.Fill(ds, table);

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            Guild newGuild = new Guild(Int32Helper.Parse(ds.Tables[0].Rows[i]["ID"]), ds.Tables[0].Rows[i]["GuildName"].ToString(), ds.Tables[0].Rows[i]["Guildmaster"].ToString(), Int32Helper.Parse(ds.Tables[0].Rows[i]["GuildFee"]), Int32Helper.Parse(ds.Tables[0].Rows[i]["GuildGold"]), new List<string>(), Int32Helper.Parse(ds.Tables[0].Rows[i]["HenchmenLevel1"]), Int32Helper.Parse(ds.Tables[0].Rows[i]["HenchmenLevel2"]), Int32Helper.Parse(ds.Tables[0].Rows[i]["HenchmenLevel3"]), Int32Helper.Parse(ds.Tables[0].Rows[i]["HenchmenLevel4"]), Int32Helper.Parse(ds.Tables[0].Rows[i]["HenchmenLevel5"]));

                            AllGuilds.Add(newGuild);
                        }
                    }

                    sql = "SELECT * FROM Ranks";
                    ds = new DataSet();
                    da = new SQLiteDataAdapter(sql, con);
                    da.Fill(ds, table);

                    AllRanks.Add("");

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            AllRanks.Add(ds.Tables[0].Rows[i]["RankName"].ToString());
                        }
                    }

                    sql = "SELECT * FROM Potions";
                    ds = new DataSet();
                    da = new SQLiteDataAdapter(sql, con);
                    da.Fill(ds, table);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            Potion newPotion = new Potion(ds.Tables[0].Rows[i]["PotionName"].ToString(), Int32Helper.Parse(ds.Tables[0].Rows[i]["PotionHeal"]), Int32Helper.Parse(ds.Tables[0].Rows[i]["PotionValue"]));

                            AllPotions.Add(newPotion);
                        }
                    }

                    sql = "SELECT * FROM Food";
                    ds = new DataSet();
                    da = new SQLiteDataAdapter(sql, con);
                    da.Fill(ds, table);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            Food newFood = new Food(ds.Tables[0].Rows[i]["FoodName"].ToString(), Int32Helper.Parse(ds.Tables[0].Rows[i]["RestoreHunger"]), Int32Helper.Parse(ds.Tables[0].Rows[i]["FoodValue"]));

                            AllFood.Add(newFood);
                        }
                    }

                    sql = "SELECT * FROM Drinks";
                    ds = new DataSet();
                    da = new SQLiteDataAdapter(sql, con);
                    da.Fill(ds, table);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            Drink newDrink = new Drink(ds.Tables[0].Rows[i]["DrinkName"].ToString(), Int32Helper.Parse(ds.Tables[0].Rows[i]["RestoreThirst"]), Int32Helper.Parse(ds.Tables[0].Rows[i]["DrinkValue"]));

                            AllDrinks.Add(newDrink);
                        }
                    }

                    sql = "SELECT * FROM Users";
                    ds = new DataSet();
                    da = new SQLiteDataAdapter(sql, con);
                    da.Fill(ds, table);

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            WeaponType currentWeapon;
                            Enum.TryParse(ds.Tables[0].Rows[i]["CurrentWeapon"].ToString(), out currentWeapon);
                            User newUser = new User();
                            newUser.Name = ds.Tables[0].Rows[i]["Username"].ToString();
                            newUser.Password = ds.Tables[0].Rows[i]["UserPassword"].ToString();
                            newUser.Level = Int32Helper.Parse(ds.Tables[0].Rows[i]["Level"]);
                            newUser.Experience = Int32Helper.Parse(ds.Tables[0].Rows[i]["Experience"]);
                            newUser.SkillPoints = Int32Helper.Parse(ds.Tables[0].Rows[i]["SkillPoints"]);
                            newUser.Alive = BoolHelper.Parse(ds.Tables[0].Rows[i]["Alive"]);
                            newUser.CurrentLocation = ds.Tables[0].Rows[i]["Location"].ToString();
                            newUser.CurrentEndurance = Int32Helper.Parse(ds.Tables[0].Rows[i]["CurrentEndurance"]);
                            newUser.MaximumEndurance = Int32Helper.Parse(ds.Tables[0].Rows[i]["MaximumEndurance"]);
                            newUser.Hunger = Int32Helper.Parse(ds.Tables[0].Rows[i]["Hunger"]);
                            newUser.Thirst = Int32Helper.Parse(ds.Tables[0].Rows[i]["Thirst"]);
                            newUser.CurrentWeapon = currentWeapon;
                            newUser.LightWeapon = AllWeapons.Find(wpn => wpn.Name == ds.Tables[0].Rows[i]["LightWeapon"].ToString());
                            newUser.HeavyWeapon = AllWeapons.Find(wpn => wpn.Name == ds.Tables[0].Rows[i]["HeavyWeapon"].ToString());
                            newUser.TwoHandedWeapon = AllWeapons.Find(wpn => wpn.Name == ds.Tables[0].Rows[i]["TwoHandedWeapon"].ToString());
                            newUser.Armor = AllArmor.Find(armr => armr.Name == ds.Tables[0].Rows[i]["Armor"].ToString());
                            newUser.Potion = AllPotions.Find(potn => potn.Name == ds.Tables[0].Rows[i]["Potion"].ToString());
                            newUser.Lockpicks = Int32Helper.Parse(ds.Tables[0].Rows[i]["Lockpicks"]);
                            newUser.GoldOnHand = Int32Helper.Parse(ds.Tables[0].Rows[i]["GoldOnHand"]);
                            newUser.GoldInBank = Int32Helper.Parse(ds.Tables[0].Rows[i]["GoldInBank"]);
                            newUser.GoldOnLoan = Int32Helper.Parse(ds.Tables[0].Rows[i]["GoldOnLoan"]);
                            newUser.Shovel = BoolHelper.Parse(ds.Tables[0].Rows[i]["Shovel"]);
                            newUser.Lantern = BoolHelper.Parse(ds.Tables[0].Rows[i]["Lantern"]);
                            newUser.Amulet = BoolHelper.Parse(ds.Tables[0].Rows[i]["Amulet"]);
                            newUser.LightWeaponSkill = Int32Helper.Parse(ds.Tables[0].Rows[i]["LightWeaponSkill"]);
                            newUser.HeavyWeaponSkill = Int32Helper.Parse(ds.Tables[0].Rows[i]["HeavyWeaponSkill"]);
                            newUser.TwoHandedWeaponSkill = Int32Helper.Parse(ds.Tables[0].Rows[i]["TwoHandedWeaponSkill"]);
                            newUser.Blocking = Int32Helper.Parse(ds.Tables[0].Rows[i]["Blocking"]);
                            newUser.Slipping = Int32Helper.Parse(ds.Tables[0].Rows[i]["Slipping"]);
                            newUser.Stealth = Int32Helper.Parse(ds.Tables[0].Rows[i]["Stealth"]);
                            newUser.HenchmenLevel1 = Int32Helper.Parse(ds.Tables[0].Rows[i]["HenchmenLevel1"]);
                            newUser.HenchmenLevel2 = Int32Helper.Parse(ds.Tables[0].Rows[i]["HenchmenLevel2"]);
                            newUser.HenchmenLevel3 = Int32Helper.Parse(ds.Tables[0].Rows[i]["HenchmenLevel3"]);
                            newUser.HenchmenLevel4 = Int32Helper.Parse(ds.Tables[0].Rows[i]["HenchmenLevel4"]);
                            newUser.HenchmenLevel5 = Int32Helper.Parse(ds.Tables[0].Rows[i]["HenchmenLevel5"]);

                            AllUsers.Add(newUser);
                        }
                    }

                    sql = "SELECT * FROM Enemies";
                    ds = new DataSet();
                    da = new SQLiteDataAdapter(sql, con);
                    da.Fill(ds, table);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            Enemy newEnemy = new Enemy(ds.Tables[0].Rows[i]["EnemyName"].ToString(), Int32Helper.Parse(ds.Tables[0].Rows[i]["Level"]), Int32Helper.Parse(ds.Tables[0].Rows[i]["Endurance"]), Int32Helper.Parse(ds.Tables[0].Rows[i]["Endurance"]), new Weapon(AllWeapons.Find(wpn => wpn.Name == (ds.Tables[0].Rows[i]["Weapon"]).ToString())), new Armor(AllArmor.Find(armr => armr.Name == (ds.Tables[0].Rows[i]["Armor"]).ToString())), Int32Helper.Parse(ds.Tables[0].Rows[i]["Gold"]), Int32Helper.Parse(ds.Tables[0].Rows[i]["Attack"]), Int32Helper.Parse(ds.Tables[0].Rows[i]["Blocking"]), Int32Helper.Parse(ds.Tables[0].Rows[i]["Slipping"]));

                            AllEnemies.Add(newEnemy);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error Filling DataSet", MessageBoxButton.OK);
                }
                finally { con.Close(); }

                AllArmor = AllArmor.OrderBy(armor => armor.Value).ToList();
                AllDrinks = AllDrinks.OrderBy(drink => drink.Value).ToList();
                AllEnemies = AllEnemies.OrderBy(enemy => enemy.Level).ToList();
                AllFood = AllFood.OrderBy(food => food.Value).ToList();
                AllGuilds = AllGuilds.OrderBy(guild => guild.ID).ToList();
                AllPotions = AllPotions.OrderBy(potion => potion.Value).ToList();
                AllUsers = AllUsers.OrderBy(user => user.Name).ToList();
                AllWeapons = AllWeapons.OrderBy(weapon => weapon.Value).ToList();
            });
        }

        #region Random Number Generation

        /// <summary>
        /// Generates a random number between min and max (inclusive).
        /// </summary>
        /// <param name="min">Inclusive minimum number</param>
        /// <param name="max">Inclusive maximum number</param>
        /// <returns>Returns randomly generated integer between min and max.</returns>
        internal static int GenerateRandomNumber(int min, int max)
        {
            return GenerateRandomNumber(min, max, Int32.MaxValue);
        }

        /// <summary>
        /// Generates a random number between min and max (inclusive).
        /// </summary>
        /// <param name="min">Inclusive minimum number</param>
        /// <param name="max">Inclusive maximum number</param>
        /// <param name="upperLimit">Maximum limit for the method, regardless of min and max.</param>
        /// <returns>Returns randomly generated integer between min and max with an upper limit of upperLimit.</returns>
        internal static int GenerateRandomNumber(int min, int max, int upperLimit)
        {
            int result;

            if (min < max)
                result = ThreadSafeRandom.ThisThreadsRandom.Next(min, max + 1);
            else
                result = ThreadSafeRandom.ThisThreadsRandom.Next(max, min + 1);

            if (result > upperLimit)
                return upperLimit;

            return result;
        }

        #endregion Random Number Generation

        internal static bool CheckLogin(string username, string password)
        {
            User checkUser = new User();
            try
            {
                checkUser = AllUsers.Find(hero => hero.Name == username);
                if (PasswordHash.ValidatePassword(password, checkUser.Password))
                {
                    CurrentUser = new User(checkUser);
                    return true;
                }
                else
                {
                    MessageBox.Show("Invalid login.", "Assassin", MessageBoxButton.OK);
                    return false;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Invalid login.", "Assassin", MessageBoxButton.OK);
                return false;
            }
        }

        /// <summary>
        /// Turns several Keyboard.Keys into a list of Keys which can be tested using List.Any.
        /// </summary>
        /// <param name="keys">Array of Keys</param>
        /// <returns></returns>
        internal static List<bool> GetListOfKeys(params Key[] keys)
        {
            List<bool> allKeys = new List<bool>();
            foreach (Key key in keys)
                allKeys.Add(Keyboard.IsKeyDown(key));
            return allKeys;
        }

        /// <summary>
        /// Saves the current User.
        /// </summary>
        internal async static Task<bool> SaveUser(User saveUser)
        {
            bool success = false;
            SQLiteCommand cmd = new SQLiteCommand();
            SQLiteConnection con = new SQLiteConnection();
            con.ConnectionString = _DBPROVIDERANDSOURCE;

            string sql = "UPDATE Users SET [Level] = @level, [Experience] = @experience, [SkillPoints] = @skillPoints, [Alive] = @alive, [Location] = location, [CurrentEndurance] = @currentEndurance, [MaximumEndurance] = @maximumEndurance, [Hunger] = @hunger, [Thirst] = @thirst, [CurrentWeapon] = @currentWeapon, [LightWeapon] = @lightWeapon, [HeavyWeapon] = @heavyWeapon, [TwoHandedWeapon] = @twoHandedWeapon, [Armor] = @armor, [Potion] = @potion, [Lockpicks] = @lockpicks, [GoldOnHand] = @goldOnHand, [GoldInBank] = @goldInBank, [GoldOnLoan] = @goldOnLoan, [Shovel] = @shovel, [Lantern] = @lantern, [Amulet] = @amulet, [LightWeaponSkill] = @lightWeaponSkill, [HeavyWeaponSkill] = @heavyWeaponSkill, [TwoHandedWeaponSkill] = @twoHandedWeaponSkill, [Blocking] = @blocking, [Slipping] = @slipping, [Stealth] = @stealth, [HenchmenLevel1] = @henchmenLevel1, [HenchmenLevel2] = @henchmenLevel2, [HenchmenLevel3] = @henchmenLevel3, [HenchmenLevel4] = @henchmenLevel4, [HenchmenLevel5] = @henchmenLevel5 WHERE [Username] = @name";

            cmd.CommandType = CommandType.Text;
            cmd.CommandText = sql;
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

            await Task.Factory.StartNew(() =>
            {
                try
                {
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    int index = AllUsers.FindIndex(user => user.Name == saveUser.Name);
                    AllUsers[index] = new User(saveUser);
                    success = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error Saving User", MessageBoxButton.OK);
                }
                finally { con.Close(); }
            });
            return success;
        }

        internal async static Task<bool> CreateUser(User newUser)
        {
            SQLiteConnection con = new SQLiteConnection();
            SQLiteDataAdapter da = new SQLiteDataAdapter();
            con.ConnectionString = _DBPROVIDERANDSOURCE;

            bool success = false;
            await Task.Factory.StartNew(() =>
            {
                try
                {
                    string sql = "SELECT * FROM Users WHERE Username='" + newUser.Name + "'";
                    string table = "User";
                    DataSet ds = new DataSet();
                    da = new SQLiteDataAdapter(sql, con);
                    da.Fill(ds, table);

                    if (ds.Tables[0].Rows.Count > 0 | newUser.Name == "Computer")
                        MessageBox.Show("This username has already been used.", "Assassin", MessageBoxButton.OK);
                    else
                    {
                        SQLiteCommand cmd = con.CreateCommand();
                        cmd.CommandText = "Insert into Users([Username],[UserPassword],[Level],[Experience],[SkillPoints],[Alive],[Location],[CurrentEndurance],[MaximumEndurance],[Hunger],[Thirst],[CurrentWeapon],[LightWeapon],[HeavyWeapon],[TwoHandedWeapon],[Armor],[Potion],[Lockpicks],[GoldOnHand],[GoldInBank],[GoldOnLoan],[Shovel],[Lantern],[Amulet],[LightWeaponSkill],[HeavyWeaponSkill],[TwoHandedWeaponSkill],[Blocking],[Slipping],[Stealth],[HenchmenLevel1],[HenchmenLevel2],[HenchmenLevel3],[HenchmenLevel4],[HenchmenLevel5])Values('" + newUser.Name + "','" + newUser.Password + "','" + newUser.Level + "','" + newUser.Experience + "','" + newUser.SkillPoints + "'," + Int32Helper.Parse(newUser.Alive) + ",'" + newUser.CurrentLocation + "','" + newUser.CurrentEndurance + "','" + newUser.MaximumEndurance + "','" + newUser.Hunger + "','" + newUser.Thirst + "','" + newUser.CurrentWeapon + "','" + newUser.LightWeapon.Name + "','" + newUser.HeavyWeapon.Name + "','" + newUser.TwoHandedWeapon.Name + "','" + newUser.Armor.Name + "','" + newUser.Potion.Name + "','" + newUser.Lockpicks + "','" + newUser.GoldOnHand + "','" + newUser.GoldInBank + "','" + newUser.GoldOnLoan + "'," + Int32Helper.Parse(newUser.Shovel) + "," + Int32Helper.Parse(newUser.Lantern) + "," + Int32Helper.Parse(newUser.Amulet) + ",'" + newUser.LightWeaponSkill + "','" + newUser.HeavyWeaponSkill + "','" + newUser.TwoHandedWeaponSkill + "','" + newUser.Blocking + "','" + newUser.Slipping + "','" + newUser.Stealth + "','" + newUser.HenchmenLevel1 + "','" + newUser.HenchmenLevel2 + "','" + newUser.HenchmenLevel3 + "','" + newUser.HenchmenLevel4 + "','" + newUser.HenchmenLevel5 + "')";

                        con.Open();
                        cmd.Connection = con;
                        cmd.ExecuteNonQuery();
                        cmd.CommandText = "Insert into Guild1Members([Username])Values('" + newUser.Name + "')";
                        cmd.ExecuteNonQuery();
                        success = true;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error Creating New User", MessageBoxButton.OK);
                }
                finally { con.Close(); }
            });

            return success;
        }

        //end user exists
        //if passwords don't match

        /// <summary>
        /// Selects an Enemy based on the User's current level.
        /// </summary>
        /// <returns>Selected Enemy</returns>
        internal static Enemy SelectEnemy()
        {
            int index = 0;
            int random = GenerateRandomNumber(1, 100);
            switch (CurrentUser.Level)
            {
                case 1:
                    if (random <= 65)
                        index = 0;
                    else
                        index = 1;
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
            newEnemy.GoldOnHand = GenerateRandomNumber(newEnemy.GoldOnHand / 2, newEnemy.GoldOnHand);
            return newEnemy;
        }
    }
}