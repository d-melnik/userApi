using System;
using System.Collections.Generic;
using System.Linq;
using KellermanSoftware.CompareNetObjects;
using userApi.Helpers;
using userApi.Models.Users;
using userApi.Services.Users;
using UserApiDbClient.DbContext.Configuration;
using UserApiDbClient.Entities;
using WebApi.Models.Users;
using Xunit;

namespace userApiTests.Services;

[Collection("Sequential")]
public class UserServiceTest : BaseTest
{
    private readonly UserService userService;
    private readonly CompareLogic compareLogic;

    public UserServiceTest()
    {
       userService = new UserService(Context, new JwtUtils(GetDefaultConfig()), GetDefaultMapper());
        compareLogic = new CompareLogic(new ComparisonConfig()
        {
            MaxMillisecondsDateDifference = 10000,
            MembersToIgnore = new List<string> { nameof(UserEntity.PasswordHash) },
        });
    }

    [Fact]
    public void GetAllTest()
    {
        IList<UserEntity> userEntities = userService.GetAll().ToList();
        Assert.NotNull(userEntities);
        Assert.NotEmpty(userEntities);
        Assert.Equal(userEntities.Count, UserConfiguration.CreateDefaultUsers().Count);

        IList<UserEntity> sortedListCurrent = userEntities.OrderBy(u => u.Id).ToList();
        IList<UserEntity> sortedListExpected = UserConfiguration.CreateDefaultUsers().OrderBy(u => u.Id).ToList();
     
        for (int userI = 0; userI < userEntities.Count; userI++)
        {
            var result = compareLogic.Compare(sortedListCurrent[userI], sortedListExpected[userI]);
            Assert.True(result.AreEqual, result.DifferencesString);
        }
    }
    
    [Fact]
    public void GetExistingUser_UserFound()
    {
        UserEntity existingUser = UserConfiguration.CreateDefaultUsers().First();
        UserEntity userEntity = userService.GetById(existingUser.Id);

        var result = compareLogic.Compare(existingUser, userEntity);
        Assert.True(result.AreEqual, result.DifferencesString);
    }
    
    [Fact]
    public void GetNotExistingUser_ExceptionThrown()
    {
        var exception = Assert.Throws<KeyNotFoundException>(() => userService.GetById(-999));
        Assert.Contains("not found", exception.Message.ToLower());
    }

    [Fact]
    public void RegisterUserValidModel_UserCreated()
    {
        var userRegisterRequest = new RegisterRequest()
        {
            FirstName = "Test First",
            LastName = "Test Last",
            Email = "test@test.com",
            Password = "Test123",
        };
        
        userService.Register(userRegisterRequest);
        
        Assert.Equal(UserConfiguration.CreateDefaultUsers().Count + 1, Context.Users.Count());
        UserEntity? createdUser = Context.Users.FirstOrDefault(u =>
            u.Email.Equals(userRegisterRequest.Email, StringComparison.InvariantCulture));
        
        Assert.NotNull(createdUser);
        Assert.Equal(userRegisterRequest.FirstName, createdUser.FirstName);
        Assert.Equal(userRegisterRequest.LastName, createdUser.LastName);
        Assert.NotNull(createdUser.PasswordHash);
    }
    
    [Fact]
    public void RegisterUserExistingUser_ExceptionThrown()
    {
        var userRegisterRequest = new RegisterRequest()
        {
            FirstName = "Test First",
            LastName = "Test Last",
            Email = UserConfiguration.CreateDefaultUsers().First().Email,
            Password = "Test123",
        };
        
        var exception = Assert.Throws<InvalidOperationException>(() => userService.Register(userRegisterRequest));
        Assert.Contains("already taken", exception.Message.ToLower());
    }
    
    [Fact]
    public void RegisterUserEmptyPassword_ExceptionThrown()
    {
        var userRegisterRequest = new RegisterRequest()
        {
            FirstName = "Test First",
            LastName = "Test Last",
            Email = "test@test.com",
            Password = "",
        };
        
        var exception = Assert.Throws<ArgumentException>(() => userService.Register(userRegisterRequest));
        Assert.Contains("should not be null or empty", exception.Message.ToLower());
    }
    
    [Fact]
    public void UpdateUserValidModel_UserCreated()
    {
        var userRegisterRequest = new UpdateRequest()
        {
            FirstName = "Test First Upd",
            LastName = "Test Last Upd",
            Email = "testUpd@test.com",
            Password = "Test123 Upd",
        };
        
        userService.Update(UserConfiguration.AdminUserId, userRegisterRequest);
        
        Assert.Equal(UserConfiguration.CreateDefaultUsers().Count, Context.Users.Count());
        UserEntity? updatedUser = Context.Users.FirstOrDefault(u =>
            u.Email.Equals(userRegisterRequest.Email, StringComparison.InvariantCulture));
        
        Assert.NotNull(updatedUser);
        Assert.Equal(userRegisterRequest.FirstName, updatedUser.FirstName);
        Assert.Equal(userRegisterRequest.LastName, updatedUser.LastName);
        Assert.NotNull(updatedUser.PasswordHash);
    }
    
    [Fact]
    public void UpdateUserUsedEmail_ExceptionThrown()
    {
        var userRegisterRequest = new UpdateRequest()
        {
            FirstName = "Test First",
            LastName = "Test Last",
            Email = UserConfiguration.CreateDefaultUsers()[1].Email,
        };
        
        var exception = Assert.Throws<InvalidOperationException>(() => userService.Update(UserConfiguration.AdminUserId, userRegisterRequest));
        Assert.Contains("is already taken", exception.Message.ToLower());
    }
    
    [Fact]
    public void AuthenticateUserExistingEmail_TokenGenerated()
    {
        var sampleUser = UserConfiguration.CreateDefaultUsers().First();
        var authRequest = new AuthenticateRequest()
        {
            Email = sampleUser.Email,
            Password = UserConfiguration.defaultPwd
        };

        AuthenticateResponse response = userService.Authenticate(authRequest);
        Assert.NotNull(response);
        Assert.NotEmpty(response.Token);
        Assert.Equal(response.FirstName, sampleUser.FirstName);
        Assert.Equal(response.LastName, sampleUser.LastName);
        Assert.Equal(response.Email, sampleUser.Email);
    }
    
    [Fact]
    public void AuthenticateUserNotExistingEmail_Exception()
    {
        var authRequest = new AuthenticateRequest()
        {
            Email = "Bla",
            Password ="Bla"
        };

        var exception = Assert.Throws<InvalidOperationException>(() => userService.Authenticate(authRequest));
        Assert.Contains("Username or password is incorrect", exception.Message);

    }
    
    [Fact]
    public void AuthenticateUserInvalidPassword_Exception()
    {
        var authRequest = new AuthenticateRequest()
        {
            Email = UserConfiguration.CreateDefaultUsers().First().Email,
            Password ="Bla"
        };

        var exception = Assert.Throws<InvalidOperationException>(() => userService.Authenticate(authRequest));
        Assert.Contains("Username or password is incorrect", exception.Message);
    }
    
    [Fact]
    public void DeleteExistingUser_UserDeleted()
    {
        int idForDeletion = UserConfiguration.CreateDefaultUsers().First().Id;
        userService.Delete(idForDeletion);
        Assert.Equal(UserConfiguration.CreateDefaultUsers().Count -1, Context.Users.Count());
        UserEntity? deletedUser = Context.Users.FirstOrDefault(u => u.Id == idForDeletion);
        Assert.Null(deletedUser);
    }
    
    [Fact]
    public void DeleteNonExistingUser_Exception()
    {
        var exception = Assert.Throws<KeyNotFoundException>(() => userService.Delete(-999));
        Assert.Contains("not found", exception.Message);
    }
}