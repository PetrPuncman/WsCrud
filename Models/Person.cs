namespace WsCrud.Models
{
    /// <summary>
    /// Represents a person with an Id, Name, and Age.
    /// </summary>
    public class Person
    {
        /// <summary>
        /// Unique identifier (auto-assigned).
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Full name of the person.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Age in years.
        /// </summary>
        public int Age { get; set; }
    }
}