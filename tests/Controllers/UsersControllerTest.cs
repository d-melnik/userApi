using System;
using System.Collections.Generic;
using System.Linq;
using KellermanSoftware.CompareNetObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using userApi.Controllers;
using UserApiDbClient.DbContext.Configuration;
using UserApiDbClient.Entities;
using UserApiServices.Helpers;
using UserApiServices.Models.Users;
using UserApiServices.Services.Users;
using Xunit;

namespace userApiTests.Controllers;

[Collection("Sequential")]
public class UsersControllerTest : BaseTest
{
    private readonly UsersController usersController;
    private readonly CompareLogic compareLogic;

    public UsersControllerTest()
    {
        usersController = new UsersController(new UserService(Context,new JwtUtils(GetDefaultConfig()), GetDefaultMapper()));
        compareLogic = new CompareLogic(new ComparisonConfig()
        {
            MaxMillisecondsDateDifference = 10000,
            MembersToIgnore = new List<string> { nameof(UserEntity.PasswordHash) },
        });
    }

    [Fact]
    public void AuthenticateValidModel_200OK()
    {
        var sampleUser = UserConfiguration.CreateDefaultUsers().First();
        
        var authRequest = new AuthenticateRequest()
        {
            Email = UserConfiguration.CreateDefaultUsers().First().Email,
            Password = UserConfiguration.defaultPwd
        };
        var response = usersController.Authenticate(authRequest) as OkObjectResult;
        Assert.NotNull(response);
        AuthenticateResponse? convertedResponse = response.Value as AuthenticateResponse;
        Assert.NotNull(convertedResponse);
        Assert.NotEmpty(convertedResponse.Token);
        Assert.Equal(convertedResponse.FirstName, sampleUser.FirstName);
        Assert.Equal(convertedResponse.LastName, sampleUser.LastName);
        Assert.Equal(convertedResponse.Email, sampleUser.Email);
    }
    
    [Fact]
    public void RegisterValidModel_200OK()
    {
        var userRegisterRequest = new RegisterRequest()
        {
            FirstName = "Test First",
            LastName = "Test Last",
            Email = "test@test.com",
            Password = "Test123",
        };
        
        var response = usersController.Register(userRegisterRequest) as OkObjectResult;
        Assert.NotNull(response);
        
        Assert.Equal(UserConfiguration.CreateDefaultUsers().Count + 1, Context.Users.Count());
        UserEntity? createdUser = Context.Users.FirstOrDefault(u =>
            u.Email.Equals(userRegisterRequest.Email, StringComparison.InvariantCulture));
        
        Assert.NotNull(createdUser);
        Assert.Equal(userRegisterRequest.FirstName, createdUser.FirstName);
        Assert.Equal(userRegisterRequest.LastName, createdUser.LastName);
        Assert.NotNull(createdUser.PasswordHash);
    }
    
    [Fact]
    public void GetAll_AllUsersList()
    {
        OkObjectResult? response = usersController.GetAll() as OkObjectResult ;
        Assert.NotNull(response);

        IList<UserEntity> userEntities = (response.Value as InternalDbSet<UserEntity>).ToList();
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
        OkObjectResult? response = usersController.GetById(existingUser.Id) as OkObjectResult ;
        Assert.NotNull(response);
         
        UserEntity userEntity = response.Value as UserEntity;
        Assert.NotNull(userEntity);
        var comareResult = compareLogic.Compare(existingUser, userEntity);
        Assert.True(comareResult.AreEqual, comareResult.DifferencesString);
    }
    
    [Fact]
    public void UpdateUserValidModel_200OK()
    {
        var userRegisterRequest = new UpdateRequest()
        {
            FirstName = "Test First Upd",
            LastName = "Test Last Upd",
            Email = "testUpd@test.com",
            Password = "Test123 Upd",
        };
        OkObjectResult? response = usersController.Update(UserConfiguration.AdminUserId,userRegisterRequest) as OkObjectResult ;
        Assert.NotNull(response);
        
        string responseMessage = response.Value.ToString();
        Assert.NotNull(responseMessage);
        Assert.Contains("User updated successfully", responseMessage);
        
        Assert.Equal(UserConfiguration.CreateDefaultUsers().Count, Context.Users.Count());
        UserEntity? updatedUser = Context.Users.FirstOrDefault(u =>
            u.Email.Equals(userRegisterRequest.Email, StringComparison.InvariantCulture));
        
        Assert.NotNull(updatedUser);
        Assert.Equal(userRegisterRequest.FirstName, updatedUser.FirstName);
        Assert.Equal(userRegisterRequest.LastName, updatedUser.LastName);
        Assert.NotNull(updatedUser.PasswordHash);
    }
    
    [Fact]
    public void DeleteExistingUser_200OK()
    {
        int idForDeletion = UserConfiguration.CreateDefaultUsers().First().Id;
        OkObjectResult? response = usersController.Delete(idForDeletion) as OkObjectResult;
        Assert.NotNull(response);
        
        Assert.Equal(UserConfiguration.CreateDefaultUsers().Count -1, Context.Users.Count());
        UserEntity? deletedUser = Context.Users.FirstOrDefault(u => u.Id == idForDeletion);
        Assert.Null(deletedUser);
    }
}