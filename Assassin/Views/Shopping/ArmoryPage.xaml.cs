using Assassin.Models;
using Assassin.Models.Entities;
using Assassin.Models.Items;
using Extensions;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace Assassin.Views.Shopping
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
                NotifyPropertyChanged(nameof(SelectedArmor));
                GrpSelected.DataContext = SelectedArmor;
            }
        }

        #region Data-Binding

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

        /// <summary>Updates the Page's binding.</summary>
        private void UpdateBinding()
        {
            DataContext = GameState.CurrentUser;
            GrpCurrent.DataContext = GameState.CurrentUser.Armor;
            GrpSelected.DataContext = SelectedArmor;
        }

        #endregion Data-Binding

        #region Purchase/Sell

        /// <summary>Checks whether the currently selected <see cref="Armor"/> can be purchased.</summary>
        private void CheckPurchase() => BtnPurchase.IsEnabled = SelectedArmor != new Armor() && SelectedArmor != GameState.CurrentUser.Armor && GameState.CurrentUser.GoldOnHand >= SelectedArmor.Value;

        /// <summary>Checks whether the currently equipped <see cref="Armor"/> can be sold.</summary>
        private void CheckSell() => BtnSell.IsEnabled = GameState.CurrentUser.Armor.Value > 0;

        /// <summary>Asks if the current <see cref="User"/>'s wants to sell their <see cref="Armor"/>.</summary>
        private bool AskSell() => GameState.YesNoNotification($"Art thou sure thou want to sell thy {GameState.CurrentUser.Armor} for { GameState.CurrentUser.Armor.SellValueToString} gold?", "Assassin");

        /// <summary>Sells the <see cref="User"/>'s current <see cref="Armor"/>.</summary>
        private void Sell()
        {
            Functions.AddTextToTextBox(TxtArmor, $"Thou hast sold thy {GameState.CurrentUser.Armor.Name} for {GameState.CurrentUser.Armor.SellValueToString} gold.");
            GameState.CurrentUser.GoldOnHand += GameState.CurrentUser.Armor.SellValue;
            GameState.CurrentUser.Armor = GameState.AllArmor.Find(armor => armor.Name == "Clothes");
            CheckSell();
            UpdateBinding();
        }

        /// <summary>Purchases the selected <see cref="Armor"/>.</summary>
        private void Purchase()
        {
            Functions.AddTextToTextBox(TxtArmor, $"Thou hast purchased the {SelectedArmor.Name} for {SelectedArmor.ValueToString} gold.");
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
            if (AskSell())
                Sell();
        }

        private async void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            GameState.GoBack();
            await GameState.DatabaseInteraction.SaveUser(GameState.CurrentUser);
        }

        #endregion Button-Click Methods

        #region Page-Manipulation Methods

        public ArmoryPage()
        {
            InitializeComponent();
            TxtArmor.Text = "Thou hast entered The Armoury.The armorer greets you.\n\n" +
                "\"Welcome to The Armoury! We deal only in the finest of armors.\"";
            _allArmor = GameState.AllArmor.FindAll(armor => !armor.Hidden);
            LstArmor.ItemsSource = _allArmor;
            LstArmor.SelectedIndex = 0;
        }

        private void LstArmor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedArmor = LstArmor.SelectedIndex >= 0 ? (Armor)LstArmor.SelectedItem : new Armor();
            CheckPurchase();
            CheckSell();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e) => UpdateBinding();

        #endregion Page-Manipulation Methods
    }
}