using System.Collections.Generic;
using System.Linq;

using Newtonsoft.Json;

namespace Squishify.Website.Models {
    [JsonObject(Title = "minificationResult")]
    public class MinificationResult {
        [JsonProperty(PropertyName = "originalSize")]
        public string OriginalSizeText {
            get {
                return this.OriginalSize.ToString();
            }
        }

        public FileSize OriginalSize { get; set; }

        [JsonProperty(PropertyName = "types")]
        public IEnumerable<MinificationType> Types { get; set; }

        [JsonProperty(PropertyName = "smallest")]
        public string Smallest {
            get {
                if(this.Types == null || !this.Types.Any()) {
                    return string.Empty;
                }

                string smallest = this.Types.OrderBy(x => x.MinifiedSize.Value).First().Id;
                return smallest;
            }
        }
    }
}