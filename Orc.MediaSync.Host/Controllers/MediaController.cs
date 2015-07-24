using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;
using Orc.MediaSync.Host.Filters;
using Orc.MediaSync.Shared.Helpers;
using Orc.MediaSync.Shared.Models;

namespace Orc.MediaSync.Host.Controllers
{
    [CheckForAuthenticationHeader]
    public class MediaController : ApiController
    {

        /// <summary>
        /// Returns a list of assets which are in the specified project
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        [HttpGet]
        public MediaItemCollection ListMediaItems(string project)
        {
            var collection = MediaItemCollection.PopulateFromPath(HttpContext.Current.Server.MapPath("~/Projects/" + project));
            return collection;
        }

        [HttpGet]
        public HttpResponseMessage DownloadFile(string file)
        {
            var path = HttpUtility.UrlDecode(file);
            var fullPath = HttpContext.Current.Server.MapPath("~/Projects/" + path);
            if (!File.Exists(fullPath))
            {
                var fileName = Path.GetFileName(fullPath);
                fileName = fileName.Replace(" ", "%20");
                var encoded = HttpUtility.UrlEncode(fileName);
                fullPath = fullPath.Replace(fileName, encoded);
            }

            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
            
            var uncompressed = File.ReadAllBytes(fullPath);
            var compressed = Compression.Compress(uncompressed);

            result.Content = new ByteArrayContent(compressed);// = new StreamContent(stream);
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

            return result;
        }

        /// <summary>
        /// uploads the specified media assets
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage UploadMediaItem()
        {
            HttpResponseMessage result = null;

            var httpRequest = HttpContext.Current.Request;

            string project = httpRequest["project"];

            string fullPath = httpRequest["path"];

            if (httpRequest.Files.Count > 0)
            {
                var docfiles = new List<string>();
                foreach (string file in httpRequest.Files)
                {
                    var postedFile = httpRequest.Files[file];
                    var filePath = HttpContext.Current.Server.MapPath("~/Projects/" + project + "/" + fullPath);

                    //ensure the directory exists
                    Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                    
                    postedFile.SaveAs(filePath);

                    docfiles.Add(filePath);
                }
                result = Request.CreateResponse(HttpStatusCode.Created, docfiles);
            }
            else
            {
                result = Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            return result;
        }
    }
}