using UserApiDbClient.Entities;
using UserApiServices.Models.Users;

namespace UserApiServices.Services.Users;

public interface IUserService
{
    AuthenticateResponse Authenticate(AuthenticateRequest model);
    IEnumerable<UserEntity> GetAll();
    UserEntity GetById(int id);
    void Register(RegisterRequest model);
    void Update(int id, UpdateRequest model);
    void Delete(int id);
}