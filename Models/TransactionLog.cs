using System.ComponentModel.DataAnnotations.Schema;

namespace MigrationTestApp.Models
{
    [Table("transaction_log")]
    public class TransactionLog
    {
        public long Id { get; set; }

        [Column("order_id")]
        public long OrderId { get; set; }

        [Column("event_type")]
        public string EventType { get; set; } = string.Empty;

        [Column("event_payload")]
        public string EventPayload { get; set; } = "{}";

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}