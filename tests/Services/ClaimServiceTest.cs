using System;
using System.Collections.Generic;
using System.Linq;
using KellermanSoftware.CompareNetObjects;
using UserApiDbClient.DbContext.Configuration;
using UserApiDbClient.Entities;
using UserApiServices.Models.Roles;
using UserApiServices.Services.Claims;
using Xunit;

namespace userApiTests.Services;

[Collection("Sequential")]
public class ClaimServiceTest : BaseTest
{
    private readonly ClaimsService claimsService;
    private readonly CompareLogic compareLogic;

    public ClaimServiceTest()
    {
        claimsService = new ClaimsService(Context, GetDefaultMapper());
        compareLogic = new CompareLogic();
    }

    [Fact]
    public void GetAllTest()
    {
        IList<ClaimEntity> claimEntities = claimsService.GetAll().ToList();
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
        ClaimEntity existingUser = ClaimConfiguration.CreateDefaultClaims().First();
        ClaimEntity userEntity = claimsService.GetById(existingUser.Id);

        var result = compareLogic.Compare(existingUser, userEntity);
        Assert.True(result.AreEqual, result.DifferencesString);
    }

    [Fact]
    public void GetNotExistingClaim_ExceptionThrown()
    {
        var exception = Assert.Throws<KeyNotFoundException>(() => claimsService.GetById(-999));
        Assert.Contains("not found", exception.Message.ToLower());
    }

    [Fact]
    public void CreateClaimValidModel_ClaimCreated()
    {
        var claimRegisterRequest = new ChangeClaimRequest()
        {
            Name = "For unit test",
            Description = "For unit test"
        };

        claimsService.Create(claimRegisterRequest);

        Assert.Equal(ClaimConfiguration.CreateDefaultClaims().Count + 1, Context.Claims.Count());
        ClaimEntity? createdClaim = Context.Claims.FirstOrDefault(u =>
            u.Name.Equals(claimRegisterRequest.Name, StringComparison.InvariantCulture));

        Assert.NotNull(createdClaim);
        Assert.Equal(claimRegisterRequest.Name, createdClaim.Name);
        Assert.Equal(claimRegisterRequest.Description, createdClaim.Description);
    }

    [Fact]
    public void RegisterClaimExistingUser_ExceptionThrown()
    {
        var claimRegisterRequest = new ChangeClaimRequest()
        {
            Name = ClaimConfiguration.CreateDefaultClaims().First().Name,
            Description = "For unit test"
        };

        var exception = Assert.Throws<InvalidOperationException>(() => claimsService.Create(claimRegisterRequest));
        Assert.Contains("already exists", exception.Message.ToLower());
    }

    [Fact]
    public void UpdateClaimValidModel_ClaimCreated()
    {
        var claimRegisterRequest = new ChangeClaimRequest()
        {
            Name = "For unit test",
            Description = "For unit test"
        };

        claimsService.Update(ClaimConfiguration.AdminClaimId, claimRegisterRequest);

        Assert.Equal(ClaimConfiguration.CreateDefaultClaims().Count, Context.Users.Count());
        ClaimEntity? updatedClaim = Context.Claims.FirstOrDefault(u =>
            u.Name.Equals(claimRegisterRequest.Name, StringComparison.InvariantCulture));

        Assert.NotNull(updatedClaim);
        Assert.Equal(claimRegisterRequest.Name, updatedClaim.Name);
        Assert.Equal(claimRegisterRequest.Description, updatedClaim.Description);
    }

    [Fact]
    public void DeleteClaimValidModel_ClaimCreated()
    {
        claimsService.Delete(ClaimConfiguration.AdminClaimId);

        Assert.Equal(ClaimConfiguration.CreateDefaultClaims().Count - 1, Context.Claims.Count());
        ClaimEntity? updatedClaim = Context.Claims.FirstOrDefault(u => u.Id == ClaimConfiguration.AdminClaimId);
        Assert.Null(updatedClaim);
    }


    [Fact]
    public void DeleteClaimInvalidId_Exception()
    {
        var exception = Assert.Throws<KeyNotFoundException>(() => claimsService.Delete(-999));
        Assert.Contains("not found", exception.Message);
    }
}