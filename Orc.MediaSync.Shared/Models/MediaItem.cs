using Orc.MediaSync.Shared.Helpers;

namespace Orc.MediaSync.Shared.Models
{
    public class MediaItem
    {
        public MediaItem()
        {
            
        }

        public string Filename { get; set; }
        public string MD5 { get; set; }
        public long FileSize { get; set; }

        public static MediaItem FromPath(string file, string root)
        {
            var fileInfo = new System.IO.FileInfo(file);
            var mediaItem = new MediaItem();
            mediaItem.Filename = file.Replace(root, "");
            mediaItem.FileSize = fileInfo.Length;
            mediaItem.MD5 = MD5Helper.Hash(file);
            return mediaItem;
        }
    }
}
