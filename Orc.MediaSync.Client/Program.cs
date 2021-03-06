﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using NuSelfUpdate;

namespace Orc.MediaSync.Client
{
    class Program
    {
        private static string _host;
        private static string _project;
        private static string _path;
        private static string _apiKey;

        static void Main(string[] args)
        {
            var filename = "Project.xml";

            if (args.Any(x => x.ToLower().StartsWith("filename=")))
            {
                filename = args.First(x => x.ToLower().StartsWith("filename=")).ToLower().Replace("filename=", "");
            }



            if (!File.Exists(filename))
            {
                var defaultcolor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("No project.xml found!");
                Console.ForegroundColor = defaultcolor;
            }
            else
            {
                Console.WriteLine("Offroadcode MediaSync");
                Console.WriteLine("Using server " + _host);
                var client = new MediaServerClient(_host, _apiKey);

                var serverInformation = client.GetServerInformation();
                Console.WriteLine("Host is using version: " + serverInformation.ServerVersion);
                var isLatestVersion = Updater.Check(serverInformation);

                if (isLatestVersion)
                {
                    Console.WriteLine("Project: " + _project);
                    if (args.Contains("up"))
                    {
                        Console.WriteLine("Syncing Up!");
                        Actions.SyncMedia.SyncUp(client, _path, _project);
                    }
                    else
                    {
                        Console.WriteLine("Syncing Down!");
                        Actions.SyncMedia.SyncDown(client, _path, _project);
                    }
                }
            }
        }

        private static void LoadProject(string filename)
        {
            XmlDocument document = new XmlDocument();
            document.Load(filename);
            _host = document.SelectSingleNode("/Project/MediaSync/Host").InnerText;
            _project = document.SelectSingleNode("/Project/MediaSync/Name").InnerText;
            _apiKey = document.SelectSingleNode("/Project/MediaSync/ApiKey").InnerText;

            var currentDir = Directory.GetCurrentDirectory();
            _path = currentDir + document.SelectSingleNode("/Project/MediaSync/MediaDirectory").InnerText;
        }
    }
}
