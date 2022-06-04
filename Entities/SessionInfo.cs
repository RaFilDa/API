namespace RaFilDaAPI.Entities
{
    public record SessionInfo
    {
        public Session session { get; set; }
        public bool expired { get; set; }
    }
}