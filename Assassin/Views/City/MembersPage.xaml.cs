using Assassin.Models;
using Assassin.Models.Entities;
using Assassin.Models.Enums;
using Assassin.Views.Guilds;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace Assassin.Views.City
{
    /// <summary>Interaction logic for MembersPage.xaml</summary>
    public partial class MembersPage : Page, INotifyPropertyChanged
    {
        private string _previousPage;
        private List<User> _users;
        private User _selectedUser = new User();

        /// <summary>List of <see cref="User"/>s currently accessible from the previous area.</summary>
        private List<User> Users
        {
            get => _users;
            set
            {
                _users = value;
                NotifyPropertyChanged(nameof(Users));
            }
        }

        #region INPC Members

        /// <summary>The event that is raised when a property that calls the NotifyPropertyChanged method is changed.</summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>Notifys the PropertyChanged event alerting the WPF Framework to update the UI.</summary>
        /// <param name="propertyNames">The names of the properties to update in the UI.</param>
        protected void NotifyPropertyChanged(params string[] propertyNames)
        {
            if (PropertyChanged != null)
            {
                foreach (string propertyName in propertyNames)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                }
            }
        }

        /// <summary>Notifys the PropertyChanged event alerting the WPF Framework to update the UI.</summary>
        /// <param name="propertyName">The optional name of the property to update in the UI. If this is left blank, the name will be taken from the calling member via the CallerMemberName attribute.</param>
        protected virtual void NotifyPropertyChanged([CallerMemberName]string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion INPC Members

        #region Click

        private void BtnAttack_Click(object sender, RoutedEventArgs e)
        {
            // TODO Implement attacking others from the Members Page.
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e) => GameState.GoBack();

        private async void BtnExpel_Click(object sender, RoutedEventArgs e)
        {
            if (await GameState.DatabaseInteraction.MemberLeavesGuild(_selectedUser, GameState.CurrentGuild))
                await GameState.DatabaseInteraction.SendMessage(new Message(0, GameState.CurrentGuild.Master, _selectedUser.Name, $"You have been expelled from the guild {GameState.CurrentGuild.Name}.", DateTime.UtcNow, true));
        }

        private void BtnMessage_Click(object sender, RoutedEventArgs e)
        {
            // TODO Implement messaging.
        }

        private void LstMembers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LstMembers.SelectedIndex >= 0)
            {
                _selectedUser = Users[LstMembers.SelectedIndex];
                switch (_previousPage)
                {
                    case "Game":
                        BtnAttack.IsEnabled = _selectedUser.Alive && _selectedUser.CurrentLocation == SleepLocation.Streets;
                        break;

                    case "Guild":
                        //You shouldn't attack your guildmates, and if you're not the master, you can't expel someone.
                        break;

                    case "Guild Manage":
                        BtnExpel.IsEnabled = true;
                        break;

                    case "Inn":
                        BtnAttack.IsEnabled = _selectedUser.Alive;
                        break;
                }
            }
            else
            {
                BtnAttack.IsEnabled = false;
                BtnExpel.IsEnabled = false;
            }
            BtnMessage.IsEnabled = LstMembers.SelectedIndex >= 0;
        }

        #endregion Click

        #region Page-Manipulation Methods

        public MembersPage(Page previousPage)
        {
            InitializeComponent();
            if (previousPage is GamePage)
            {
                _previousPage = "Game";
                Users = new List<User>(GameState.AllUsers);
                Users.Remove(GameState.CurrentUser);
            }
            else if (previousPage is GuildPage)
            {
                _previousPage = "Guild";
                Users = GameState.AllUsers.Where(user => GameState.CurrentGuild.Members.Contains(user.Name)).ToList();
            }
            else if (previousPage is GuildManagePage)
            {
                _previousPage = "Guild Manage";
                BtnExpel.Visibility = Visibility.Visible;
                Users = GameState.AllUsers.Where(user => GameState.CurrentGuild.Members.Contains(user.Name)).ToList();
            }
            else if (previousPage is InnPage)
            {
                _previousPage = "Inn";
                _users = GameState.AllUsers.Where(user => user.CurrentLocation == SleepLocation.Inn).ToList();
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LstMembers.ItemsSource = Users;
            if (LstMembers.Items.Count > 0)
                LstMembers.SelectedIndex = 0;
        }

        #endregion Page-Manipulation Methods
    }
}