using Newtonsoft.Json;

namespace HomeStay_MVC.Models
{
    public class HomestaysObj
    {
        [JsonProperty("id")]
        public string? id { get; set; }

        [JsonProperty("HOMESTAYS_NAME")]
        public string HOMESTAYS_NAME { get; set; }

        [JsonProperty("MANAGER_CARD_NUMBER")]
        public string MANAGER_CARD_NUMBER { get; set; }

        [JsonProperty("HOMESTAYS_ADDRESS")]
        public string HOMESTAYS_ADDRESS { get; set; }

        [JsonProperty("HOMESTAY_DESCRIPTION")]
        public string HOMESTAY_DESCRIPTION { get; set; }

        [JsonProperty("MANAGER_NAME")]
        public string MANAGER_NAME { get; set; }

        [JsonProperty("MANAGER_PHONE")]
        public string MANAGER_PHONE { get; set; }

        [JsonProperty("USERS_ID")]
        public string USERS_ID { get; set; }
        [JsonProperty("AVATAR_PATH")]
        public string? AVATAR_PATH { get; set; }
        [JsonProperty("CREATE_AT")]
        public DateTime? CREATE_AT { get; set; }
        [JsonProperty("UPDATE_AT")]
        public DateTime? UPDATE_AT { get; set; }
        [JsonProperty("Save_code")]
        public string? Save_code { get; set; }



    }
}
