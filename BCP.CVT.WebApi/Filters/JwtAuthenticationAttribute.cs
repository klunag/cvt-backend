using BCP.CVT.WebApi.Auth;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Filters;

namespace BCP.CVT.WebApi.Filters
{
    public class JwtAuthenticationAttribute : Attribute, IAuthenticationFilter
    {
        public string Realm { get; set; }
        public bool AllowMultiple => false;

        public async Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            var request = context.Request;
            var authorization = request.Headers.Authorization;

            if (authorization == null || authorization.Scheme != "Bearer")
                return;

            if (string.IsNullOrEmpty(authorization.Parameter))
            {
                context.ErrorResult = new AuthenticationFailureResult("Missing Jwt Token", request);
                return;
            }

            var token = authorization.Parameter;
            var principal = await AuthenticateJwtToken(token);

            if (principal == null)
                context.ErrorResult = new AuthenticationFailureResult("Invalid token", request);

            else
                context.Principal = principal;
        }

        private static bool ValidateToken(string token, out string username)
        {
            username = null;
           
            string stsDiscoveryEndpoint = "https://login.microsoftonline.com/common/v2.0/.well-known/openid-configuration";

            ConfigurationManager<OpenIdConnectConfiguration> configManager = new ConfigurationManager<OpenIdConnectConfiguration>(stsDiscoveryEndpoint, new OpenIdConnectConfigurationRetriever());

            OpenIdConnectConfiguration config = configManager.GetConfigurationAsync().Result;

            TokenValidationParameters validationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                IssuerSigningKeys = config.SigningKeys,
                ValidateLifetime = false
            };

            JwtSecurityTokenHandler tokendHandler = new JwtSecurityTokenHandler();

            SecurityToken jwt;

            var result = tokendHandler.ValidateToken(token, validationParameters, out jwt);

            var identity = result?.Identity as ClaimsIdentity;

            if (identity == null)
                return false;

            if (!identity.IsAuthenticated)
                return false;

            var jwtToString = jwt.ToString();
            if (string.IsNullOrWhiteSpace(jwtToString))
                return false;

            var dateCurrent = DateTime.UtcNow;

            var dateFrom = jwt.ValidFrom;
            var dateTo = jwt.ValidTo;

            if ((dateCurrent >= dateFrom && dateCurrent <= dateTo) == false)
                return false;

            username = jwt.Issuer;

            return true;
        }
        
        protected Task<IPrincipal> AuthenticateJwtToken(string token)
        {
            if (ValidateToken(token, out var username))
            {
                // based on username to get more information from database in order to build local identity
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, username)
                    // Add more claims if needed: Roles, ...
                };

                var identity = new ClaimsIdentity(claims, "Jwt");
                IPrincipal user = new ClaimsPrincipal(identity);

                return Task.FromResult(user);
            }

            return Task.FromResult<IPrincipal>(null);
        }

        public Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            Challenge(context);
            return Task.FromResult(0);
        }

        private void Challenge(HttpAuthenticationChallengeContext context)
        {
            string parameter = null;

            if (!string.IsNullOrEmpty(Realm))
                parameter = "realm=\"" + Realm + "\"";

            context.ChallengeWith("Bearer", parameter);
        }
    }
}