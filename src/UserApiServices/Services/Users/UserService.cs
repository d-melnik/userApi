using AutoMapper;
using Microsoft.EntityFrameworkCore;
using UserApiDbClient.DbContext;
using UserApiDbClient.Entities;
using UserApiServices.Helpers;
using UserApiServices.Models.Users;

namespace UserApiServices.Services.Users
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
            var user = context.Users.Include(u => u.UserClaim).ThenInclude(uc => uc.Claim)
                .SingleOrDefault(x => x.Email == model.Email);
            bool IsValidPassword () => BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash);

            // validate
            if (user == null || IsValidPassword() == false)
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
            return GetUser(id);
        }

        public void Register(RegisterRequest model)
        {
            // validate
            if (context.Users.Any(x => x.Email == model.Email))
                throw new InvalidOperationException("Username '" + model.Email + "' is already taken");

            // map model to new user object
            var user = mapper.Map<UserEntity>(model);

            if (string.IsNullOrEmpty(model.Password))
            {
                throw new ArgumentException("Password should not be null or empty");
            }

            // hash password
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password);
            user.CreateDate = DateTime.UtcNow;
            user.UpdateDate = DateTime.UtcNow;

            // save user
            context.Users.Add(user);
            context.SaveChanges();
        }

        public void Update(int id, UpdateRequest model)
        {
            var user = GetUser(id);

            // validate
            if (model.Email != user.Email && context.Users.Any(x => x.Email == model.Email))
            {
                throw new InvalidOperationException("Username '" + model.Email + "' is already taken");
            }

            // hash password if it was entered
            if (!string.IsNullOrEmpty(model.Password))
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password);

            // copy model to user and save
            mapper.Map(model, user);
            user.UpdateDate = DateTime.UtcNow;
            context.Users.Update(user);
            context.SaveChanges();
        }

        public void Delete(int id)
        {
            var user = GetUser(id);
            context.Users.Remove(user);
            context.SaveChanges();
        }

        // helper methods

        private UserEntity GetUser(int id)
        {
            var user = context.Users.Find(id);
            if (user == null)
            {
                throw new KeyNotFoundException("User not found");
            }
            return user;
        }
    }
}