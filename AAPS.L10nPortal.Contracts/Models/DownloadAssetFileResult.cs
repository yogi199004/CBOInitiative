﻿namespace CAPPortal.Contracts.Models
{
    public class DownloadAssetFileResult
    {
        public string FileName { get; set; }
        public Stream FileContent { get; set; }
    }
}
