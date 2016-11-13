namespace Assassin
{
    internal class Employer
    {
        private string _prefix, _age, _gender;

        #region Properties

        public string Age
        {
            get { return _age; }
            set { _age = value; }
        }

        public string Gender
        {
            get { return _gender; }
            set { _gender = value; }
        }

        public string Prefix
        {
            get { return _prefix; }
            set { _prefix = value; }
        }

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