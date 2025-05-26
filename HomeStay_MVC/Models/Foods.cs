using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace HomeStay_MVC.Models
{
    public class FOODS
    {
        [JsonProperty("ID")]
        public string ID { get; set; }

        [JsonProperty("FOODS_NAME")]
        public string FOODS_NAME { get; set; }

        [JsonProperty("FOODS_PRICE")]
        public string FOODS_PRICE { get; set; }

        [JsonProperty("FOODS_TYPE")]
        public string FOODS_TYPE { get; set; }

        [JsonProperty("AVATAR_PATH")]
        public string AVATAR_PATH { get; set; }

        [JsonProperty("USERS")]
        public string USERS { get; set; }

        [JsonProperty("CREATE_AT")]
        public DateTime? CREATE_AT { get; set; }

        [JsonProperty("UPDATE_AT")]
        public DateTime? UPDATE_AT { get; set; }
        public List<SelectListItem>? TypeOptions { get; set; }
        public string Save_code { get; set; }
    }
}
