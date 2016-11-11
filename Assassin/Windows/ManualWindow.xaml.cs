using System;
using System.ComponentModel;
using System.Windows;

namespace Assassin
{
    /// <summary>
    /// Interaction logic for ManualWindow.xaml
    /// </summary>
    public partial class ManualWindow : Window
    {
        internal MainWindow RefToMainWindow { get; set; }
        private string nl = Environment.NewLine;

        private void Introduction()
        {
            txtManual.Text = " * * * ASSASSIN * * *" + nl + nl +

        "Lightning crackles through the the stormy skies, as screams are heard in the distance." + nl +
        "War continues on between nobles, as they send their soldiers and mercenaries to do their bidding, to gain more land and power." + nl +
        "The church watches quietly, patiently, waiting to take a chunk of the newly won land." + nl +
        "Death covers the land, as another age of darkness approaches." + nl +
        "Life is basically wretched for most." + nl +
        "They are not fulfilled, or not even given enough to survive, most being the latter." + nl +
        "The standard of life is worthless." + nl +
        "Many take second jobs as highwaymen, stealing from others so that they can survive." + nl +
        "Others serve the war efforts, which proves only slightly better, since most do not survive from skirmish to skirmish." + nl +
        "The few survivors gain living wages." + nl +
        "A few elite warriors manage to make it up the perilous ladder, only to lose their hard-won gains to a rival noble." + nl +
        "There is a possible hope coming, which is known as magic, but some believe this same life-restoring power is part of the problem." + nl +
        "A few smart commoners have learned of another calling." + nl +
        "There is indeed need of people to eliminate problems such as rival nobles, and power hungry clergy members." + nl +
        "Instead of fighting forces of insurmountable odds, they fight for the one cause." + nl +
        "These wanted persons being Assassins..." + nl + nl +

        "ASSASSIN is a role-playing game that takes place in a time similar to the dark ages, where assassins work to make their fortune." + nl +
        "You, as a player, fight to gain experience, gold, and the control of your own guild as you try to become the most powerful assassin in the city." + nl +
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
            txtManual.Text = " * * * THE CITY * * * " + nl + nl +
        "ASSASSINATE - This command puts your assassin in the position to watch for a promising target." + nl +
        "If you are successful, you will gain experience, skill, and money." + nl +
        "If not, you maybe lucky to escape with your life!" + nl + nl +

        "BANK - For your finacial needs the bank is here." + nl +
        "You can take out a loan, make a deposit to your account, check your current balances, as well as a few other interesting things." + nl + nl +

        "GUILDS - Guilds provide a fairly safe place to stay overnight, as well as opportunities to get paid for assassinations." + nl +
        "You can even begin recruiting the henchmen you will need later in the game." + nl +
        "Each guild has an initiation fee you must pay to join." + nl +
        "All new players are given a complimentary membership to the Master's Tavern." + nl +
        "This should be taken advantage of." + nl +
        "One advantage is that the job offerings provided give a good source of income." + nl + nl +

        "INN - After a hard day of assassinations and walking around the city, an assassin tends to get tired and worn out." + nl +
        "The Inn is here to serve your full needs for protection and privacy." + nl +
        "You can either nap to regain some lost endurance or spend the night and be well protected." + nl +
        "Inns also provide a messenger service, along with the daily newspaper." + nl +

        "JAIL - Take a look at all the Assassin's who've been caught in their illegal activities.  You can even BAIL them out!" + nl + nl +

        "ALCHEMIST'S LABORATORY - The lab is here for those that wish a speedy recovery after a long, grueling battle." + nl +
        "They currently are working on potions that will cause your system to heal quickly." + nl +
        "If you purchase a potion, you can either drink it there or take it along until you may really need it." + nl +
        "Be warned though, not all potions are beneficial.  Some can even be harmful!" + nl + nl +

        "OTHER ASSASSINS - This will give you a list of all the living assassins that currently make their stay in the city and that you may encounter in the future." + nl + nl +

        "PUB - When you are hungry or thirsty, stop by the pub, it aims to please." + nl + nl +

        "ROB SOMEONE - If you need some money, you may prefer to steal it rather than fight for it." + nl +
        "Your methods are simple: Waylay an opponent, or Pickpocket your victim." + nl +
        "Waylaying usually yields more than picking, but pickpocketing is far less dangerous." + nl + nl +

        "SHOPS - More than likely you will visit here a good few times." + nl +
        "There are many places to shop, to satisfy many needs." + nl +
        "Window-shopping is acceptable, however stealing is quite punishable." + nl + nl +

        "TRAINING GROUNDS - Here's where you begin and should visit often during the game." + nl +
        "The training grounds offer courses in offensive and defensive skills by using your accumulated skill points for training sessions.";
        }

        private void btnSkills_Click(object sender, RoutedEventArgs e)
        {
            txtManual.ScrollToHome();
            txtManual.Text = " * * * SKILLS * * * " + nl + nl +

        "Skill is gained through battles and through certain events." + nl +
        "Skill points will be gained if the assassin is successful, and sometimes even when he fails an attempt." + nl +
        "Skill points can be redeemed at the Training Grounds." + nl + nl +

        "ENDURANCE is your ability to withstand punishment." + nl +
        "The more of this you have, the longer you can endure." + nl + nl +

        "LIGHT WEAPON skill relates to the dexterity of the individual." + nl +
        "This ability is used to manipulate cutting weapons and items that weigh up to ten pounds." + nl +
        "Light weapons are meant to be used to cut the opponent and try to sever a major vein." + nl +
        "Weapons in the category of light weapon include daggers and most types of swords." + nl + nl +

        "HEAVY WEAPON skill relates to the strength of the individual." + nl +
        "The ability is used to manipulate bashing weapons and items up to twenty pounds efficiently." + nl +
        "Heavier weapons are not meant to be used in the way of swords." + nl +
        "Instead, they are be used to make a major impact upon the opponent and break them." + nl +
        "Heavier weapons include clubs, maces, flails, morning stars and most axes." + nl + nl +

        "TWO-HANDED WEAPON skill relates to the the ability to quickly manipulate larger sized items." + nl +
        "These weapons are usually held with both hands; hence the name." + nl +
        "When used, the weapon gives a superior edge both in defense and damage due to the concentrated effort." + nl +
        "Two-handed weapons include staves, warhammers, halberds, and great weapons." + nl + nl +

        "BLOCKING is a defensive skill in which the assassin uses an item to block oncoming attacks." + nl +
        "This defense is used against weapons by countering the force of the blow with an equal or better force." + nl +
        "Blocking is also used in determining your ability to prevent an enemy from fleeing." + nl +

        "SLIPPING is the defensive skill used to maneuver out of the way of a blow before it strikes." + nl +
        "Slipping is also used in determining how well you can escape a fight." + nl + nl +

        "STEALTH is the ability to hide in shadows, to escape when being pursued and generally disappear when the tough needs to get going." + nl +
        "Stealth is a statistic used throughout the game, from sneaking a first blow to pickpocketing an unsuspecting victim." + nl +
        "The more stealth you have, the better you will fare.";
        }

        private void btnPlaying_Click(object sender, RoutedEventArgs e)
        {
            txtManual.ScrollToHome();
            txtManual.Text = " * * * PLAYING THE GAME * * * " + nl + nl +

        "The value on which all assassins are based is their experience." + nl +
        "Experience is gained through successful assassinations and other events." + nl +
        "It controls many factors in an assassin's life, such as prestige, infamy, knowledge, and ability." + nl +
        "For every ten experience points, an assassin achieves a new level of mastery." + nl +
        "At each level of mastery, assassins become more and more advanced." + nl +
        "Some of these enhancements are blending into the surroundings striking with deadlier force, and skill with unusual and unknown weapons." + nl + nl +

        "The bread and butter of all assassins is assassination (isn't that obvious? + nl +" + nl +
        "It is done in two phases: the stalking and then the actual attack." + nl +
        "The first phase is done in one of three ways." + nl + nl +

        "The first method is simply look around for a promising victim, one of the appropriate level and of the proper prestige." + nl +
        "This method provides only a small amount of coins, and a weapon." + nl + nl +

        "The second method is to be hired by someone." + nl +
        "This usually provides a larger amount of coins, but the weapon must be given to the employer as proof of the slain opponent." + nl + nl +

        "The third method is the full blown assassination." + nl +
        "This method is employed against other assassins" + nl +
        "It is a battle first against the opponent's henchmen, followed by a final conflict between the two assassins." + nl + nl +

        "Raids are used to gain wealth and fame, as well as prove you are better than other assassins." + nl +
        "Usually raids are between guildmasters, however sometimes raids are against city banks." + nl + nl +

        "Raids against guilds are simple." + nl +
        "You group your assassin and henchmen against a guild, then storm in, wreaking as much havoc and damage as possible, then looting the remains and taking it over yourself." + nl + nl +

        "Raids against banks are just as simple, with the exception that you are not trying to permanently take over the bank, but rather take it over temporarily so that you can steal as much as possible." + nl +
        "However, raiding banks can prove more challenging and hazardous since all gains are earned through others' accounts." + nl + nl +

        "A common establishment an assassin will encounter is the Guild." + nl +
        "Any promising assassin will eventually find his or her path leading there." + nl +
        "A guild can be located from anywhere from a Tavern to a Hotel." + nl +
        "At first you must choose which guild to go to." + nl +
        "A list of the available guilds will be displayed from which to choose." + nl +
        "Once inside, your options available will depend on your status." + nl +
        "Non-members may wish to send an application to join the guild." + nl +
        "Each guild is controlled by a guildmaster, who may be another player's character." + nl +
        "Computer-controlled guildmasters always allow players to join if they qualify." + nl +
        "Applicants to player-controlled guilds have to wait for a response from the guildmaster." + nl +
        "In most guilds, the player can challenge the guildmaster for control of the guild." + nl +
        "The ensuing battle will pit the player's henchmen against the guild's henchmen." + nl +
        "If the player's henchmen defeat the guild's henchmen, the player will face the guildmaster in one-on-one combat." + nl +
        "If the player defeats the guildmaster, the player takes control of the guild. If the player fails, he is ejected from the guild." + nl +
        "The Master's Tavern, the guild new players are automatically entered into, can not be controlled by a player." + nl +
        "After a player reaches level 6, the player will be ejected from The Master's Tavern, as they are too experienced." + nl + nl +

        "The alley is where you can find the toughest, possibly most dangerous assassins around, or perhaps ones that don't have a place to shelter overnight." + nl +
        "It is an Arena where only the strongest survive and even then sometimes they don't live to see the next day.";
        }

        private void btnCombat_Click(object sender, RoutedEventArgs e)
        {
            txtManual.ScrollToHome();
            txtManual.Text = " * * * COMBAT * * * " + nl + nl +

        "In all assassinations and even some raids you may find yourself locked into mortal combat." + nl +
        "To destroy or be destroyed, that is your goal." + nl +
        "First, you will get to ready one of your weapons." + nl +
        "Status displays will be the next thing that you see." + nl +
        "These give you an overall status of you and your opponent." + nl +
        "The levels of status are: Healthy, Wounded, Heavily Wounded, Seriously Wounded, and Critical." + nl +
        "At times you may notice the status of Stunned as combat ensues." + nl +
        "If you are stunned, you will not be able to do anything except wait until you recover." + nl +
        "If your opponent is stunned you may wish to take advantage of the situation." + nl +
        "Get in a few good swings, or, if your opponent is too formidable, Flee." + nl + nl +

        "You will also notice a fatigue level." + nl +
        "If you become too fatigued, you will not be able to fight until you gain stamina back." + nl +
        "The rate of fatigue increases quicker by going Berserk and decreases by Defending." + nl + nl +

        "Your attack options include:" + nl + nl +

        "ATTACK - A normal attack." + nl +
        "BERSERK - This options put your assassin in a totally uncontrollable rage." + nl +
        "Your attacks do twice the damage, but will fatigue you twice as quickly." + nl +
        "PARRY - Attempt to counter your opponent's attack, increasing the chance they will become stunned." + nl +
        "DEFEND - Increase your chance to block or dodge an enemy's attack." + nl +
        "This option also recovers fatigue." + nl +
        "FLEE - Attempt to escape from the battle." + nl + nl +

        "You will also have the option of drinking a potion to recover Endurance during the battle." + nl +
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