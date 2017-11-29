using Assassin.Classes;

namespace Assassin.Pages.Shopping
{
    /// <summary>Interaction logic for MysticPage.xaml</summary>
    public partial class MysticPage
    {
        public MysticPage() => InitializeComponent();

        private void Page_Loaded(object sender, System.Windows.RoutedEventArgs e) => GameState.CalculateScale(Grid);
    }
}