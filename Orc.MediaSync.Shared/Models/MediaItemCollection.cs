using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Orc.MediaSync.Shared.Models
{
    public class MediaItemCollection
    {
        public MediaItemCollection()
        {
            Items = new List<MediaItem>();
        }

        public List<MediaItem> Items { get; set; }

        public static MediaItemCollection PopulateFromPath(string mapPath)
        {
            var collection = new MediaItemCollection();
            
            Directory.CreateDirectory(mapPath);
            
            var files = Directory.GetFiles(mapPath, "*", SearchOption.AllDirectories);
        
            foreach (var file in files)
            {
                if (!file.Contains("\\Cached\\"))
                {
                    collection.Items.Add(MediaItem.FromPath(file, mapPath));
                }
            }
            
            return collection;
        }

        public MediaItemCollection FindMissingFiles(MediaItemCollection localCollection)
        {
            var missing = new MediaItemCollection();
           
            foreach (var mediaItem in Items)
            {
                if(!localCollection.ContainsFile(mediaItem.Filename))
                {
                    missing.Items.Add(mediaItem);
                }
            }
            
            return missing;
        }

        public MediaItemCollection FindChangedFiles(MediaItemCollection localCollection)
        {
            var listOfDifferent = new MediaItemCollection();
            foreach (MediaItem mediaItem in Items)
            {
                var item = localCollection.FindByFilename(mediaItem.Filename);
                if (item != null)
                {
                    if (item.MD5 != mediaItem.MD5)
                    {
                        listOfDifferent.Items.Add(item);
                    }
                }
            }
            return listOfDifferent;
        }

        private bool ContainsFile(string filename)
        {
            return this.Items.Any(x => x.Filename == filename);
        }

        private MediaItem FindByFilename(string filename)
        {
            return this.Items.FirstOrDefault(x => x.Filename == filename);
        }
    }
}