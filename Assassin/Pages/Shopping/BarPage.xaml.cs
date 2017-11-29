using Assassin.Classes;

namespace Assassin.Pages.Shopping
{
    /// <summary>Interaction logic for BarPage.xaml</summary>
    public partial class BarPage
    {
        public BarPage() => InitializeComponent();

        private void Page_Loaded(object sender, System.Windows.RoutedEventArgs e) => GameState.CalculateScale(Grid);
    }
}