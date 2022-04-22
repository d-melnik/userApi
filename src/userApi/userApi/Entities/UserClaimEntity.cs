namespace userApi.Entities;

public class UserClaimEntity
{
    public int Id { get; set; }
    public int UserId { get; set; } //links to user id
    public UserEntity User { get; set; }
    
    public int ClaimId { get; set; }//links to claim id
    public ClaimEntity Claim { get; set; }
}