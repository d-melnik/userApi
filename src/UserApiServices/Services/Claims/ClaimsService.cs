using AutoMapper;
using UserApiDbClient.DbContext;
using UserApiDbClient.Entities;
using UserApiServices.Models.Roles;

namespace UserApiServices.Services.Claims
{
    public class ClaimsService : IClaimsService
    {
        private readonly DataContext context;
        private readonly IMapper mapper;

        public ClaimsService(
            DataContext context,
            IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public IEnumerable<ClaimEntity> GetAll()
        {
            return context.Claims;
        }

        public ClaimEntity GetById(int id)
        {
            return GetClaim(id);
        }

        public ClaimEntity Create(ChangeClaimRequest request)
        {
            // validate
            if (context.Claims.Any(x => x.Name.Equals(request.Name, StringComparison.InvariantCultureIgnoreCase)))
                throw new InvalidOperationException("Claim '" + request.Name + "' is already exists");

            // map model to new user object
            ClaimEntity claimEntity = mapper.Map<ClaimEntity>(request);

            context.Claims.Add(claimEntity);
            context.SaveChanges();
            return claimEntity;
        }

        public void Update(int id, ChangeClaimRequest request)
        {
            ClaimEntity claim = GetClaim(id);

            // validate
            if (context.Claims.Any(x => x.Name.Equals(request.Name, StringComparison.InvariantCultureIgnoreCase)))
                throw new InvalidOperationException("Claim '" + request.Name + "' is already exists");

            mapper.Map(request, claim);
            context.Claims.Update(claim);
            context.SaveChanges();
        }

        public void Delete(int id)
        {
            ClaimEntity claimEntity = GetClaim(id);
            context.Claims.Remove(claimEntity);
            context.SaveChanges();
        }

        // helper methods
        private ClaimEntity GetClaim(int id)
        {
            ClaimEntity claimEntity = context.Claims.Find(id);
            if (claimEntity == null) throw new KeyNotFoundException("Claim not found");
            return claimEntity;
        }
    }
}