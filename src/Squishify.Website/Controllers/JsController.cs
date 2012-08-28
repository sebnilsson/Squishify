using System;
using System.Web.Http;

using Newtonsoft.Json.Linq;
using Squishify.Website.Models;
using SquishIt.Framework.JavaScript;
using SquishIt.Framework.Minifiers;

namespace Squishify.Website.Controllers {
    public class JsController : ApiController {
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
                //case "closure":
                //    jsMinifier = new SquishIt.Framework.Minifiers.JavaScript.ClosureMinifier();
                //    break;
                case "jsmin":
                    var jsMin = new SquishIt.Framework.Minifiers.JavaScript.JsMinMinifier();
                    minifiedContent = jsMin.Minify(source);
                    usedMinifier = "JsMinMinifier";
                    break;
                case "msmin":
                    var ms = new SquishIt.Framework.Minifiers.JavaScript.MsMinifier();
                    minifiedContent = ms.Minify(source);
                    usedMinifier = "MsMinifier";
                    break;
                case "yui":
                    var yui = new Yahoo.Yui.Compressor.JavaScriptCompressor {
                        CompressionType = Yahoo.Yui.Compressor.CompressionType.Standard,
                        DisableOptimizations = false,
                    };
                    minifiedContent = yui.Compress(source);
                    usedMinifier = "YuiMinifier";
                    break;
                default:
                    var nullMinifier = new SquishIt.Framework.Minifiers.JavaScript.NullMinifier();
                    minifiedContent = nullMinifier.Minify(source);
                    usedMinifier = "NullMinifier";
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