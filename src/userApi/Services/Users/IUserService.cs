﻿using System.Collections.Generic;
using userApi.Models.Users;
using UserApiDbClient.Entities;
using WebApi.Models.Users;

namespace userApi.Services.Users;

public interface IUserService
{
    AuthenticateResponse Authenticate(AuthenticateRequest model);
    IEnumerable<UserEntity> GetAll();
    UserEntity GetById(int id);
    void Register(RegisterRequest model);
    void Update(int id, UpdateRequest model);
    void Delete(int id);
}