using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace userApi.Entities;

public class ClaimEntity
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    
    [JsonIgnore]
    public List<UserClaimEntity> UserClaim { get; set; }
}