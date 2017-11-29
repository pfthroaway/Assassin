using Assassin.Classes;
using System.Windows;

namespace Assassin.Pages.Admin
{
    /// <summary>Interaction logic for AdminEnemiesPage.xaml</summary>
    public partial class AdminEnemiesPage
    {
        public AdminEnemiesPage() => InitializeComponent();

        private void AdminEnemiesPage_OnLoaded(object sender, RoutedEventArgs e) => GameState.CalculateScale(Grid);
    }
}