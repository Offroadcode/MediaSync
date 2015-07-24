using System.Net;
using System.Runtime.InteropServices;
using Orc.MediaSync.Shared.Helpers;
using Orc.MediaSync.Shared.Models;
using RestSharp;

namespace Orc.MediaSync.Client
{
    public class MediaServerClient
    {
        private RestClient Client;
        public MediaServerClient(string host, string apiKey)
        {
            Client = new RestClient(host+"/api/Media/");
            Client.AddDefaultHeader("ApiKey",apiKey);
        }

        public ServerInformation GetServerInformation()
        {
            var request = new RestRequest("Information");
            var results = Client.Get<ServerInformation>(request);
            return results.Data;
        }

        public MediaItemCollection GetFiles(string projectName)
        {
            var request = new RestRequest("ListMediaItems");
            request.Timeout = 3600000;
            request.AddParameter("project", projectName);

            var results =  Client.Get<MediaItemCollection>(request);
            return results.Data;
        }

        public void UploadFile(string path, string projectName, MediaItem mediaItem)
        {

            var fullPath = path + mediaItem.Filename;
            var request = new RestRequest("UploadMediaItem");
            request.AddParameter("path", mediaItem.Filename);
            
            request.AddParameter("project", projectName);

            request.AddFile(mediaItem.Filename, fullPath);
            var res=Client.Post(request);
        }

        public byte[] DownloadFile(string remotePath)
        {
            var request = new RestRequest("DownloadFile");
            request.AddParameter("file", remotePath);

            var compressedData = Client.DownloadData(request);
            var uncompressed = Compression.Unzip(compressedData);

            return uncompressed;
        }
    }
}
