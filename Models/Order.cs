using System.ComponentModel.DataAnnotations.Schema;

namespace MigrationTestApp.Models
{
    [Table("orders")]
    public class Order
    {
        public long Id { get; set; }

        [Column("customer_id")]
        public long CustomerId { get; set; }
        public Customer? Customer { get; set; }

        [Column("order_date")]
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        public string Status { get; set; } = "Pending";

        [Column("total_amount", TypeName = "decimal(14,2)")]
        public decimal TotalAmount { get; set; }

        [Column("storage_blob_ref")]
        public string? StorageBlobRef { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}