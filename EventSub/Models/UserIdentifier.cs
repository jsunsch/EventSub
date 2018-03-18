using System.ComponentModel.DataAnnotations;

namespace EventSub.Models
{
    public class UserIdentifier : IUserIdentifier
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string LastName { get; set; }
    }
}