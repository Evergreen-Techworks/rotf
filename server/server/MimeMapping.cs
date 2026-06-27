using System;
using System.Collections.Generic;
using System.IO;

namespace server
{
    // Drop-in replacement for System.Web.MimeMapping (removed in .NET Core/8).
    // Maps a web file's extension to a content type for serving static assets.
    internal static class MimeMapping
    {
        private static readonly Dictionary<string, string> Map =
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { ".mp3",  "audio/mpeg" },
            { ".wav",  "audio/wav" },
            { ".ogg",  "audio/ogg" },
            { ".xml",  "text/xml" },
            { ".json", "application/json" },
            { ".html", "text/html" },
            { ".htm",  "text/html" },
            { ".js",   "application/javascript" },
            { ".css",  "text/css" },
            { ".txt",  "text/plain" },
            { ".png",  "image/png" },
            { ".jpg",  "image/jpeg" },
            { ".jpeg", "image/jpeg" },
            { ".gif",  "image/gif" },
            { ".swf",  "application/x-shockwave-flash" },
        };

        public static string GetMimeMapping(string fileName)
        {
            var ext = Path.GetExtension(fileName ?? string.Empty);
            return Map.TryGetValue(ext, out var type) ? type : "application/octet-stream";
        }
    }
}
