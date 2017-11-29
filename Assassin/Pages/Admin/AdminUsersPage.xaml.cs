using Assassin.Classes;
using System.Windows;

namespace Assassin.Pages.Admin
{
    /// <summary>Interaction logic for AdminUsersPage.xaml</summary>
    public partial class AdminUsersPage
    {
        public AdminUsersPage() => InitializeComponent();

        private void AdminUsersPage_OnLoaded(object sender, RoutedEventArgs e) => GameState.CalculateScale(Grid);
    }
}