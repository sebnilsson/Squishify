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

            IMinifier<JavaScriptBundle> jsMinifier;
            switch((minifier ?? string.Empty).ToLowerInvariant()) {
                //case "closure":
                //    jsMinifier = new SquishIt.Framework.Minifiers.JavaScript.ClosureMinifier();
                //    break;
                case "jsmin":
                    jsMinifier = new SquishIt.Framework.Minifiers.JavaScript.JsMinMinifier();
                    break;
                case "msmin":
                    jsMinifier = new SquishIt.Framework.Minifiers.JavaScript.MsMinifier();
                    break;
                case "yui":
                    jsMinifier = new SquishIt.Framework.Minifiers.JavaScript.YuiMinifier();
                    break;
                default:
                    jsMinifier = new SquishIt.Framework.Minifiers.JavaScript.NullMinifier();
                    break;
            }

            string minifiedContent = jsMinifier.Minify(source).TrimStart('\n');

            return new MinificationResult {
                MinifiedContent = minifiedContent,
                MinifiedSize = minifiedContent.Length,
                Minifier = jsMinifier.GetType().Name,
                OriginalSize = source.Length,
            };
        }
    }
}