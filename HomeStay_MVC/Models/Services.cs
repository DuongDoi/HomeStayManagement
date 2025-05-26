using Newtonsoft.Json;

namespace HomeStay_MVC.Models
{
    public class Services
    {
        [JsonProperty("ID")]
        public string ID { get; set; }

        [JsonProperty("SERVICES_NAME")]
        public string SERVICES_NAME { get; set; }

        [JsonProperty("SERVICES_PRICE")]
        public string SERVICES_PRICE { get; set; }


        [JsonProperty("AVATAR_PATH")]
        public string AVATAR_PATH { get; set; }

        [JsonProperty("USERS")]
        public string USERS { get; set; }

        [JsonProperty("CREATE_AT")]
        public DateTime? CREATE_AT { get; set; }

        [JsonProperty("UPDATE_AT")]
        public DateTime? UPDATE_AT { get; set; }
        public string Save_code { get; set; }
    }
}
