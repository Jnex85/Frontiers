using System.ComponentModel.DataAnnotations;

namespace Api.Models
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public int NumberOfPublications { get; set; }
        [Required]
        public string UniversityName { get; set; }
    }
}