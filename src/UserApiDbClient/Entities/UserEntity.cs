using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace userApi.Entities
{
    public class UserEntity
    {
        public int Id { get; set; }
        
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }

        [JsonIgnore]
        public string PasswordHash { get; set; }
        
        public List<UserClaimEntity> UserClaim { get; set; }
    }
}