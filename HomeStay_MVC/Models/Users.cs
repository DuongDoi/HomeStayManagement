using Newtonsoft.Json;
namespace HomeStay_MVC.Models
{
    public class UsersObj
    {
        [JsonProperty("id")]
        public string id { get; set; }


        [JsonProperty("users")]
        public string users { get; set; }

        [JsonProperty("pass")]
        public string pass { get; set; }

        [JsonProperty("phone")]
        public string phone { get; set; }

        [JsonProperty("email")]
        public string email { get; set; }
        [JsonProperty("name")]
        public string name { get; set; }
    }
}
