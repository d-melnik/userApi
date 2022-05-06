using System.ComponentModel.DataAnnotations;

namespace userApi.Models.Roles
{
    public class ChangeClaimRequest
    {
        [Required]
        public string Name { get; set; }
        
        public string Description { get; set; }
    }
}