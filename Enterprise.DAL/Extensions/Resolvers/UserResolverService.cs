using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace Enterprise.DAL.Extensions.Resolvers
{
    public interface IUserResolverService
    {
        int GetUserId();
    }
    public class UserResolverService : IUserResolverService
    {
        private readonly IHttpContextAccessor _context;
        public UserResolverService(IHttpContextAccessor context)
        {
            _context = context;
        }

        public int GetUserId()
        {
           var userId = String.IsNullOrEmpty(_context?.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value)
                            ? 0
                            : Int32.Parse(_context?.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            return userId;
        }
    }
}
