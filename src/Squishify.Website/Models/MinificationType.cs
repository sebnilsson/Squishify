using Newtonsoft.Json;

namespace Squishify.Website.Models {
    [JsonObject(Title = "minificationType")]
    public class MinificationType {
        private MinificationResult _result;

        public MinificationType(MinificationResult result, string id) {
            _result = result;
            this.Id = id;
        }

        [JsonProperty(PropertyName = "minifiedSize")]
        public int MinifiedSize { get; set; }

        [JsonProperty(PropertyName = "minifiedContent")]
        public string MinifiedContent { get; set; }

        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "difference")]
        public int Difference {
            get {
                if(_result.OriginalSize == 0) {
                    return 0;
                }

                double org = _result.OriginalSize;
                double min = MinifiedSize;

                return (int)((1 - (min / org)) * 100);
            }
        }
    }
}