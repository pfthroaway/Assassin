using System.ComponentModel;

namespace Assassin
{
    public class Drink : INotifyPropertyChanged
    {
        private string _name;
        private int _restoreThirst, _value;

        #region Properties

        public string Name
        {
            get { return _name; }
            set { _name = value; OnPropertyChanged("Name"); }
        }

        public int RestoreThirst
        {
            get { return _restoreThirst; }
            set { _restoreThirst = value; OnPropertyChanged("RestoreThirst"); }
        }

        public int Value
        {
            get { return _value; }
            set { _value = value; OnPropertyChanged("Value"); }
        }

        #endregion Properties

        #region Data-Binding

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        #endregion Data-Binding

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the Drink class.
        /// </summary>
        internal Drink()
        {
            Name = "";
            RestoreThirst = 0;
            Value = 0;
        }

        /// <summary>
        /// Initializes a new instance of the Drink class using Property values.
        /// </summary>
        /// <param name="name">Name of Drink</param>
        /// <param name="restoreThirst">Amount of Thirst restored by Drink</param>
        /// <param name="value">Gold value of Drink</param>
        internal Drink(string name, int restoreThirst, int value)
        {
            Name = name;
            RestoreThirst = restoreThirst;
            Value = value;
        }

        /// <summary>
        /// Initializes a new instance of the Drink class using another Drink.
        /// </summary>
        /// <param name="otherDrink">Drink to replace this instance</param>
        internal Drink(Drink otherDrink)
        {
            Name = otherDrink.Name;
            RestoreThirst = otherDrink.RestoreThirst;
            Value = otherDrink.Value;
        }

        #endregion Constructors
    }
}