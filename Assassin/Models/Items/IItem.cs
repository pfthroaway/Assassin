namespace Assassin.Models.Items
{
    internal interface IItem
    {
        #region Properties

        /// <summary>Name of Item</summary>
        string Name { get; set; }

        /// <summary>Value of Item</summary>
        int Value { get; set; }

        /// <summary>Is Item hidden from sale?</summary>
        bool Hidden { get; set; }

        #endregion Properties
    }
}