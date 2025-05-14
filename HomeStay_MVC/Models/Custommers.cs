using Newtonsoft.Json;

namespace HomeStay_MVC.Models
{
    public class CustomersObj
    {
        [JsonProperty("id")]
        public string id { get; set; }

        [JsonProperty("customer_card_id")]
        public string customer_card_id { get; set; }

        [JsonProperty("customer_name")]
        public string customer_name { get; set; }

        [JsonProperty("customer_phone")]
        public string customer_phone { get; set; }

        [JsonProperty("customer_address")]
        public string customer_address { get; set; }

        [JsonProperty("users_id")]
        public string users_id { get; set; }

    }

}
