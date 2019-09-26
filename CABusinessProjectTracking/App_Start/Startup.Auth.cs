using System;
using System.Collections.Generic;
using System.IdentityModel.Claims;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using BusinessProjectTracking.Utils;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Protocols;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Notifications;
using Microsoft.Owin.Security.OpenIdConnect;
using System.Configuration;
using Owin;
using Microsoft.Owin.Extensions;

namespace BusinessProjectTracking
{
    public partial class Startup
    {
        public static string ClientId = ConfigurationManager.AppSettings["ida:ClientId"];
        public static string RedirectUri = ConfigurationManager.AppSettings["ida:RedirectUrl"];
        public static string ClientSecret = ConfigurationManager.AppSettings["ida:ClientSecret"];
        public static string AadInstance = ConfigurationManager.AppSettings["ida:AADInstance"];
        public static string GraphResourceId = ConfigurationManager.AppSettings["ida:ResourceId"];
        public static string BasicSignInScope = ConfigurationManager.AppSettings["ida:BasicSignInScopes"];
        public static string Authority = ConfigurationManager.AppSettings["ida:Authority"];


        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        private void ConfigureAuth(IAppBuilder app)
        {
            app.SetDefaultSignInAsAuthenticationType(CookieAuthenticationDefaults.AuthenticationType);

            app.UseCookieAuthentication(new CookieAuthenticationOptions { });

            app.UseOpenIdConnectAuthentication(
                new OpenIdConnectAuthenticationOptions
                {
                    Authority = Authority,
                    ClientId = ClientId,
                    RedirectUri = RedirectUri,
                    PostLogoutRedirectUri = RedirectUri,
                    Scope = BasicSignInScope, // a basic set of permissions for user sign in & profile access
                    AuthenticationMode = AuthenticationMode.Passive,
                    TokenValidationParameters = new TokenValidationParameters
                    {
                        // We'll inject our own issuer validation logic below.
                        ValidateIssuer = false,
                        NameClaimType = "name",
                    },
                    Notifications = new OpenIdConnectAuthenticationNotifications()
                    {
                        SecurityTokenValidated = OnSecurityTokenValidated,
                        AuthorizationCodeReceived = OnAuthorizationCodeRecieved,
                        AuthenticationFailed = OnAuthenticationFailed,
                    }
                });
        }

        private Task OnAuthenticationFailed(AuthenticationFailedNotification<OpenIdConnectMessage, OpenIdConnectAuthenticationOptions> context)
        {
            // Handle any unexpected errors during sign in
            context.OwinContext.Response.Redirect("/Error?message=" + context.Exception.Message);
            context.HandleResponse(); // Suppress the exception
            return Task.FromResult(0);
        }

        private async Task OnAuthorizationCodeRecieved(AuthorizationCodeReceivedNotification context)
        {
            // Upon successful sign in, get & cache a token using MSAL
            string userId = context.AuthenticationTicket.Identity.FindFirst(ClaimTypes.NameIdentifier).Value;
            TokenCache userTokenCache = new MsalSessionTokenCache(userId, context.OwinContext.Environment["System.Web.HttpContextBase"] as HttpContextBase).GetMsalCacheInstance();
            ConfidentialClientApplication cc = new ConfidentialClientApplication(ClientId, RedirectUri, new ClientCredential(ClientSecret), userTokenCache, null);
            AuthenticationResult result = await cc.AcquireTokenByAuthorizationCodeAsync(context.Code, new[] { "user.readbasic.all" });
        }

        private Task OnSecurityTokenValidated(SecurityTokenValidatedNotification<OpenIdConnectMessage, OpenIdConnectAuthenticationOptions> context)
        {
            // Verify the user signing in is a business user, not a consumer user.
            string[] issuer = context.AuthenticationTicket.Identity.FindFirst("iss").Value.Split('/');
            string tenantId = issuer[(issuer.Length - 2)];

            return Task.FromResult(0);
        }
    }
}