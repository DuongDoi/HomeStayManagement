using Newtonsoft.Json;

namespace HomeStay_MVC.Models
{
    public class Bills
    {
        [JsonProperty("ID")]
        public string ID { get; set; }
        [JsonProperty("CUSTOMERS_CARD_ID")]
        public string? CUSTOMERS_CARD_ID { get; set; }
        [JsonProperty("TOTAL_MONEY")]
        public string? TOTAL_MONEY { get; set; }
        [JsonProperty("BILLS_STATUS")]
        public string? BILLS_STATUS { get; set; }
        [JsonProperty("CREATE_BY")]
        public string? CREATE_BY { get; set; }
        [JsonProperty("UPDATE_BY")]
        public string? UPDATE_BY { get; set; }
        [JsonProperty("ID_HOMESTAYS")]
        public string? ID_HOMESTAYS { get; set; }

        [JsonProperty("CUSTOMERS_NAME")]
        public string? CUSTOMERS_NAME { get; set; }

        [JsonProperty("HOMESTAYS_NAME")]
        public string? HOMESTAYS_NAME { get; set; }
        [JsonProperty("CREATE_AT")]
        public DateTime? CREATE_AT { get; set; }
        [JsonProperty("UPDATE_AT")]
        public DateTime? UPDATE_AT { get; set; }
    }
}
