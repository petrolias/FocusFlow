﻿using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace FocusFlow.Blazor.Auth
{
    public class JwtAuthStateProvider : AuthenticationStateProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public JwtAuthStateProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {            
            var cookie = _httpContextAccessor.HttpContext?.Request.Cookies[Constants.CookieAccessToken];
            ClaimsIdentity identity = new();

            if (!string.IsNullOrWhiteSpace(cookie))
            {
                var handler = new JwtSecurityTokenHandler();
                var jwt = handler.ReadJwtToken(cookie);
                identity = new ClaimsIdentity(jwt.Claims, "jwt");
            }

            return Task.FromResult(new AuthenticationState(new ClaimsPrincipal(identity)));
        }

        public void NotifyAuthenticationStateChanged() =>
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

}
