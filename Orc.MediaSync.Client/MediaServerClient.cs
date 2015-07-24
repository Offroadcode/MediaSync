using Orc.MediaSync.Shared.Models;
using RestSharp;

namespace Orc.MediaSync.Client
{
    public class MediaServerClient
    {
        private RestClient Client;
        public MediaServerClient(string host, string projectName, string apiKey)
        {
            Client = new RestClient(host+"/api/Media/");
            Client.AddDefaultParameter("project",projectName);
            Client.AddDefaultHeader("ApiKey",apiKey);
        }

        public MediaItemCollection GetFiles()
        {
            var request = new RestRequest("ListMediaItems");
            request.Timeout = 3600000;
            var results =  Client.Get<MediaItemCollection>(request);
            return results.Data;
        }

        public void UploadFile(string path, MediaItem mediaItem)
        {

            var fullPath = path + mediaItem.Filename;
            var request = new RestRequest("UploadMediaItem");
            request.AddParameter("path", mediaItem.Filename);
            request.AddFile(mediaItem.Filename, fullPath);
            var res=Client.Post(request);
        }
    }
}
