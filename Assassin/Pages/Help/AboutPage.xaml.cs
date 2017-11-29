using Assassin.Classes;
using System.Windows;

namespace Assassin.Pages.Help
{
    /// <summary>Interaction logic for AboutPage.xaml</summary>
    public partial class AboutPage
    {
        private void BtnBack_Click(object sender, RoutedEventArgs e) => GameState.GoBack();

        #region Page-Manipulation Methods

        public AboutPage()
        {
            InitializeComponent();
            TxtAbout.Text = "Assassin v1.0\n" +
                "Based on the BBS game created by Kevin MacFarland\n" +
                "Copyright © 1990-1995\n" +
                "Recreated for Windows\n" +
                "Copyright © 2008-2017";
        }

        #endregion Page-Manipulation Methods
    }
}