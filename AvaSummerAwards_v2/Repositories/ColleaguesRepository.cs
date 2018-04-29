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
using System.Threading.Tasks;

namespace Awards.Repositories
{
    public class ColleaguesRepository
    {
        public List<Colleague> _colleagues { get; private set; }

        public ColleaguesRepository()
        {
        }

        /*public async Task<List<Colleague>> GetColleagues(string userObjectID, ISession session)
        {
            if(_colleagues == null)
            {
                var accessToken = await GetToken(userObjectID, session);
                HttpClient client = new HttpClient();
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, "https://graph.microsoft.com/v1.0/users");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                HttpResponseMessage response = await client.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    List<Dictionary<String, String>> responseElements = new List<Dictionary<String, String>>();
                    JsonSerializerSettings settings = new JsonSerializerSettings();
                    String responseString = await response.Content.ReadAsStringAsync();
                    responseElements = JsonConvert.DeserializeObject<List<Dictionary<String, String>>>(responseString, settings);
                    foreach (Dictionary<String, String> responseElement in responseElements)
                    {
                        TodoItem newItem = new TodoItem();
                        newItem.Title = responseElement["title"];
                        newItem.Owner = responseElement["owner"];
                        itemList.Add(newItem);
                    }

                    return View(itemList);
                }
            return _colleagues;
        }
        private async Task<string> GetToken(string userObjectID, ISession session)
        {
            AuthenticationContext authContext = new AuthenticationContext(AzureAdOptions.Settings.Authority, new NaiveSessionCache(userObjectID, session));
            ClientCredential credential = new ClientCredential(AzureAdOptions.Settings.ClientId, AzureAdOptions.Settings.ClientSecret);
            var result = await authContext.AcquireTokenSilentAsync("https://graph.microsoft.com", credential, new UserIdentifier(userObjectID, UserIdentifierType.UniqueId));
            return result.AccessToken;
        }*/
    }
}
