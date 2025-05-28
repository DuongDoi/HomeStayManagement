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

        [JsonProperty("HOMESTAYS_ADDRESS")]
        public string? HOMESTAYS_ADDRESS { get; set; }
        [JsonProperty("CREATE_AT")]
        public DateTime? CREATE_AT { get; set; }
        [JsonProperty("UPDATE_AT")]
        public DateTime? UPDATE_AT { get; set; }
        [JsonProperty("CUSTOMERS_PHONE")]
        public string? CUSTOMERS_PHONE { get; set; }
        [JsonProperty("CUSTOMERS_ADDRESS")]
        public string? CUSTOMERS_ADDRESS { get; set; }

        public List<RoomDetail> Rooms { get; set; } = new List<RoomDetail>();
        public List<ServiceDetail> Services { get; set; } = new List<ServiceDetail>();
        public List<FoodDetail> Foods { get; set; } = new List<FoodDetail>();
    }
    public class RoomDetail
    {
        public string ROOMS_NAME { get; set; }
        public DateTime CHECKIN_DATE { get; set; }
        public DateTime CHECKOUT_DATE { get; set; }
        public int TOTAL_DAYS { get; set; }
        public decimal PRICE_PER_DAY { get; set; }
        public decimal TOTAL_PRICE { get; set; }
    }

    public class ServiceDetail
    {
        public string SERVICES_NAME { get; set; }
        public int QUANTITY_SERVICE { get; set; }
        public decimal UNIT_PRICE_SERVICE { get; set; }
        public decimal TOTAL_PRICE_SERVICE { get; set; }
    }
    public class FoodDetail
    {
        public string FOODS_NAME { get; set; }
        public int QUANTITY_FOOD { get; set; }
        public decimal UNIT_PRICE_FOOD { get; set; }
        public decimal TOTAL_PRICE_FOOD { get; set; }
    }
}
