using System;
using System.ComponentModel;
using System.Windows;

namespace Assassin
{
    /// <summary>
    /// Interaction logic for AboutWindow.xaml
    /// </summary>
    public partial class AboutWindow : Window
    {
        private string nl = Environment.NewLine;
        internal MainWindow RefToMainWindow { get; set; }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        #region Window-Manipulation Methods

        public AboutWindow()
        {
            InitializeComponent();
            txtAbout.Text = "Assassin v1.0" + nl + nl +
                "Based on the BBS game created by Kevin MacFarland" + nl +
                "Copyright © 1990-1995" + nl + nl +
                "Recreated for Windows" + nl +
                "Copyright © 2008-2016";
        }

        private void windowAbout_Closing(object sender, CancelEventArgs e)
        {
            RefToMainWindow.Show();
        }

        #endregion Window-Manipulation Methods
    }
}