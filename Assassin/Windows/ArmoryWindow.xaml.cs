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
    /// Interaction logic for ArmoryWindow.xaml
    /// </summary>
    public partial class ArmoryWindow : INotifyPropertyChanged
    {
        #region Data-Binding

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        #endregion Data-Binding

        #region Button-Click Methods

        private void btnPurchase_Click(object sender, RoutedEventArgs e)
        {
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
        }

        private void btnSell_Click(object sender, RoutedEventArgs e)
        {
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
        }

        #endregion Button-Click Methods

        #region Window-Manipulation Methods

        public ArmoryWindow()
        {
            InitializeComponent();
        }

        private void lstArmor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void windowArmory_Closing(object sender, CancelEventArgs e)
        {
        }

        #endregion Window-Manipulation Methods
    }
}