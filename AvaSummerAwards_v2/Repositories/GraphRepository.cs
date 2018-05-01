using Awards.Authentication;
using Awards.Helpers;
using Awards.Models;
using Awards.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Awards.Repositories
{
    public class GraphRepository
    {
        private HttpClient httpClient;
        public GraphRepository()
        {
            httpClient = new HttpClient();
        }

        public async Task<Nominee> PopulateNominee(NewNominationViewModel nomination, ClaimsPrincipal user, ISession session)
        {
            var accessToken = await GetAuthToken(user, session);
            var userInfo = await GetUserInfo(nomination.Email, accessToken);
            return new Nominee
            {
                CategoryID = nomination.CategoryID,
                Email = nomination.Email,
                Name = userInfo.FullName,
                Image = await GetUserPicture(nomination.Email, accessToken),
                Nominations = new List<Nomination>()
            };
        }

        private async Task<GraphUser> GetUserInfo(string userPrincipalName, string accessToken)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, $"https://graph.microsoft.com/v1.0/users/{userPrincipalName}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            HttpResponseMessage response = await httpClient.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                JsonSerializerSettings settings = new JsonSerializerSettings();
                String responseString = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<GraphUser>(responseString, settings);
            }
            throw new Exception("Error");
        }

        private async Task<string> GetUserPicture(string userPrincipalName, string accessToken)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, $"https://graph.microsoft.com/v1.0/users/{userPrincipalName}/Photos/120x120/$value");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            HttpResponseMessage response = await httpClient.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var stream = await response.Content.ReadAsStreamAsync();
                var contentType = response.Content.Headers.ContentType.MediaType;
                byte[] bytes = new byte[stream.Length];
                stream.Read(bytes, 0, (int)stream.Length);
                return $"data:{contentType};base64," + Convert.ToBase64String(bytes);
            }
            return "";
        }
        private async Task<string> GetAuthToken(ClaimsPrincipal user, ISession session)
        {
            string userObjectID = (user.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier"))?.Value;
            AuthenticationContext authContext = new AuthenticationContext(AzureAdOptions.Settings.Authority, new NaiveSessionCache(userObjectID, session));
            ClientCredential credential = new ClientCredential(AzureAdOptions.Settings.ClientId, AzureAdOptions.Settings.ClientSecret);
            var result = await authContext.AcquireTokenSilentAsync("https://graph.microsoft.com", credential, new UserIdentifier(userObjectID, UserIdentifierType.UniqueId));
            return result.AccessToken;
        }
    }
}
