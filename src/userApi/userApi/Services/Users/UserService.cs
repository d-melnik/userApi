using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using userApi.DbContext;
using userApi.Entities;
using userApi.Helpers;
using userApi.Models.Users;
using WebApi.Models.Users;

namespace userApi.Services.Users
{
    public class UserService : IUserService
    {
        private readonly DataContext context;
        private readonly IJwtUtils jwtUtils;
        private readonly IMapper mapper;

        public UserService(
            DataContext context,
            IJwtUtils jwtUtils,
            IMapper mapper)
        {
            this.context = context;
            this.jwtUtils = jwtUtils;
            this.mapper = mapper;
        }

        public AuthenticateResponse Authenticate(AuthenticateRequest model)
        {
            var user = context.Users.SingleOrDefault(x => x.Email == model.Email);

            // validate
            if (user == null || !BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash))
                throw new InvalidOperationException("Username or password is incorrect");

            // authentication successful
            var response = mapper.Map<AuthenticateResponse>(user);
            response.Token = jwtUtils.GenerateToken(user);
            return response;
        }

        public IEnumerable<UserEntity> GetAll()
        {
            return context.Users;
        }

        public UserEntity GetById(int id)
        {
            return getUser(id);
        }

        public void Register(RegisterRequest model)
        {
            // validate
            if (context.Users.Any(x => x.Email == model.Email))
                throw new InvalidOperationException("Username '" + model.Email + "' is already taken");

            // map model to new user object
            var user = mapper.Map<UserEntity>(model);

            // hash password
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password);

            // save user
            context.Users.Add(user);
            context.SaveChanges();
        }

        public void Update(int id, UpdateRequest model)
        {
            var user = getUser(id);

            // validate
            if (model.Username != user.Email && context.Users.Any(x => x.Email == model.Username))
                throw new InvalidOperationException("Username '" + model.Username + "' is already taken");

            // hash password if it was entered
            if (!string.IsNullOrEmpty(model.Password))
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password);

            // copy model to user and save
            mapper.Map(model, user);
            context.Users.Update(user);
            context.SaveChanges();
        }

        public void Delete(int id)
        {
            var user = getUser(id);
            context.Users.Remove(user);
            context.SaveChanges();
        }

        // helper methods

        private UserEntity getUser(int id)
        {
            var user = context.Users.Find(id);
            if (user == null) throw new KeyNotFoundException("User not found");
            return user;
        }
    }
}