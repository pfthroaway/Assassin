using Assassin.Models;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Assassin.Views.City
{
    /// <summary>
    /// Interaction logic for JailPage.xaml
    /// </summary>
    public partial class JailPage : INotifyPropertyChanged
    {
        internal GamePage RefToGamePage { get; set; }

        #region Data-Binding

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

        #endregion Data-Binding

        #region Page-Manipulation Methods

        /// <summary>Closes the Page.</summary>
        private async void ClosePage()
        {
            GameState.GoBack();
            await GameState.SaveUser(GameState.CurrentUser);
        }

        public JailPage() => InitializeComponent();

        #endregion Page-Manipulation Methods
    }
}