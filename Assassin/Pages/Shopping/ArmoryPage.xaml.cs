using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace Assassin.Pages.Shopping
{
    /// <summary>
    /// Interaction logic for ArmoryPage.xaml
    /// </summary>
    public partial class ArmoryPage : INotifyPropertyChanged
    {
        #region Data-Binding

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string property) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));

        #endregion Data-Binding

        #region Button-Click Methods

        private void BtnPurchase_Click(object sender, RoutedEventArgs e)
        {
        }

        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
        }

        private void BtnSell_Click(object sender, RoutedEventArgs e)
        {
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
        }

        #endregion Button-Click Methods

        #region Page-Manipulation Methods

        public ArmoryPage() => InitializeComponent();

        private void lstArmor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void windowArmory_Closing(object sender, CancelEventArgs e)
        {
        }

        #endregion Page-Manipulation Methods
    }
}