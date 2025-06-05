using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace HomeStay_MVC.Models
{
    public class Rooms
    {
        [JsonProperty("ID")]
        public string? ID { get; set; }

        [JsonProperty("HOMESTAYS_NAME")]
        public string? HOMESTAYS_NAME { get; set; }

        [JsonProperty("ROOMS_NAME")]
        public string? ROOMS_NAME { get; set; }

        [JsonProperty("ROOMS_PRICE")]
        public string? ROOMS_PRICE { get; set; }

        [JsonProperty("ROOMS_STATUS")]
        public string? ROOMS_STATUS { get; set; }


        [JsonProperty("AVATAR_PATH")]
        public string? AVATAR_PATH { get; set; }

        [JsonProperty("CREATE_AT")]
        public DateTime? CREATE_AT { get; set; }

        [JsonProperty("UPDATE_AT")]
        public string? TYPE { get; set; }
        public string? NUMBER_BED { get; set; }
        public string? SQUARE { get; set; }
        public DateTime? UPDATE_AT { get; set; }
        public string? Save_code { get; set; }
        public IEnumerable<SelectListItem>? TypeOptions { get; set; }
        public IEnumerable<SelectListItem>? StatusOptions { get; set; }
        public List<SelectListItem> HomeStayOptions { get; set; } = new List<SelectListItem>();
        public string? HOMESTAYS_ID { get; set; }
        public string? USERS_ID { get; set; }
    }
}
