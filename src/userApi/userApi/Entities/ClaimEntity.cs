using System.Collections.Generic;

namespace userApi.Entities;

public class ClaimEntity
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    
    public List<UserClaimEntity> UserClaim { get; set; }
}