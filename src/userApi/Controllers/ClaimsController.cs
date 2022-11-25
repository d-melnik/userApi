using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserApiDbClient.DbContext.Configuration;
using UserApiDbClient.Entities;
using UserApiServices.Models.Roles;
using UserApiServices.Services.Claims;

namespace userApi.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize(Roles = ClaimConfiguration.AdminRoleName, AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class ClaimsController : ControllerBase
{
    private readonly IClaimsService claimsService;

    public ClaimsController(IClaimsService claimsService)
    {
        this.claimsService = claimsService;
    }
    
    [HttpGet]
    
    public IActionResult GetAll()
    {
        var claimEntities = claimsService.GetAll();
        return Ok(claimEntities);
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        ClaimEntity claimEntity = claimsService.GetById(id);
        return Ok(claimEntity);
    }
    
    [HttpPost]
    public IActionResult Create(ChangeClaimRequest model)
    {
        ClaimEntity claimEntity = claimsService.Create(model);
        return Ok(claimEntity);
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, ChangeClaimRequest model)
    {
        claimsService.Update(id, model);
        return Ok(new { message = "Claim updated successfully" });
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        claimsService.Delete(id);
        return Ok(new { message = "Claim deleted successfully" });
    }
}