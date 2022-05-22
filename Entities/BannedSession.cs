namespace RaFilDaAPI.Entities
{
    public record BannedSession
    {
        public int id { get; set; }
        public string token { get; set; }
    }
}