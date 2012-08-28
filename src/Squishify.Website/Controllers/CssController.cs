using System;
using System.Web.Http;

using Newtonsoft.Json.Linq;
using Squishify.Website.Models;

namespace Squishify.Website.Controllers {
    public class CssController : ApiController {
        [HttpPost]
        public MinificationResult Get(JObject jsonData) {
            dynamic json = jsonData;
            string source = json.source;
            string minifier = json.minifier;

            if(string.IsNullOrWhiteSpace(source)) {
                throw new ArgumentNullException("source");
            }
            if(string.IsNullOrWhiteSpace(minifier)) {
                throw new ArgumentNullException("minifier");
            }

            string minifiedContent = string.Empty;
            string usedMinifier = string.Empty;

            switch((minifier ?? string.Empty).ToLowerInvariant()) {
                case "ms":
                    var ms = new SquishIt.Framework.Minifiers.CSS.MsCompressor();
                    minifiedContent = ms.Minify(source);
                    usedMinifier = "MsCompressor";
                    break;
                case "yui":
                    var yui = new Yahoo.Yui.Compressor.CssCompressor {
                        CompressionType = Yahoo.Yui.Compressor.CompressionType.Standard,
                        RemoveComments = true,
                    };
                    minifiedContent = yui.Compress(source);
                    usedMinifier = "YuiCompressor";
                    break;
                default:
                    var nullCompressor = new SquishIt.Framework.Minifiers.CSS.NullCompressor();
                    minifiedContent = nullCompressor.Minify(source);
                    usedMinifier = "NullCompressor";
                    break;
            }

            minifiedContent = minifiedContent.TrimStart('\n', ' ');

            return new MinificationResult {
                MinifiedContent = minifiedContent,
                MinifiedSize = minifiedContent.Length,
                Minifier = usedMinifier,
                OriginalSize = source.Length,
            };
        }
    }
}