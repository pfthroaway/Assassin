using Assassin.Classes;

namespace Assassin.Pages.Shopping
{
    /// <summary>Interaction logic for PubPage.xaml</summary>
    public partial class PubPage
    {
        public PubPage() => InitializeComponent();

        private void Page_Loaded(object sender, System.Windows.RoutedEventArgs e) => GameState.CalculateScale(Grid);
    }
}