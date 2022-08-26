using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using KellermanSoftware.CompareNetObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using userApi.Controllers;
using userApi.Helpers;
using userApi.Models.Roles;
using userApi.Models.Users;
using userApi.Services.Claims;
using userApi.Services.Users;
using UserApiDbClient.DbContext.Configuration;
using UserApiDbClient.Entities;
using WebApi.Models.Users;
using Xunit;

namespace userApiTests.Controllers;

[Collection("Sequential")]
public class ClaimsControllerTest : BaseTest
{
    private readonly ClaimsController claimsController;
    private readonly CompareLogic compareLogic;

    public ClaimsControllerTest()
    {
        claimsController = new ClaimsController(new ClaimsService(Context, GetDefaultMapper()));
        compareLogic = new CompareLogic();
    }

    [Fact]
    public void CreateClaimValidModel_200OK()
    {
       var claimRegisterRequest = new ChangeClaimRequest()
        {
            Name = "For unit test",
            Description = "For unit test"
        };
        var response = claimsController.Create(claimRegisterRequest) as OkObjectResult;
        Assert.NotNull(response);
        ClaimEntity? convertedResponse = response.Value as ClaimEntity;
        Assert.NotNull(convertedResponse);
        
        Assert.Equal(ClaimConfiguration.CreateDefaultClaims().Count + 1, Context.Claims.Count());
        ClaimEntity? createdClaim = Context.Claims.FirstOrDefault(u =>
            u.Name.Equals(claimRegisterRequest.Name, StringComparison.InvariantCulture));

        Assert.NotNull(createdClaim);
        Assert.Equal(claimRegisterRequest.Name, createdClaim.Name);
        Assert.Equal(claimRegisterRequest.Description, createdClaim.Description);
        
        Assert.Equal(claimRegisterRequest.Name, convertedResponse.Name);
        Assert.Equal(claimRegisterRequest.Description, convertedResponse.Description);
    }
    
    [Fact]
    public void GetAll_AllClaimsList()
    {
        OkObjectResult? response = claimsController.GetAll() as OkObjectResult ;
        Assert.NotNull(response);

        IList<ClaimEntity> claimEntities = (response.Value as InternalDbSet<ClaimEntity>).ToList();
        Assert.NotNull(claimEntities);
        Assert.NotEmpty(claimEntities);
        Assert.Equal(claimEntities.Count, UserConfiguration.CreateDefaultUsers().Count);

        IList<ClaimEntity> sortedListCurrent = claimEntities.OrderBy(u => u.Id).ToList();
        IList<ClaimEntity> sortedListExpected = ClaimConfiguration.CreateDefaultClaims().OrderBy(u => u.Id).ToList();

        for (int claimI = 0; claimI < claimEntities.Count; claimI++)
        {
            var result = compareLogic.Compare(sortedListCurrent[claimI], sortedListExpected[claimI]);
            Assert.True(result.AreEqual, result.DifferencesString);
        }
    }
    
    [Fact]
    public void GetExistingClaim_ClaimFound()
    {
        ClaimEntity existingClaim = ClaimConfiguration.CreateDefaultClaims().First();
        OkObjectResult? response = claimsController.GetById(existingClaim.Id) as OkObjectResult ;
        Assert.NotNull(response);
         
        ClaimEntity claimEntity = response.Value as ClaimEntity;
        Assert.NotNull(claimEntity);
        var comareResult = compareLogic.Compare(existingClaim, claimEntity);
        Assert.True(comareResult.AreEqual, comareResult.DifferencesString);
    }
    
    [Fact]
    public void UpdateClaimValidModel_200OK()
    {
        var claimRegisterRequest = new ChangeClaimRequest()
        {
            Name = "For unit test",
            Description = "For unit test"
        };
        OkObjectResult? response = claimsController.Update(ClaimConfiguration.AdminClaimId, claimRegisterRequest) as OkObjectResult ;
        Assert.NotNull(response);
        
        string responseMessage = response.Value.ToString();
        Assert.NotNull(responseMessage);
        Assert.Contains("Claim updated successfully", responseMessage);
        
        Assert.Equal(ClaimConfiguration.CreateDefaultClaims().Count, Context.Users.Count());
        ClaimEntity? updatedClaim = Context.Claims.FirstOrDefault(u =>
            u.Name.Equals(claimRegisterRequest.Name, StringComparison.InvariantCulture));

        Assert.NotNull(updatedClaim);
        Assert.Equal(claimRegisterRequest.Name, updatedClaim.Name);
        Assert.Equal(claimRegisterRequest.Description, updatedClaim.Description);
    }
    
    [Fact]
    public void DeleteExistingClaim_200OK()
    {
        int idForDeletion = ClaimConfiguration.CreateDefaultClaims().First().Id;
        OkObjectResult? response = claimsController.Delete(idForDeletion) as OkObjectResult;
        Assert.NotNull(response);
        
        Assert.Equal(ClaimConfiguration.CreateDefaultClaims().Count -1, Context.Claims.Count());
        ClaimEntity? deletedUser = Context.Claims.FirstOrDefault(u => u.Id == idForDeletion);
        Assert.Null(deletedUser);
    }
}