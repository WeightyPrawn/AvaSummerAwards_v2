using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Awards.Models
{
    public class GraphUser
    {
        [JsonProperty("id")]
        public string ID { get; set; }
        [JsonProperty("givenName")]
        public string GivenName { get; set; }
        [JsonProperty("surname")]
        public string Surname { get; set; }
        [JsonProperty("userPrincipalName")]
        public string UserPrincipalName { get; set; }
        [JsonProperty("displayName")]
        public string DisplayName { get; set; }
        public string FullName
        {
            get
            {
                return $"{GivenName} {Surname}";
            }
        }
    }
}