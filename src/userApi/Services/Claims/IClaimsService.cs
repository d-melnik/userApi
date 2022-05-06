using System.Collections.Generic;
using userApi.Entities;
using userApi.Models.Roles;
using WebApi.Models.Users;

namespace userApi.Services.Claims;

public interface IClaimsService
{
    IEnumerable<ClaimEntity> GetAll();
    ClaimEntity GetById(int id);
    ClaimEntity Create(ChangeClaimRequest request);
    void Update(int id, ChangeClaimRequest request);
    void Delete(int id);
}