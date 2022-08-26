using System.Text.Json.Serialization;

namespace UserApiDbClient.Entities;

public class ClaimEntity
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    
    [JsonIgnore]
    public List<UserClaimEntity>? UserClaim { get; set; }
}