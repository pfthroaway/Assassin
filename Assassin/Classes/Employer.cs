namespace Assassin
{
    internal class Employer
    {
        #region Properties

        public string Age { get; set; }

        public string Gender { get; set; }

        public string Prefix { get; set; }

        #endregion Properties

        public override string ToString()
        {
            return Prefix + " " + Age + " " + Gender;
        }

        #region Constructors

        /// <summary>
        /// Initializes a default instance of Employer.
        /// </summary>
        public Employer()
        {
        }

        /// <summary>
        /// Initializes an instance of Employer by assigning Properties.
        /// </summary>
        /// <param name="prefix">A/an</param>
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