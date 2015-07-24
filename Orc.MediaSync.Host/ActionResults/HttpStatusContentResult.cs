﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Orc.MediaSync.Host.ActionResults
{

    /// <summary>
    /// from http://stackoverflow.com/a/17324823
    /// </summary>
    public class HttpStatusContentResult : ActionResult
    {
        private string _content;
        private HttpStatusCode _statusCode;
        private string _statusDescription;

        public HttpStatusContentResult(string content,
                                       HttpStatusCode statusCode = HttpStatusCode.OK,
                                       string statusDescription = null)
        {
            _content = content;
            _statusCode = statusCode;
            _statusDescription = statusDescription;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            var response = context.HttpContext.Response;
            response.StatusCode = (int)_statusCode;
            if (_statusDescription != null)
            {
                response.StatusDescription = _statusDescription;
            }

            if (_content != null)
            {
                context.HttpContext.Response.Write(_content);
            }
        }
    }
}