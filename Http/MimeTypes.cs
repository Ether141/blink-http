namespace BlinkHttp.Http
{
    public static class MimeTypes
    {
        // Text
        public const string TextPlain = "text/plain";
        public const string TextHtml = "text/html";
        public const string TextCss = "text/css";
        public const string TextJavascript = "text/javascript";
        public const string TextCsv = "text/csv";

        // Image
        public const string ImagePng = "image/png";
        public const string ImageJpeg = "image/jpeg";
        public const string ImageGif = "image/gif";
        public const string ImageSvgXml = "image/svg+xml";
        public const string ImageWebp = "image/webp";
        public const string ImageIcon = "image/x-icon";

        // Application
        public const string ApplicationJson = "application/json";
        public const string ApplicationXml = "application/xml";
        public const string ApplicationPdf = "application/pdf";
        public const string ApplicationZip = "application/zip";
        public const string ApplicationOctetStream = "application/octet-stream";
        public const string ApplicationRtf = "application/rtf";
        public const string ApplicationXWwwFormUrlencoded = "application/x-www-form-urlencoded";
        public const string ApplicationJavascript = "application/javascript";
        public const string ApplicationWasmtime = "application/wasm";
        public const string ApplicationMsWord = "application/msword";
        public const string ApplicationVndMsExcel = "application/vnd.ms-excel";
        public const string ApplicationVndMsPowerpoint = "application/vnd.ms-powerpoint";
        public const string ApplicationVndOpenxmlformatsOfficedocumentWordprocessingmlDocument = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
        public const string ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        public const string ApplicationVndOpenxmlformatsOfficedocumentPresentationmlPresentation = "application/vnd.openxmlformats-officedocument.presentationml.presentation";

        // Audio
        public const string AudioMpeg = "audio/mpeg";
        public const string AudioOgg = "audio/ogg";
        public const string AudioWav = "audio/wav";
        public const string AudioWebm = "audio/webm";

        // Video
        public const string VideoMp4 = "video/mp4";
        public const string VideoWebm = "video/webm";
        public const string VideoOgg = "video/ogg";

        // Font
        public const string FontWoff = "font/woff";
        public const string FontWoff2 = "font/woff2";
        public const string FontTtf = "font/ttf";
        public const string FontOtf = "font/otf";

        // Archive
        public const string ApplicationGzip = "application/gzip";
        public const string ApplicationTar = "application/x-tar";
        public const string ApplicationRar = "application/vnd.rar";

        // JSON
        public const string ApplicationProblemJson = "application/problem+json";

        // Multipart
        public const string MultipartFormData = "multipart/form-data";

        public static readonly Dictionary<string, string> ExtensionToMimeType = new(StringComparer.OrdinalIgnoreCase)
        {
            { ".txt", TextPlain },
            { ".html", TextHtml },
            { ".htm", TextHtml },
            { ".css", TextCss },
            { ".js", ApplicationJavascript },
            { ".json", ApplicationJson },
            { ".xml", ApplicationXml },
            { ".csv", TextCsv },
            { ".pdf", ApplicationPdf },
            { ".zip", ApplicationZip },
            { ".tar", ApplicationTar },
            { ".gz", ApplicationGzip },
            { ".rar", ApplicationRar },
            { ".rtf", ApplicationRtf },

            { ".png", ImagePng },
            { ".jpg", ImageJpeg },
            { ".jpeg", ImageJpeg },
            { ".gif", ImageGif },
            { ".svg", ImageSvgXml },
            { ".ico", ImageIcon },
            { ".webp", ImageWebp },

            { ".mp3", AudioMpeg },
            { ".ogg", AudioOgg },
            { ".wav", AudioWav },

            { ".mp4", VideoMp4 },
            { ".webm", VideoWebm },
            { ".ogv", VideoOgg },

            { ".woff", FontWoff },
            { ".woff2", FontWoff2 },
            { ".ttf", FontTtf },
            { ".otf", FontOtf },

            { ".doc", ApplicationMsWord },
            { ".xls", ApplicationVndMsExcel },
            { ".xlsx", ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet },
            { ".docx", ApplicationVndOpenxmlformatsOfficedocumentWordprocessingmlDocument },
            { ".ppt", ApplicationVndMsPowerpoint },
            { ".pptx", ApplicationVndOpenxmlformatsOfficedocumentPresentationmlPresentation }
        };

        public static string? GetMimeTypeForExtension(string extension)
        {
            if (string.IsNullOrWhiteSpace(extension))
            {
                return null;
            }

            if (!extension.StartsWith('.'))
            {
                extension = "." + extension;
            }

            return ExtensionToMimeType.TryGetValue(extension, out string? mimeType) ? mimeType : null;
        }
    }

}
