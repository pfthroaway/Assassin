using Assassin.Models;
using Assassin.Models.Items;
using Extensions;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace Assassin.Views.Shopping
{
    /// <summary>Interaction logic for BarPage.xaml</summary>
    public partial class BarPage
    {
        private Drink _selectedDrink = new Drink();
        private Food _selectedFood = new Food();

        #region Properties

        /// <summary>The currently selected <see cref="Drink"/>.</summary>
        public Drink SelectedDrink
        {
            get => _selectedDrink;
            set
            {
                _selectedDrink = value;
                NotifyPropertyChanged(nameof(SelectedDrink));
            }
        }

        /// <summary>The currently selected <see cref="Food"/>.</summary>
        public Food SelectedFood
        {
            get => _selectedFood;
            set
            {
                _selectedFood = value;
                NotifyPropertyChanged(nameof(SelectedFood));
            }
        }

        #endregion Properties

        #region INPC Members

        /// <summary>The event that is raised when a property that calls the NotifyPropertyChanged method is changed.</summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>Notifys the PropertyChanged event alerting the WPF Framework to update the UI.</summary>
        /// <param name="propertyNames">The names of the properties to update in the UI.</param>
        protected void NotifyPropertyChanged(params string[] propertyNames)
        {
            if (PropertyChanged != null)
            {
                foreach (string propertyName in propertyNames)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                }
            }
        }

        /// <summary>Notifys the PropertyChanged event alerting the WPF Framework to update the UI.</summary>
        /// <param name="propertyName">The optional name of the property to update in the UI. If this is left blank, the name will be taken from the calling member via the CallerMemberName attribute.</param>
        protected virtual void NotifyPropertyChanged([CallerMemberName]string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion INPC Members

        #region Click

        private void BtnBack_Click(object sender, RoutedEventArgs e) => GameState.GoBack();

        private void BtnPurchase_Click(object sender, RoutedEventArgs e)
        {
            if (RadDrinks.IsChecked != null && RadDrinks.IsChecked.Value)
            {
                GameState.CurrentUser.GoldOnHand -= SelectedDrink.Value;
                GameState.CurrentUser.Thirst -= SelectedDrink.RestoreThirst;
                Functions.AddTextToTextBox(TxtBar, $"You purchase the {SelectedDrink.Name} for {SelectedDrink.ValueToString} gold.");
                if (GameState.CurrentUser.Thirst < 0)
                {
                    GameState.CurrentUser.Thirst = 0;
                    Functions.AddTextToTextBox(TxtBar, "You don't need any more to drink right now.");
                }
            }
            else if (RadFood.IsChecked != null && RadFood.IsChecked.Value)
            {
                GameState.CurrentUser.GoldOnHand -= SelectedFood.Value;
                GameState.CurrentUser.Hunger -= SelectedFood.RestoreHunger;
                Functions.AddTextToTextBox(TxtBar, $"You purchase the {SelectedFood.Name} for {SelectedFood.ValueToString} gold.");
                if (GameState.CurrentUser.Hunger < 0)
                {
                    GameState.CurrentUser.Hunger = 0;
                    Functions.AddTextToTextBox(TxtBar, "You don't need any more to eat right now.");
                }
            }
            Functions.AddTextToTextBox(TxtBar, GameState.CurrentUser.DisplayHungerThirstText());
        }

        private void RadDrinks_Checked(object sender, RoutedEventArgs e) => CheckedChanged();

        private void RadFood_Checked(object sender, RoutedEventArgs e) => CheckedChanged();

        private void LstBar_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LstBar.SelectedIndex >= 0)
                BindLabels();
        }

        #endregion Click

        #region Page-Manipulation Methods

        /// <summary>Binds all labels appropriately.</summary>
        private void BindLabels()
        {
            DataContext = GameState.CurrentUser;

            if (RadDrinks.IsChecked != null && RadDrinks.IsChecked.Value)
            {
                SelectedDrink = GameState.AllDrinks[LstBar.SelectedIndex];
                GrpSelected.DataContext = SelectedDrink;
                BtnPurchase.IsEnabled = GameState.CurrentUser.GoldOnHand >= SelectedDrink.Value;
            }
            else if (RadFood.IsChecked != null && RadFood.IsChecked.Value)
            {
                SelectedFood = GameState.AllFood[LstBar.SelectedIndex];
                GrpSelected.DataContext = SelectedFood;
                BtnPurchase.IsEnabled = GameState.CurrentUser.GoldOnHand >= SelectedFood.Value;
            }
        }

        /// <summary>Displays the proper list if the radio buttons have changed value.</summary>
        private void CheckedChanged()
        {
            if (RadDrinks.IsChecked != null && RadDrinks.IsChecked.Value)
                LstBar.ItemsSource = GameState.AllDrinks;
            else if (RadFood.IsChecked != null && RadFood.IsChecked.Value)
                LstBar.ItemsSource = GameState.AllFood;

            LstBar.SelectedIndex = 0;
        }

        public BarPage() => InitializeComponent();

        private void Page_Loaded(object sender, RoutedEventArgs e) => RadDrinks.IsChecked = true;

        #endregion Page-Manipulation Methods
    }
}