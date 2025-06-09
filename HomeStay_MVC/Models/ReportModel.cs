using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace HomeStay_MVC.Models
{
    public class ReportModel
    {
        
            [JsonProperty("ID")]
            public string? ID { get; set; }

            [JsonProperty("AMOUNT")]
            public int? AMOUNT { get; set; }
            [JsonProperty("HOMESTAYS_ID")]
            public string? HOMESTAYS_ID { get; set; }
            [JsonProperty("HOMESTAYS_NAME")]
            public string? HOMESTAYS_NAME { get; set; }
        [JsonProperty("BILLS_ID")]
            public string? BILLS_ID { get; set; }
            [JsonProperty("CATEGORY")]
            public string? CATEGORY { get; set; }

            [JsonProperty("DESCRIPT")]
            public string? DESCRIPT { get; set; }

            [JsonProperty("TYPE")]
            public string? TYPE { get; set; }

            [JsonProperty("UPDATE_BY_USERS_NAME")]
            public string? UPDATE_BY_USERS_NAME { get; set; }
            [JsonProperty("UPDATE_BY")]
            public string? UPDATE_BY { get; set; }

            [JsonProperty("CREATE_BY_USERS_NAME")]
            public string? CREATE_BY_USERS_NAME { get; set; }

            [JsonProperty("CREATE_BY")]
            public string? CREATE_BY { get; set; }


            [JsonProperty("CREATE_AT")]
            public DateTime? CREATE_AT { get; set; }

            [JsonProperty("UPDATE_AT")]
            public DateTime? UPDATE_AT { get; set; }
            public List<SelectListItem>? TypeOptions { get; set; }
            public string? Save_code { get; set; }
        
    }
}
