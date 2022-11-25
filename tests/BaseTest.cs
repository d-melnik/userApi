using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using UserApiDbClient.DbContext;
using UserApiDbClient.DbContext.Configuration;
using UserApiServices.Helpers;

namespace userApiTests;

public class BaseTest : IDisposable
{
    protected readonly DataContext Context;
    
    protected BaseTest()
    {
        Context = GetDataContextMock();
        Task[] taskList = {
            Context.Users.AddRangeAsync(UserConfiguration.CreateDefaultUsers()),
            Context.Claims.AddRangeAsync(ClaimConfiguration.CreateDefaultClaims()),
        };
        Task.WaitAll(taskList);
        Context.SaveChanges();
    }

    protected IConfiguration GetDefaultConfig()
    {
        return new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();
    }
    
    protected IMapper GetDefaultMapper()
    {
        var mapperConfig = new MapperConfiguration(mc =>
        {
            mc.AddProfile(new AutoMapperProfile());
        });
        return new Mapper(mapperConfig);
    }
    
    private DataContext GetDataContextMock()
    {
        var options = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(databaseName: "Test")
            .Options;
        var context = new DataContext(GetDefaultConfig(), options);
        return context;
    }


    public void Dispose()
    {
        Context.Database.EnsureDeleted();
        Context.Dispose();
    }
}