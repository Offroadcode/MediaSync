using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Configuration;
using System.Web.Mvc;
using Orc.MediaSync.Host.ActionResults;

namespace Orc.MediaSync.Host.Filters
{
    public class CheckForAuthenticationHeaderAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var req = filterContext.HttpContext.Request;
            var suppliedApiKey = req.Headers["ApiKey"];
            var correctApiKey = WebConfigurationManager.AppSettings["ApiKey"];
            
            if (!String.IsNullOrEmpty(suppliedApiKey))
            {
                if (correctApiKey == suppliedApiKey)
                {
                    return;
                }
            }
            
            //fallen through the checks, so lets return an error!
            filterContext.Result = new HttpStatusContentResult("Missing or invalid ApiKey Header", HttpStatusCode.Unauthorized);
        }
    }
}