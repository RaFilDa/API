using System.Collections.Generic;

namespace RaFilDaAPI.Entities
{
    public record DaemonInfo
    {
        public Config Config { get; set; }
        public List<Destination> Destinations { get; set; }
        public List<Source> Sources { get; set; }
    }
}