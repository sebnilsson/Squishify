using System;
using System.Web.Http;

using Newtonsoft.Json.Linq;
using Squishify.Website.Models;

namespace Squishify.Website.Controllers {
    public class JsController : ApiController {
        [HttpPost]
        public MinificationResult Get(JObject jsonData) {
            dynamic json = jsonData;
            string source = json.source;

            if(string.IsNullOrWhiteSpace(source)) {
                throw new ArgumentNullException("source");
            }

            var result = new MinificationResult {
                OriginalSize = source.Length,
            };

            var jsMin = new SquishIt.Framework.Minifiers.JavaScript.JsMinMinifier();
            string jsMinContent = jsMin.Minify(source).TrimStart('\n', ' ');

            var jsMinResultType = new MinificationType(result, "JsMinMinifier") {
                MinifiedContent = jsMinContent,
                MinifiedSize = jsMinContent.Length,
            };

            var ms = new SquishIt.Framework.Minifiers.JavaScript.MsMinifier();
            string msConfig = ms.Minify(source).TrimStart('\n', ' ');

            var msResultType = new MinificationType(result, "MsMinifier") {
                MinifiedContent = msConfig,
                MinifiedSize = msConfig.Length,
            };

            var yui = new Yahoo.Yui.Compressor.JavaScriptCompressor {
                CompressionType = Yahoo.Yui.Compressor.CompressionType.Standard,
                DisableOptimizations = false,
            };
            string yuiContent = yui.Compress(source).TrimStart('\n', ' ');

            var yuiResultType = new MinificationType(result, "YuiMinifier") {
                MinifiedContent = yuiContent,
                MinifiedSize = yuiContent.Length,
            };

            result.Types = new[] { jsMinResultType, msResultType, yuiResultType, };
            return result;
        }
    }
}