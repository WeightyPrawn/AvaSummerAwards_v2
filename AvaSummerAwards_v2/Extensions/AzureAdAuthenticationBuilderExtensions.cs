using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Awards.Helpers;
//using avainterviews.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.IdentityModel.Tokens;

namespace Awards.Authentication
{
    public static class AzureAdAuthenticationBuilderExtensions
    {
        public static AuthenticationBuilder AddAzureAd(this AuthenticationBuilder builder)
            => builder.AddAzureAd(_ => { });

        public static AuthenticationBuilder AddAzureAd(this AuthenticationBuilder builder, Action<AzureAdOptions> configureOptions)
        {
            builder.Services.Configure(configureOptions);
            builder.Services.AddTransient<IConfigureOptions<OpenIdConnectOptions>, ConfigureAzureOptions>();
            builder.AddOpenIdConnect();
            return builder;
        }

        private class ConfigureAzureOptions : IConfigureNamedOptions<OpenIdConnectOptions>
        {
            private readonly AzureAdOptions _azureOptions;

            public ConfigureAzureOptions(IOptions<AzureAdOptions> azureOptions)
            {
                _azureOptions = azureOptions.Value;
            }

            public void Configure(string name, OpenIdConnectOptions options)
            {
                options.ClientId = _azureOptions.ClientId;
                options.Authority = $"{_azureOptions.Instance}{_azureOptions.TenantId}";
                options.UseTokenLifetime = true;
                options.CallbackPath = _azureOptions.CallbackPath;
                options.RequireHttpsMetadata = false;
                options.ClientSecret = _azureOptions.ClientSecret;
                //options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.Resource = "https://graph.microsoft.com"; // AAD graph

                options.ResponseType = "id_token code";

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    // Instead of using the default validation (validating against a single issuer value, as we do in line of business apps),
                    // we inject our own multitenant validation logic
                    ValidateIssuer = true,

                    // If the app is meant to be accessed by entire organizations, add your issuer validation logic here.
                    IssuerValidator = (issuer, securityToken, validationParameters) => {
                        if (issuer == "https://sts.windows.net/cf36141c-ddd7-45a7-b073-111f66d0b30c/") return issuer;
                        else throw new SecurityTokenInvalidIssuerException("Invalid issues");
                    }
                };

                options.Events = new OpenIdConnectEvents
                {
                    OnTicketReceived = context =>
                    {
                        // If your authentication logic is based on users then add your logic here
                        return Task.CompletedTask;
                    },
                    OnAuthenticationFailed = context =>
                    {
                        context.Response.Redirect("/Home/Error");
                        context.HandleResponse(); // Suppress the exception
                        return Task.CompletedTask;
                    },
                    // If your application needs to do authenticate single users, add your user validation below.
                    OnTokenValidated = context =>
                    {
                        //TODO: this -really- shouldn't be hard coded but DI won't cooperate :(
                        List<string> admins = new List<string>() {
                            "m.jarncrantz.ward@avanade.com"
                        };
                        string id = context.Principal.Identity.Name;
                        if (admins.Contains(id)) {
                            var claims = new List<Claim>() {
                                new Claim(ClaimTypes.Role, "admin")
                            };
                            var appIdentity = new ClaimsIdentity(claims);
                            context.Principal.AddIdentity(appIdentity);
                        }
                        return Task.CompletedTask;
                    },
                    OnAuthorizationCodeReceived = async ctx =>
                    {
                        var code = ctx.ProtocolMessage.Code;
                        string userObjectId = (ctx.Principal.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier"))?.Value;
                        var authContext = new AuthenticationContext(ctx.Options.Authority, new NaiveSessionCache(userObjectId, ctx.HttpContext.Session));
                        var credential = new ClientCredential(ctx.Options.ClientId, ctx.Options.ClientSecret);

                        var authResult = await authContext.AcquireTokenByAuthorizationCodeAsync(ctx.TokenEndpointRequest.Code,
                            new Uri(ctx.TokenEndpointRequest.RedirectUri, UriKind.RelativeOrAbsolute), credential, ctx.Options.Resource);

                        // Notify the OIDC middleware that we already took care of code redemption.
                        ctx.HandleCodeRedemption(authResult.AccessToken, ctx.ProtocolMessage.IdToken);
                    }
                    };
            }

            public void Configure(OpenIdConnectOptions options)
            {
                Configure(Options.DefaultName, options);
            }
        }
    }
}