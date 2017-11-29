using Assassin.Classes;

namespace Assassin.Pages.Shopping
{
    /// <summary>Interaction logic for ThievesGuildPage.xaml</summary>
    public partial class ThievesGuildPage
    {
        public ThievesGuildPage() => InitializeComponent();

        private void Page_Loaded(object sender, System.Windows.RoutedEventArgs e) => GameState.CalculateScale(Grid);
    }
}