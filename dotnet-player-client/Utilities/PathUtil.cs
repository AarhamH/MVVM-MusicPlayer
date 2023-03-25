using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotnet_player_client.Utilities
{
    public static class PathUtil
    {
        public static bool HasOneOfExtensions(string path, params string[] extensions)
        {
            foreach (string extension in extensions)
            {
                if (Path.GetExtension(path) == extension)
                {
                    return true;
                }
            }
            return false;
        }

        public static bool HasAudioVideoExtensions(string path)
        {
            if (HasOneOfExtensions(path, ".wav", ".mp3", ".mp4", ".mov", ".m4a", ".m4v", ".mpg", ".mpeg", ".wmv", ".avi")) return true;
            return false;
        }
    }
}
