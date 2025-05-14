using Newtonsoft.Json;

namespace HomeStay_MVC.Models
{
    public class Customers
    {
        [JsonProperty("ID")]
        public string ID { get; set; }

        [JsonProperty("CUSTOMERS_CARD_NUMBER")]
        public string CUSTOMERS_CARD_NUMBER { get; set; }

        [JsonProperty("CUSTOMERS_NAME")]
        public string CUSTOMERS_NAME { get; set; }

        [JsonProperty("CUSTOMERS_PHONE")]
        public string CUSTOMERS_PHONE { get; set; }

        [JsonProperty("CUSTOMERS_ADDRESS")]
        public string CUSTOMERS_ADDRESS { get; set; }

        [JsonProperty("USERS")]
        public string USERS { get; set; }

        [JsonProperty("CREATE_AT")]
        public DateTime? CREATE_AT { get; set; }

        [JsonProperty("CREATE_BY")]
        public string CREATE_BY { get; set; }

        [JsonProperty("UPDATE_AT")]
        public DateTime? UPDATE_AT { get; set; }
    }
}
