using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NuSelfUpdate;

namespace Orc.MediaSync.Client
{
    public static class Updater
    {
        public static bool Check()
        {
            var appUpdater = new AppUpdaterBuilder("Orc.MediaSync.Client")
                                .SourceUpdatesFrom("http://nuget.offroadcode.com/")
                                .Build();
            
            // This will run the UpdateDatabase method if this is the first 
            // time we have run the application after an update is installed.
            if (appUpdater.OldVersionExists)
            {
                appUpdater.RemoveOldVersionFiles();
            }
            Console.WriteLine("Current Version:"+appUpdater.CurrentVersion);
            Console.WriteLine("Checking for updates..."); 
            var updateCheck = appUpdater.CheckForUpdate();

            if (updateCheck.UpdateAvailable) 
            {
                Console.WriteLine("Update found, updating to " + updateCheck.UpdatePackage.Version);    
                var preparedUpdate = appUpdater.PrepareUpdate(updateCheck.UpdatePackage);
                var installedUpdate = appUpdater.ApplyPreparedUpdate(preparedUpdate);
                // Runs the new version of the application with the same command
                // line arguments that we were initially given.
                appUpdater.LaunchInstalledUpdate(installedUpdate);
                return false;
            }
            Console.WriteLine("No Updates available!"); 
            return true;
        }
    }
}
