namespace Assassin.Classes.Entities
{
    /// <summary>Represents someone who offers the User a job.</summary>
    internal class Employer
    {
        #region Properties

        /// <summary>Age of the <see cref="Employer"/>.</summary>
        public string Age { get; set; }

        /// <summary>Gender of the <see cref="Employer"/>.</summary>
        public string Gender { get; set; }

        /// <summary>Prefix of the <see cref="Employer"/>.</summary>
        public string Prefix { get; set; }

        #endregion Properties

        public override string ToString() => $"{Prefix} {Age} {Gender}";

        #region Constructors

        /// <summary>Initializes a default instance of <see cref="Employer"/>.</summary>
        public Employer()
        {
        }

        /// <summary>Initializes an instance of <see cref="Employer"/> by assigning Properties.</summary>
        /// <param name="prefix">A(n)</param>
        /// <param name="age">Age of Employer</param>
        /// <param name="gender">Gender of Employer</param>
        public Employer(string prefix, string age, string gender)
        {
            Prefix = prefix;
            Age = age;
            Gender = gender;
        }

        #endregion Constructors
    }
}