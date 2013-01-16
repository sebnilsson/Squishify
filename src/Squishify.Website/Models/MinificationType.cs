using Newtonsoft.Json;

namespace Squishify.Website.Models
{
    [JsonObject(Title = "minificationType")]
    public class MinificationType
    {
        private readonly MinificationResult currentResult;

        public MinificationType(MinificationResult result, string id)
        {
            currentResult = result;
            this.Id = id;
        }

        [JsonProperty(PropertyName = "minifiedSize")]
        public string MinifiedSizeText
        {
            get
            {
                return this.MinifiedSize.ToString();
            }
        }

        public FileSize MinifiedSize { get; set; }

        [JsonProperty(PropertyName = "minifiedContent")]
        public string MinifiedContent { get; set; }

        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "difference")]
        public int Difference
        {
            get
            {
                if (currentResult.OriginalSize.Value == 0)
                {
                    return 0;
                }

                double org = currentResult.OriginalSize.Value;
                double min = MinifiedSize.Value;

                return (int)((1 - (min / org)) * 100);
            }
        }
    }
}