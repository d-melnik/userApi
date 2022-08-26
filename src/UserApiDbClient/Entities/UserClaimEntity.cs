namespace UserApiDbClient.Entities;

public class UserClaimEntity
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public UserEntity? User { get; set; }
    
    public int ClaimId { get; set; }
    public ClaimEntity? Claim { get; set; }
}