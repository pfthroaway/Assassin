using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Shapes;

namespace Assassin
{
    /// <summary>
    /// Interaction logic for JailWindow.xaml
    /// </summary>
    public partial class JailWindow : Window, INotifyPropertyChanged
    {
        internal GameWindow RefToGameWindow { get; set; }

        #region Data-Binding

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        #endregion Data-Binding

        #region Window-Manipulation Methods

        public JailWindow()
        {
            InitializeComponent();
        }

        private async void windowJail_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            await GameState.SaveUser(GameState.CurrentUser);
            RefToGameWindow.Show();
        }

        #endregion Window-Manipulation Methods
    }
}