using Assassin.Classes;
using System.Windows;

namespace Assassin.Pages.Admin
{
    /// <summary>Interaction logic for AdminGuildPage.xaml</summary>
    public partial class AdminGuildPage
    {
        public AdminGuildPage() => InitializeComponent();

        private void AdminGuildPage_OnLoaded(object sender, RoutedEventArgs e) => GameState.CalculateScale(Grid);
    }
}