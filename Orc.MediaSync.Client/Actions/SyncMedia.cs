using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using NuGet;
using Orc.MediaSync.Client.Helpers;
using Orc.MediaSync.Shared.Helpers;
using Orc.MediaSync.Shared.Models;
using HttpUtility = RestSharp.Contrib.HttpUtility;

namespace Orc.MediaSync.Client.Actions
{
    public static class SyncMedia
    {
        public static void SyncUp(MediaServerClient client, string path, string projectName)
        {
              
            Console.WriteLine("Loading local files");
            var localCollection = MediaItemCollection.PopulateFromPath(path);

            Console.WriteLine("Finding remote files");
            var remoteCollection = client.GetFiles(projectName);

            Console.WriteLine("Calculating on remote");
            var missingFiles = localCollection.FindMissingFiles(remoteCollection);
            
            Console.WriteLine("Calculating Changed");
            var changedFiles = localCollection.FindChangedFiles(remoteCollection);

            Console.WriteLine(missingFiles.Items.Count + " missing, " + changedFiles.Items.Count + " changed");
            UploadFiles(missingFiles, changedFiles, path, client, projectName);
        }

        private static void UploadFiles(MediaItemCollection missingFiles, MediaItemCollection changedFiles, string path, MediaServerClient mediaClient, string projectName)
        {
            var items = missingFiles.Items;
            items.AddRange(changedFiles.Items);
            
            var done = 0;
            var total = missingFiles.Items.Count + changedFiles.Items.Count;
            Console.WriteLine("Uploading " + total + "files " + (missingFiles.Items.Sum(x => x.FileSize) + changedFiles.Items.Sum(x => x.FileSize) + "bytes"));
            foreach (MediaItem mediaItem in items)
            {
                var localPath = path + mediaItem.Filename;
                Directory.CreateDirectory(Path.GetDirectoryName(localPath));
                mediaClient.UploadFile(path, projectName, mediaItem);
                 done++;
                UpdateStatus(done, total);
            }
        }

        public static void SyncDown(MediaServerClient client, string path, string projectName)
        {
            Console.WriteLine("Loading local files");
            var localCollection = MediaItemCollection.PopulateFromPath(path);

            Console.WriteLine("Finding remote files");
            var remoteCollection = client.GetFiles(projectName);

            Console.WriteLine("Calculating missing");
            var missingFiles = remoteCollection.FindMissingFiles(localCollection);

            Console.WriteLine("Calculating Changed");
            var changedFiles = remoteCollection.FindChangedFiles(localCollection);

            Console.WriteLine(missingFiles.Items.Count + " missing, " + changedFiles.Items.Count + " changed");
            DownloadFiles(missingFiles, changedFiles, client, projectName, path);
        }

        private static void DownloadFiles(MediaItemCollection missingFiles, MediaItemCollection changedFiles, MediaServerClient client, string projectName, string path)
        {
            var items = missingFiles.Items;
            items.AddRange(changedFiles.Items);
           
            var done = 0;
            var total = missingFiles.Items.Count + changedFiles.Items.Count;
            var totalSize = (missingFiles.Items.Sum(x => x.FileSize) + changedFiles.Items.Sum(x => x.FileSize));
            Console.WriteLine("Downloading "+total+"files "+totalSize.ToFileSize()+"bytes");
            foreach (MediaItem mediaItem in items)
            {
                if (!mediaItem.Filename.EndsWith(".config"))
                {
                    
                    var localPath = path + mediaItem.Filename;
                        Directory.CreateDirectory(Path.GetDirectoryName(localPath));
                        var remotePath = HttpUtility.UrlEncode(projectName + "/" + mediaItem.Filename);
                    
                    try
                    {
                        var compressed = client.DownloadFile(remotePath);
                        var uncompressed = Compression.Unzip(compressed);
                        File.WriteAllBytes(localPath, uncompressed);
                        done++;
                        UpdateStatus(done, total);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Error downloading " + remotePath);
                    }
                }
            }
        }




        private static void UpdateStatus(int done, int total)
        {
             var percent = Math.Round(((double)done /(double) total)*100,0);
            var ending = "["+percent+"%|"+done+"/"+total+"]";

            var progressBar = CalculateProgressBar(percent);
            var text = "\r" + progressBar.PadRight(Console.BufferWidth-3 - ending.Length) + ending;
            Console.Write(text);
            
        }

        private static string CalculateProgressBar(double percent)
        {
            string str = "[";
            var charsToUse = percent/2;
            for (int i = 0; i < charsToUse; i++)
            {
                str = str + "#";
            }
            for (int i = 0; i < 50-charsToUse; i++)
            {
                str = str + " ";
            }
            return str+"]";
        }

       
    }
}
