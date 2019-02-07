using System.ComponentModel.DataAnnotations;

namespace BookStore.Api.Identity.Models
{
    public class User
    {
        public long Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Password { get; set; }
    }
}