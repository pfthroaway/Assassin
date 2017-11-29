using Assassin.Classes.Items;

namespace Assassin.Classes.Entities
{
    /// <summary>Represents all required information for every living entity.</summary>
    internal interface IEntity
    {
        #region Modifying Properties

        /// <summary>Name of this Entity.</summary>
        string Name { get; set; }

        /// <summary>Level of this Entity.</summary>
        int Level { get; set; }

        /// <summary>Entity's current amount of health.</summary>
        int CurrentEndurance { get; set; }

        /// <summary>Entity's maxmimum amount of health.</summary>
        int MaximumEndurance { get; set; }

        /// <summary>Amount of gold the Entity has in their possession.</summary>
        int GoldOnHand { get; set; }

        /// <summary>Entity's Armor.</summary>
        Armor Armor { get; set; }

        #endregion Modifying Properties

        #region Helper Properties

        /// <summary>Entity's current amount of health as opposed to their maximum health, formatted.</summary>
        string EnduranceToString { get; }

        /// <summary>Amount of gold the Entity has in their possession, formatted.</summary>
        string GoldOnHandToString { get; }

        /// <summary>Entity's skill at blocking.</summary>
        int Blocking { get; set; }

        /// <summary>Entity's skill at dodging and running away.</summary>
        int Slipping { get; set; }

        #endregion Helper Properties

        #region Health Manipulation

        string TakeDamage(int damage);

        #endregion Health Manipulation

        string ToString();
    }
}