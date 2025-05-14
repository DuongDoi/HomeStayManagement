using Newtonsoft.Json;

namespace HomeStay_MVC.Models
{
    public class ResponseObjs
    {



        [JsonProperty("errCode")]
        public string errCode { get; set; }

        [JsonProperty("errMsg")]
        public string errMsgs { get; set; }


        [JsonProperty("data")]
        public Object data { get; set; }

    }
}
