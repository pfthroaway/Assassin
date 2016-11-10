using System.ComponentModel;

namespace Assassin
{
    public class Food : INotifyPropertyChanged
    {
        private string _name;
        private int _restoreHunger, _value;

        #region Properties

        public string Name
        {
            get { return _name; }
            set { _name = value; OnPropertyChanged("Name"); }
        }

        public int RestoreHunger
        {
            get { return _restoreHunger; }
            set { _restoreHunger = value; OnPropertyChanged("RestoreHunger"); }
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
        /// Initializes a new instance of the Food class.
        /// </summary>
        internal Food()
        {
            Name = "";
            RestoreHunger = 0;
            Value = 0;
        }

        /// <summary>
        /// Initializes a new instance of the Food class using Property values.
        /// </summary>
        /// <param name="name">Name of Food</param>
        /// <param name="restoreHunger">Amount of Hunger restored by Food</param>
        /// <param name="value">Gold value of Food</param>
        internal Food(string name, int restoreHunger, int value)
        {
            Name = name;
            RestoreHunger = restoreHunger;
            Value = value;
        }

        /// <summary>
        /// Initializes a new instance of the Food class using another Food.
        /// </summary>
        /// <param name="otherFood">Food to replace this instance</param>
        internal Food(Food otherFood)
        {
            Name = otherFood.Name;
            RestoreHunger = otherFood.RestoreHunger;
            Value = otherFood.Value;
        }

        #endregion Constructors
    }
}