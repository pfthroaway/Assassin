﻿using System.ComponentModel;

namespace Assassin.Classes.Items
{
    /// <summary>Represents an <see cref="Item"/> in the game.</summary>
    public abstract class Item : IItem, INotifyPropertyChanged
    {
        private string _name;
        private int _value;
        private bool _hidden;

        #region Properties

        /// <summary>Name of <see cref="Item"/>.</summary>
        public string Name
        {
            get => _name;
            set { _name = value; OnPropertyChanged("Name"); }
        }

        /// <summary>Value of <see cref="Item"/>.</summary>
        public int Value
        {
            get => _value;
            set
            {
                _value = value;
                OnPropertyChanged("Value");
                OnPropertyChanged("ValueToString");
                OnPropertyChanged("ValueToStringWithText");
                OnPropertyChanged("SellValue");
                OnPropertyChanged("SellValueToString");
                OnPropertyChanged("SellValueToStringWithText");
            }
        }

        /// <summary>Is Item hidden from sale?</summary>
        public bool Hidden
        {
            get => _hidden;
            set { _hidden = value; OnPropertyChanged("Hidden"); }
        }

        #endregion Properties

        #region Helper Properties

        /// <summary>Sell Value of <see cref="Item"/>.</summary>
        public int SellValue => Value / 2;

        /// <summary>Sell Value of <see cref="Item"/>, formatted.</summary>
        public string SellValueToString => SellValue.ToString("N0");

        /// <summary>Value of <see cref="Item"/>, formatted with text.</summary>
        public string SellValueToStringWithText => $"Sell Value: {SellValueToString}";

        /// <summary>Value of <see cref="Item"/>, formatted.</summary>
        public string ValueToString => Value.ToString("N0");

        /// <summary>Value of <see cref="Item"/>, formatted with text.</summary>
        public string ValueToStringWithText => $"Value: {ValueToString}";

        #endregion Helper Properties

        #region Overrides

        public override string ToString() => Name;

        #endregion Overrides

        #region Data-Binding

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string property) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));

        #endregion Data-Binding
    }
}