using Assassin.Models;
using Assassin.Models.Entities;
using Assassin.Views.Guilds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Assassin.Views.City
{
    /// <summary>Interaction logic for MembersPage.xaml</summary>
    public partial class MembersPage : Page
    {
        private List<User> _users { get; set; }
        internal GuildPage RefToGuildPage { get; set; }

        public MembersPage(GuildPage guildPage)
        {
            InitializeComponent();
            RefToGuildPage = guildPage;
            _users = GameState.AllUsers.Where(user => GameState.CurrentGuild.Members.Contains(user.Name)).ToList();
        }

        public MembersPage(GamePage gamePage)
        {
            _users = GameState.AllUsers;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
        }
    }
}