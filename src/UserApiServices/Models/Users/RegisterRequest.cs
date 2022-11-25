using System.ComponentModel.DataAnnotations;

namespace UserApiServices.Models.Users
{
    public class RegisterRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Password minimum length is 6")]
        [MaxLength(40, ErrorMessage = "Password maximum length is 40")]
        
        public string Password { get; set; }
    }
}