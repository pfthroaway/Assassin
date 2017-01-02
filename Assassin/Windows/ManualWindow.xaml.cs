using System.ComponentModel;
using System.Windows;

namespace Assassin
{
    /// <summary>
    /// Interaction logic for ManualWindow.xaml
    /// </summary>
    public partial class ManualWindow
    {
        internal MainWindow RefToMainWindow { private get; set; }

        private void Introduction()
        {
            txtManual.Text = " * * * ASSASSIN * * *\n\n" +
        "Lightning crackles through the the stormy skies, as screams are heard in the distance.\n" +
        "War continues on between nobles, as they send their soldiers and mercenaries to do their bidding, to gain more land and power.\n" +
        "The church watches quietly, patiently, waiting to take a chunk of the newly won land.\n" +
        "Death covers the land, as another age of darkness approaches.\n" +
        "Life is basically wretched for most.\n" +
        "They are not fulfilled, or not even given enough to survive, most being the latter.\n" +
        "The standard of life is worthless.\n" +
        "Many take second jobs as highwaymen, stealing from others so that they can survive.\n" +
        "Others serve the war efforts, which proves only slightly better, since most do not survive from skirmish to skirmish.\n" +
        "The few survivors gain living wages.\n" +
        "A few elite warriors manage to make it up the perilous ladder, only to lose their hard-won gains to a rival noble.\n" +
        "There is a possible hope coming, which is known as magic, but some believe this same life-restoring power is part of the problem.\n" +
        "A few smart commoners have learned of another calling.\n" +
        "There is indeed need of people to eliminate problems such as rival nobles, and power hungry clergy members.\n" +
        "Instead of fighting forces of insurmountable odds, they fight for the one cause.\n" +
        "These wanted persons being Assassins...\n\n" +

        "ASSASSIN is a role-playing game that takes place in a time similar to the dark ages, where assassins work to make their fortune.\n" +
        "You, as a player, fight to gain experience, gold, and the control of your own guild as you try to become the most powerful assassin in the city.\n" +
        "As the game progresses, you will learn of other missions to complete.";
        }

        #region Button-Click Methods

        private void btnIntroduction_Click(object sender, RoutedEventArgs e)
        {
            txtManual.ScrollToHome();
            Introduction();
        }

        private void btnCity_Click(object sender, RoutedEventArgs e)
        {
            txtManual.ScrollToHome();
            txtManual.Text = " * * * THE CITY * * * \n\n" +
        "ASSASSINATE - This command puts your assassin in the position to watch for a promising target.\n" +
        "If you are successful, you will gain experience, skill, and money.\n" +
        "If not, you maybe lucky to escape with your life!\n\n" +

        "BANK - For your finacial needs the bank is here.\n" +
        "You can take out a loan, make a deposit to your account, check your current balances, as well as a few other interesting things.\n\n" +

        "GUILDS - Guilds provide a fairly safe place to stay overnight, as well as opportunities to get paid for assassinations.\n" +
        "You can even begin recruiting the henchmen you will need later in the game.\n" +
        "Each guild has an initiation fee you must pay to join.\n" +
        "All new players are given a complimentary membership to the Master's Tavern.\n" +
        "This should be taken advantage of.\n" +
        "One advantage is that the job offerings provided give a good source of income.\n\n" +

        "INN - After a hard day of assassinations and walking around the city, an assassin tends to get tired and worn out.\n" +
        "The Inn is here to serve your full needs for protection and privacy.\n" +
        "You can either nap to regain some lost endurance or spend the night and be well protected.\n" +
        "Inns also provide a messenger service, along with the daily newspaper.\n" +

        "JAIL - Take a look at all the Assassin's who've been caught in their illegal activities.  You can even BAIL them out!\n\n" +

        "ALCHEMIST'S LABORATORY - The lab is here for those that wish a speedy recovery after a long, grueling battle.\n" +
        "They currently are working on potions that will cause your system to heal quickly.\n" +
        "If you purchase a potion, you can either drink it there or take it along until you may really need it.\n" +
        "Be warned though, not all potions are beneficial.  Some can even be harmful!\n\n" +

        "OTHER ASSASSINS - This will give you a list of all the living assassins that currently make their stay in the city and that you may encounter in the future.\n\n" +

        "PUB - When you are hungry or thirsty, stop by the pub, it aims to please.\n\n" +

        "ROB SOMEONE - If you need some money, you may prefer to steal it rather than fight for it.\n" +
        "Your methods are simple: Waylay an opponent, or Pickpocket your victim.\n" +
        "Waylaying usually yields more than picking, but pickpocketing is far less dangerous.\n\n" +

        "SHOPS - More than likely you will visit here a good few times.\n" +
        "There are many places to shop, to satisfy many needs.\n" +
        "Window-shopping is acceptable, however stealing is quite punishable.\n\n" +

        "TRAINING GROUNDS - Here's where you begin and should visit often during the game.\n" +
        "The training grounds offer courses in offensive and defensive skills by using your accumulated skill points for training sessions.";
        }

        private void btnSkills_Click(object sender, RoutedEventArgs e)
        {
            txtManual.ScrollToHome();
            txtManual.Text = " * * * SKILLS * * * \n\n" +

        "Skill is gained through battles and through certain events.\n" +
        "Skill points will be gained if the assassin is successful, and sometimes even when he fails an attempt.\n" +
        "Skill points can be redeemed at the Training Grounds.\n\n" +

        "ENDURANCE is your ability to withstand punishment.\n" +
        "The more of this you have, the longer you can endure.\n\n" +

        "LIGHT WEAPON skill relates to the dexterity of the individual.\n" +
        "This ability is used to manipulate cutting weapons and items that weigh up to ten pounds.\n" +
        "Light weapons are meant to be used to cut the opponent and try to sever a major vein.\n" +
        "Weapons in the category of light weapon include daggers and most types of swords.\n\n" +

        "HEAVY WEAPON skill relates to the strength of the individual.\n" +
        "The ability is used to manipulate bashing weapons and items up to twenty pounds efficiently.\n" +
        "Heavier weapons are not meant to be used in the way of swords.\n" +
        "Instead, they are be used to make a major impact upon the opponent and break them.\n" +
        "Heavier weapons include clubs, maces, flails, morning stars and most axes.\n\n" +

        "TWO-HANDED WEAPON skill relates to the the ability to quickly manipulate larger sized items.\n" +
        "These weapons are usually held with both hands; hence the name.\n" +
        "When used, the weapon gives a superior edge both in defense and damage due to the concentrated effort.\n" +
        "Two-handed weapons include staves, warhammers, halberds, and great weapons.\n\n" +

        "BLOCKING is a defensive skill in which the assassin uses an item to block oncoming attacks.\n" +
        "This defense is used against weapons by countering the force of the blow with an equal or better force.\n" +
        "Blocking is also used in determining your ability to prevent an enemy from fleeing.\n" +

        "SLIPPING is the defensive skill used to maneuver out of the way of a blow before it strikes.\n" +
        "Slipping is also used in determining how well you can escape a fight.\n\n" +

        "STEALTH is the ability to hide in shadows, to escape when being pursued and generally disappear when the tough needs to get going.\n" +
        "Stealth is a statistic used throughout the game, from sneaking a first blow to pickpocketing an unsuspecting victim.\n" +
        "The more stealth you have, the better you will fare.";
        }

        private void btnPlaying_Click(object sender, RoutedEventArgs e)
        {
            txtManual.ScrollToHome();
            txtManual.Text = " * * * PLAYING THE GAME * * * \n\n" +

        "The value on which all assassins are based is their experience.\n" +
        "Experience is gained through successful assassinations and other events.\n" +
        "It controls many factors in an assassin's life, such as prestige, infamy, knowledge, and ability.\n" +
        "For every ten experience points, an assassin achieves a new level of mastery.\n" +
        "At each level of mastery, assassins become more and more advanced.\n" +
        "Some of these enhancements are blending into the surroundings striking with deadlier force, and skill with unusual and unknown weapons.\n\n" +

        "The bread and butter of all assassins is assassination (isn't that obvious? + nl +\n" +
        "It is done in two phases: the stalking and then the actual attack.\n" +
        "The first phase is done in one of three ways.\n\n" +

        "The first method is simply look around for a promising victim, one of the appropriate level and of the proper prestige.\n" +
        "This method provides only a small amount of coins, and a weapon.\n\n" +

        "The second method is to be hired by someone.\n" +
        "This usually provides a larger amount of coins, but the weapon must be given to the employer as proof of the slain opponent.\n\n" +

        "The third method is the full blown assassination.\n" +
        "This method is employed against other assassins\n" +
        "It is a battle first against the opponent's henchmen, followed by a final conflict between the two assassins.\n\n" +

        "Raids are used to gain wealth and fame, as well as prove you are better than other assassins.\n" +
        "Usually raids are between guildmasters, however sometimes raids are against city banks.\n\n" +

        "Raids against guilds are simple.\n" +
        "You group your assassin and henchmen against a guild, then storm in, wreaking as much havoc and damage as possible, then looting the remains and taking it over yourself.\n\n" +

        "Raids against banks are just as simple, with the exception that you are not trying to permanently take over the bank, but rather take it over temporarily so that you can steal as much as possible.\n" +
        "However, raiding banks can prove more challenging and hazardous since all gains are earned through others' accounts.\n\n" +

        "A common establishment an assassin will encounter is the Guild.\n" +
        "Any promising assassin will eventually find his or her path leading there.\n" +
        "A guild can be located from anywhere from a Tavern to a Hotel.\n" +
        "At first you must choose which guild to go to.\n" +
        "A list of the available guilds will be displayed from which to choose.\n" +
        "Once inside, your options available will depend on your status.\n" +
        "Non-members may wish to send an application to join the guild.\n" +
        "Each guild is controlled by a guildmaster, who may be another player's character.\n" +
        "Computer-controlled guildmasters always allow players to join if they qualify.\n" +
        "Applicants to player-controlled guilds have to wait for a response from the guildmaster.\n" +
        "In most guilds, the player can challenge the guildmaster for control of the guild.\n" +
        "The ensuing battle will pit the player's henchmen against the guild's henchmen.\n" +
        "If the player's henchmen defeat the guild's henchmen, the player will face the guildmaster in one-on-one combat.\n" +
        "If the player defeats the guildmaster, the player takes control of the guild. If the player fails, he is ejected from the guild.\n" +
        "The Master's Tavern, the guild new players are automatically entered into, can not be controlled by a player.\n" +
        "After a player reaches level 6, the player will be ejected from The Master's Tavern, as they are too experienced.\n\n" +

        "The alley is where you can find the toughest, possibly most dangerous assassins around, or perhaps ones that don't have a place to shelter overnight.\n" +
        "It is an Arena where only the strongest survive and even then sometimes they don't live to see the next day.";
        }

        private void btnCombat_Click(object sender, RoutedEventArgs e)
        {
            txtManual.ScrollToHome();
            txtManual.Text = " * * * COMBAT * * * \n\n" +

        "In all assassinations and even some raids you may find yourself locked into mortal combat.\n" +
        "To destroy or be destroyed, that is your goal.\n" +
        "First, you will get to ready one of your weapons.\n" +
        "Status displays will be the next thing that you see.\n" +
        "These give you an overall status of you and your opponent.\n" +
        "The levels of status are: Healthy, Wounded, Heavily Wounded, Seriously Wounded, and Critical.\n" +
        "At times you may notice the status of Stunned as combat ensues.\n" +
        "If you are stunned, you will not be able to do anything except wait until you recover.\n" +
        "If your opponent is stunned you may wish to take advantage of the situation.\n" +
        "Get in a few good swings, or, if your opponent is too formidable, Flee.\n\n" +

        "You will also notice a fatigue level.\n" +
        "If you become too fatigued, you will not be able to fight until you gain stamina back.\n" +
        "The rate of fatigue increases quicker by going Berserk and decreases by Defending.\n\n" +

        "Your attack options include:\n\n" +

        "ATTACK - A normal attack.\n" +
        "BERSERK - This options put your assassin in a totally uncontrollable rage.\n" +
        "Your attacks do twice the damage, but will fatigue you twice as quickly.\n" +
        "PARRY - Attempt to counter your opponent's attack, increasing the chance they will become stunned.\n" +
        "DEFEND - Increase your chance to block or dodge an enemy's attack.\n" +
        "This option also recovers fatigue.\n" +
        "FLEE - Attempt to escape from the battle.\n\n" +

        "You will also have the option of drinking a potion to recover Endurance during the battle.\n" +
        "You can also change your weapon if it proves ineffective during the battle.";
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        #endregion Button-Click Methods

        #region Window-Manipulation Methods

        public ManualWindow()
        {
            InitializeComponent();
            Introduction();
        }

        private void windowManual_Closing(object sender, CancelEventArgs e)
        {
            RefToMainWindow.Show();
        }

        #endregion Window-Manipulation Methods
    }
}