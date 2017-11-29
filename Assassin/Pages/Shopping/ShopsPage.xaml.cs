using System.ComponentModel;
using System.Windows;

namespace Assassin.Pages.Shopping
{
    /// <summary>
    /// Interaction logic for ShopsPage.xaml
    /// </summary>
    public partial class ShopsPage
    {
        internal City.GamePage RefToGamePage { get; set; }

        #region Button-Click Methods

        private void BtnArmory_Click(object sender, RoutedEventArgs e)
        {
        }

        private void BtnGeneralStore_Click(object sender, RoutedEventArgs e)
        {
        }

        private void BtnWeapons_Click(object sender, RoutedEventArgs e)
        {
        }

        private void BtnThieves_Click(object sender, RoutedEventArgs e)
        {
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
        }

        #endregion Button-Click Methods

        #region Page-Manipulation Methods

        public ShopsPage()
        {
            InitializeComponent();
            TxtShops.Text = "You enter the shopping district.";
        }

        private void windowShops_Closing(object sender, CancelEventArgs e)
        {
        }

        #endregion Page-Manipulation Methods
    }
}