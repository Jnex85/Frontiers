using System.ComponentModel.DataAnnotations;

namespace FrontiersApp.Data
{
    public class UserModel
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public int NumberOfPublications { get; set; }
        public string UniversityName { get; set; }
        public bool Reviewer { get; set; }
    }
}