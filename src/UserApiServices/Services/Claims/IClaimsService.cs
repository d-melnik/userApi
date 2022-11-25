using UserApiDbClient.Entities;
using UserApiServices.Models.Roles;

namespace UserApiServices.Services.Claims;

public interface IClaimsService
{
    IEnumerable<ClaimEntity> GetAll();
    ClaimEntity GetById(int id);
    ClaimEntity Create(ChangeClaimRequest request);
    void Update(int id, ChangeClaimRequest request);
    void Delete(int id);
}