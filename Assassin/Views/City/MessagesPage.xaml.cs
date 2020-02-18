using Assassin.Models;
using Assassin.Models.Entities;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace Assassin.Views.City
{
    /// <summary>Interaction logic for MessagesPage.xaml</summary>
    public partial class MessagesPage : Page, INotifyPropertyChanged
    {
        //TODO Find a way to tell the User that they have new messages. Maybe save the last login time for the User and check if there are messages that were sent after that time. Maybe stick an unread Property on the message and in the database.

        private Message _currentMessage = new Message();
        private List<Message> _allMessages = new List<Message>();
        private int _index;

        #region Properties

        /// <summary>Index of currently selected <see cref="Message"/>.</summary>
        public int Index
        {
            get => _index;
            set
            {
                _index = value; BindLabels();
            }
        }

        public List<Message> AllMessages { get => _allMessages; set { _allMessages = value; BindLabels(); } }

        /// <summary>The <see cref="Message"/> currently being read.</summary>
        public Message CurrentMessage { get => _currentMessage; set { _currentMessage = value; BindLabels(); } }

        public string Count => $"{(AllMessages.Count > 0 ? Index + 1 : 0)} / {AllMessages.Count}";

        #endregion Properties

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

        private void BindLabels()
        {
            NotifyPropertyChanged(nameof(Index), nameof(CurrentMessage), nameof(AllMessages), nameof(Count));
            DataContext = CurrentMessage;
            LblCount.DataContext = this;
        }

        #endregion INPC Members

        /// <summary>Toggles all Buttons based on how many <see cref="Message"/>s exist.</summary>
        private void ToggleButtons()
        {
            BtnDelete.IsEnabled = AllMessages.Count > 0;
            BtnNext.IsEnabled = AllMessages.Count > 1;
            BtnPrevious.IsEnabled = AllMessages.Count > 1;
            BtnReply.IsEnabled = AllMessages.Count > 0 && !CurrentMessage.GuildMessage && GameState.AllUsers.Exists(user => user.Name == CurrentMessage.UserFrom);
        }

        #region Click

        private void BtnBack_Click(object sender, RoutedEventArgs e) => GameState.GoBack();

        private async void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (GameState.YesNoNotification("Are you sure you want to delete this message? This action cannot be undone.", "Assassin") && await GameState.DatabaseInteraction.DeleteMessage(CurrentMessage))
            {
                AllMessages.Remove(CurrentMessage);
                if (Index > 0)
                    Index--;
                CurrentMessage = AllMessages.Count > 0 ? AllMessages[Index] : new Message();
                BindLabels();
            }

            ToggleButtons();
        }

        private void BtnNew_Click(object sender, RoutedEventArgs e) => GameState.Navigate(new NewMessagePage(GameState.AllUsers));

        private void BtnNext_Click(object sender, RoutedEventArgs e)
        {
            if (Index == AllMessages.Count - 1)
                Index = 0;
            else
                Index++;
            CurrentMessage = AllMessages[Index];
            BindLabels();
            ToggleButtons();
        }

        private void BtnPrevious_Click(object sender, RoutedEventArgs e)
        {
            if (Index == 0)
                Index = AllMessages.Count - 1;
            else
                Index--;
            CurrentMessage = AllMessages[Index];
            BindLabels();
            ToggleButtons();
        }

        private void BtnReply_Click(object sender, RoutedEventArgs e) => GameState.Navigate(new NewMessagePage(new List<User> { GameState.AllUsers.Find(user => user.Name == CurrentMessage.UserFrom) }));

        #endregion Click

        #region Page-Manipulation Methods

        public MessagesPage()
        {
            InitializeComponent();
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            AllMessages = await GameState.DatabaseInteraction.LoadMessages(GameState.CurrentUser);
            if (AllMessages.Count > 0)
            {
                CurrentMessage = AllMessages[Index];
                ToggleButtons();
                BindLabels();
            }
        }

        #endregion Page-Manipulation Methods
    }
}