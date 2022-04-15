using System;

namespace RaFilDaAPI.Entities
{
    public record ReportDetail
    {
        public int id { get; set; }
        public DateTime date { get; set; }
        public string mac { get; set; }
        public string name { get; set; }
        public string backup { get; set; }
        public bool state { get; set; }
        public string message { get; set; }
    }
}