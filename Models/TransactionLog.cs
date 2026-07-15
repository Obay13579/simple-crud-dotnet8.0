namespace MigrationTestApp.Models
{
    public class TransactionLog
    {
        public long Id { get; set; }
        public long OrderId { get; set; }
        public string EventType { get; set; } = string.Empty;
        public string EventPayload { get; set; } = "{}";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}