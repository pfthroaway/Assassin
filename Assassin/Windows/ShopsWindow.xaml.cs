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
    /// Interaction logic for ShopsWindow.xaml
    /// </summary>
    public partial class ShopsWindow 
    {
        internal GameWindow RefToGameWindow { get; set; }
        private readonly string nl = Environment.NewLine;

        /// <summary>
        /// Adds text to the txtGame Textbox.
        /// </summary>
        /// <param name="newText">Text to be added</param>
        internal void AddTextTT(string newText)
        {
            txtShops.Text += nl + nl + newText;
            txtShops.Focus();
            txtShops.CaretIndex = txtShops.Text.Length;
            txtShops.ScrollToEnd();
        }

        #region Button-Click Methods

        private void btnArmory_Click(object sender, RoutedEventArgs e)
        {
        }

        private void btnGeneralStore_Click(object sender, RoutedEventArgs e)
        {
        }

        private void btnWeapons_Click(object sender, RoutedEventArgs e)
        {
        }

        private void btnThieves_Click(object sender, RoutedEventArgs e)
        {
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
        }

        #endregion Button-Click Methods

        #region Window-Manipulation Methods

        public ShopsWindow()
        {
            InitializeComponent();
            txtShops.Text = "You enter the shopping district.";
        }

        private void windowShops_Closing(object sender, CancelEventArgs e)
        {
        }

        #endregion Window-Manipulation Methods
    }
}