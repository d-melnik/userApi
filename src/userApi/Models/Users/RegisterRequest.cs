using System.ComponentModel.DataAnnotations;

namespace userApi.Models.Users
{
    public class RegisterRequest
    {
        [Required]
        public string Email { get; set; }
        
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}