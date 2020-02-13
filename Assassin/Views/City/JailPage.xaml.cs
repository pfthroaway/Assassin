using Assassin.Models;
using System.ComponentModel;

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

        protected void OnPropertyChanged(string property) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));

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