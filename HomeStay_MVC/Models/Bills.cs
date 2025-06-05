using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace HomeStay_MVC.Models
{
    public class Bills
    {
        [JsonProperty("ID")]
        public string? ID { get; set; }
        [JsonProperty("CUSTOMERS_CARD_ID")]
        public string? CUSTOMERS_CARD_ID { get; set; }
        [JsonProperty("TOTAL_MONEY")]
        public int? TOTAL_MONEY { get; set; }
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
        public string? MANAGER_PHONE { get; set; }

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
        public string? Save_code { get; set; }

        public List<RoomDetail>? Rooms { get; set; } = new List<RoomDetail>();
        public List<ServiceDetail>? RoomDetailsRaw { get; set; }
        public List<ServiceDetail>? Services { get; set; } = new List<ServiceDetail>();
        public List<ServiceDetail>? ServiceDetailsRaw { get; set; }
        public List<FoodDetail>? Foods { get; set; } = new List<FoodDetail>();
        public List<FoodDetail>? FoodDetailsRaw { get; set; }
        public List<SelectListItem>? RoomOptions { get; set; } = new List<SelectListItem>();
        public List<SelectListItem>? RoomSelected { get; set; } = new List<SelectListItem>();
        public List<SelectListItem>? FoodDrinkOptions { get; set; } = new List<SelectListItem>();
        public List<SelectListItem>? FoodDrinkSelected { get; set; } = new List<SelectListItem>();
        public List<SelectListItem>? ServiceOptions { get; set; } = new List<SelectListItem>();
        public List<SelectListItem>? ServiceSelected { get; set; } = new List<SelectListItem>();
        public List<SelectListItem>? HomestayOptions { get; set; } = new List<SelectListItem>();
        public List<SelectListItem>? HomestaySelected { get; set; } = new List<SelectListItem>();
        public List<SelectListItem>? CustomerOptions { get; set; } = new List<SelectListItem>();
        public List<SelectListItem>? CustomerSelected { get; set; } = new List<SelectListItem>();
    }
    public class RoomDetail
    {
        public string? RoomId { get; set; }
        public string? ROOMS_NAME { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public int? TOTAL_DAYS { get; set; }
        public int? PRICE_PER_DAY { get; set; }
        public int? TOTAL_PRICE { get; set; }
    }

    public class ServiceDetail
    {
        public string? ServiceId { get; set; }
        public string?    SERVICES_NAME { get; set; }
        public int? Quantity { get; set; }
        public int? UNIT_PRICE_SERVICE { get; set; }
        public int? TOTAL_PRICE_SERVICE { get; set; }
    }
    public class FoodDetail
    {
        public string? FoodId { get; set; }
        public string? FOODS_NAME { get; set; }
        public int? Quantity { get; set; }
        public int? UNIT_PRICE_FOOD { get; set; }
        public int? TOTAL_PRICE_FOOD { get; set; }
    }
}
