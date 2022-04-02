using System;

namespace RaFilDaAPI.Entities
{
    public record CompConf
    {
        public int Id { get; init; }
        public int ConfigID { get; set; }
        public int CompID { get; set; }
        public bool Updated { get; set; }
    }
}