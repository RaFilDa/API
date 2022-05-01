using System;

namespace RaFilDaAPI.Entities
{
    public record Config
    {
        public int Id { get; init; }
        public string Name { get; set; }
        public int RetentionSize { get; set; }
        public string BackupFrequency { get; set; }
        public string Cron { get; set; }
        public string TimeZone { get; set; }
        public int PackageSize { get; set; }
        public int BackupType { get; set; }
        public bool FileType { get; set; }
    }
}