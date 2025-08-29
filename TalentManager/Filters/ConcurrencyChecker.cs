using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Collections.Concurrent;
using System.Net.Http.Headers;
//using Microsoft.Net.Http.Headers;

namespace TalentManager.Filters
{
    public class ConcurrencyChecker : ActionFilterAttribute
    {
        private static ConcurrentDictionary<string, EntityTagHeaderValue> etags = new();

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var request = context.HttpContext.Request;

            if(request.Method == "PUT")
            {
                var key = request.Path.ToString();

                var ifMatchHeader = request.Headers["If-Match"].FirstOrDefault();

                if (!string.IsNullOrEmpty(ifMatchHeader))
                {
                    EntityTagHeaderValue? etagFromClient;
                    if (EntityTagHeaderValue.TryParse(ifMatchHeader, out etagFromClient))
                    {
                        if (etags.TryGetValue(key, out var currentEtag))
                        {
                            if (!currentEtag.Equals(etagFromClient))
                            {
                                context.Result = new StatusCodeResult(StatusCodes.Status412PreconditionFailed);
                                return;
                            }
                        }
                    }
                } else
                {
                    Console.WriteLine("Enforce Missing If-Match Header Here");
                    context.Result = new BadRequestObjectResult("Missing If-Match header");
                    //context.Result = new StatusCodeResult(428);
                }
            }
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            var request = context.HttpContext.Request;
            var response = context.HttpContext.Response;
            var key = request.Path.ToString();

            EntityTagHeaderValue? etag;

            if(!etags.TryGetValue (key, out etag) || request.Method == "PUT" || request.Method == "POST")
            {
                etag = new EntityTagHeaderValue("\"" + Guid.NewGuid().ToString() + "\"");
                etags.AddOrUpdate(key, etag, (k, val) => etag);
            }

            response.Headers["ETag"] = etag.ToString();
        }
    }
}
