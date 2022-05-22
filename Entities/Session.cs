namespace RaFilDaAPI.Entities
{
    public record Session
    {
        public int id { get; set; }
        public string name { get; set; }
        public string token { get; set; }
    }
}