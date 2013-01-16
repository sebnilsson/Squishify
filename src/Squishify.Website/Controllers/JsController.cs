using System;
using System.Web.Http;

using Newtonsoft.Json.Linq;
using Squishify.Website.Models;

namespace Squishify.Website.Controllers
{
    public class JsController : ApiController
    {
        [HttpPost]
        public MinificationResult Get(JObject jsonData)
        {
            dynamic json = jsonData;
            string source = json.source;

            if (string.IsNullOrWhiteSpace(source))
            {
                throw new ArgumentNullException("source");
            }

            var result = new MinificationResult { OriginalSize = new FileSize(source.Length), };

            //var closureMin = new SquishIt.Framework.Minifiers.JavaScript.ClosureMinifier();
            //string closeureMinContent = closureMin.Minify(source).TrimStart('\n', ' ');

            //var closureResultType = new MinificationType(result, "JsMinMinifier")
            //{
            //    MinifiedContent = closeureMinContent,
            //    MinifiedSize = new FileSize(closeureMinContent.Length),
            //};

            var jsMin = new SquishIt.Framework.Minifiers.JavaScript.JsMinMinifier();
            string jsMinContent = jsMin.Minify(source).TrimStart('\n', ' ');

            var jsMinResultType = new MinificationType(result, "JsMinMinifier")
                                      {
                                          MinifiedContent = jsMinContent,
                                          MinifiedSize =
                                              new FileSize(jsMinContent.Length),
                                      };

            var ms = new SquishIt.Framework.Minifiers.JavaScript.MsMinifier();
            string msConfig = ms.Minify(source).TrimStart('\n', ' ');

            var msResultType = new MinificationType(result, "MsMinifier")
                                   {
                                       MinifiedContent = msConfig,
                                       MinifiedSize = new FileSize(msConfig.Length),
                                   };

            var yui = new Yahoo.Yui.Compressor.JavaScriptCompressor
                          {
                              CompressionType =
                                  Yahoo.Yui.Compressor.CompressionType
                                       .Standard,
                              DisableOptimizations = false,
                          };
            string yuiContent = yui.Compress(source).TrimStart('\n', ' ');

            var yuiResultType = new MinificationType(result, "YuiMinifier")
                                    {
                                        MinifiedContent = yuiContent,
                                        MinifiedSize =
                                            new FileSize(yuiContent.Length),
                                    };

            result.Types = new[] { jsMinResultType, msResultType, yuiResultType, }; // closureResultType
            return result;
        }
    }
}