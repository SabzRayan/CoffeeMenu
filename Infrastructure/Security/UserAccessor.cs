﻿using Microsoft.AspNetCore.Http;
using Application.Interfaces;
using System.Security.Claims;

namespace Infrastructure.Security
{
    public class UserAccessor : IUserAccessor
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserAccessor(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetUsername()
        {
            return _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name);
        }

        public string GetUserId()
        {
            return _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        public string GetUserEmail()
        {
            return _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);
        }
    }
}
