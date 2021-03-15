using Newtonsoft.Json;

namespace Mon.Calculator.Models
{
    public class ResponseObject
    {
        [JsonProperty("result")]
        public string Result { get; set; }
    }
}