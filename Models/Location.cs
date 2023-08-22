using System.ComponentModel.DataAnnotations;

namespace BcDemocratize.Models
{
    public class Location
    {
        public int Id { get; set; }

        [Required]
        public string? City { get; set; }
    }
}
