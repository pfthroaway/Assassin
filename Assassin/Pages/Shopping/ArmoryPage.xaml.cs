using Assassin.Classes;
using Assassin.Classes.Items;
using Extensions;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace Assassin.Pages.Shopping
{
    /// <summary>Interaction logic for ArmoryPage.xaml</summary>
    public partial class ArmoryPage : INotifyPropertyChanged
    {
        private Armor _selectedArmor = new Armor();
        private readonly List<Armor> _allArmor = new List<Armor>();

        /// <summary>The <see cref="Armor"/> that is currently selected.</summary>
        public Armor SelectedArmor
        {
            get => _selectedArmor;
            set
            {
                _selectedArmor = value;
                OnPropertyChanged("SelectedArmor");
                GrpSelected.DataContext = SelectedArmor;
            }
        }

        #region Data-Binding

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string property) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));

        private void UpdateBinding()
        {
            DataContext = GameState.CurrentUser;
            GrpCurrent.DataContext = GameState.CurrentUser.Armor;
            GrpSelected.DataContext = SelectedArmor;
        }

        #endregion Data-Binding

        /// <summary>Adds text to the TextBox.</summary>
        /// <param name="text">Text to be added</param>
        private void AddTextTt(string text) => Functions.AddTextToTextBox(TxtArmor, text);

        #region Purchase/Sell

        /// <summary>Checks whether the currently selected <see cref="Armor"/> can be purchased.</summary>
        private void CheckPurchase() => BtnPurchase.IsEnabled = SelectedArmor != new Armor() && SelectedArmor != GameState.CurrentUser.Armor && GameState.CurrentUser.GoldOnHand >= SelectedArmor.Value;

        /// <summary>Checks whether the currently equipped <see cref="Armor"/> can be sold.</summary>
        private void CheckSell() => BtnSell.IsEnabled = GameState.CurrentUser.Armor.Value > 0;

        /// <summary>Asks if the current <see cref="User"/>'s wants to sell their <see cref="Armor"/>.</summary>
        private bool AskSell() => GameState.YesNoNotification($"Art thou sure thou want to sell thy {GameState.CurrentUser.Armor} for { GameState.CurrentUser.Armor.SellValueToString} gold?", "Assassin");

        /// <summary>Sells the current <see cref="User"/>'s <see cref="Armor"/>.</summary>
        private void Sell()
        {
            AddTextTt($"Thou hast sold thy {GameState.CurrentUser.Armor} for {GameState.CurrentUser.Armor.SellValueToString} gold.");
            GameState.CurrentUser.GoldOnHand += GameState.CurrentUser.Armor.SellValue;
            GameState.CurrentUser.Armor = GameState.AllArmor.Find(armor => armor.Name == "Clothes");
            CheckSell();
            UpdateBinding();
        }

        /// <summary>Purchases the selected <see cref="Armor"/>.</summary>
        private void Purchase()
        {
            AddTextTt($"Thou hast purchased the {SelectedArmor} for {SelectedArmor.Value} gold.");
            GameState.CurrentUser.GoldOnHand -= SelectedArmor.Value;
            GameState.CurrentUser.Armor = SelectedArmor;
            CheckSell();
            CheckPurchase();
            UpdateBinding();
        }

        #endregion Purchase/Sell

        #region Button-Click Methods

        private void BtnPurchase_Click(object sender, RoutedEventArgs e)
        {
            if (GameState.CurrentUser.Armor.SellValue > 0)
            {
                if (AskSell())
                {
                    Sell();
                    Purchase();
                }
            }
            else
                Purchase();
        }

        private void BtnSell_Click(object sender, RoutedEventArgs e)
        {
            if (GameState.YesNoNotification($"Are you sure you want to sell your {GameState.CurrentUser.Armor} for {GameState.CurrentUser.Armor.SellValueToString} gold?", "Assassin"))
                Sell();
        }

        private async void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            GameState.GoBack();
            await GameState.SaveUser(GameState.CurrentUser);
        }

        #endregion Button-Click Methods

        #region Page-Manipulation Methods

        public ArmoryPage()
        {
            InitializeComponent();
            _allArmor = GameState.AllArmor.FindAll(armor => !armor.Hidden);
            LstArmor.ItemsSource = _allArmor;
            LstArmor.SelectedIndex = 0;
        }

        private void LstArmor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedArmor = LstArmor.SelectedIndex >= 0 ? (Armor)LstArmor.SelectedItem : new Armor();
            CheckPurchase();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateBinding();
            CheckSell();
        }

        #endregion Page-Manipulation Methods
    }
}