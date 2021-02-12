using System.Text.Json.Serialization;

namespace Core.Model.Result
{
    public class Link
    {
        public string ImageSmall { get; set; }
        [JsonPropertyName("Imagelarge")] // Adjust the json attribute name to fit with the response from API
        public string ImageLarge { get; set; }
        public string ImageSvg { get; set; }
        public string Courier { get; set; }
        public string Service { get; set; }
    }
}
