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

            if(string.IsNullOrWhiteSpace(source)) {
                throw new ArgumentNullException("source");
            }

            var result = new MinificationResult {
                OriginalSize = source.Length,
            };

            var ms = new SquishIt.Framework.Minifiers.CSS.MsCompressor();
            var msContent = ms.Minify(source).TrimStart('\n', ' ');

            var msResultType = new MinificationType(result, "MsCompressor") {
                MinifiedContent = msContent,
                MinifiedSize = msContent.Length,
            };

            var yui = new Yahoo.Yui.Compressor.CssCompressor {
                CompressionType = Yahoo.Yui.Compressor.CompressionType.Standard,
                RemoveComments = true,
            };
            var yuiContent = yui.Compress(source).TrimStart('\n', ' ');

            var yuiResultType = new MinificationType(result, "YuiCompressor") {
                MinifiedContent = yuiContent,
                MinifiedSize = yuiContent.Length,
            };

            result.Types = new[] { msResultType, yuiResultType, };
            return result;
        }
    }
}