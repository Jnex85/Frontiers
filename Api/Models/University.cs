using System.ComponentModel.DataAnnotations;

namespace Api.Models
{
    public class University
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public int Score { get; set; }
    }
}