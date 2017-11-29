using System.ComponentModel;

namespace Assassin.Classes.Items
{
    public class Item : IItem, INotifyPropertyChanged
    {
        private string _name;
        private int _value;
        private bool _hidden;

        #region Properties

        /// <summary>Name of Item</summary>
        public string Name
        {
            get => _name;
            set { _name = value; OnPropertyChanged("Name"); }
        }

        /// <summary>Value of Item</summary>
        public int Value
        {
            get => _value;
            set { _value = value; OnPropertyChanged("Value"); }
        }

        /// <summary>Is Item hidden from sale?</summary>
        public bool Hidden
        {
            get => _hidden;
            set { _hidden = value; OnPropertyChanged("Hidden"); }
        }

        #endregion Properties

        public override string ToString() => Name;

        #region Data-Binding

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string property) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));

        #endregion Data-Binding
    }
}