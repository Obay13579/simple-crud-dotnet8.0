using System.ComponentModel.DataAnnotations.Schema;

namespace MigrationTestApp.Models
{
    public class Order
    {
        public long Id { get; set; }

        public long CustomerId { get; set; }
        public Customer? Customer { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        public string Status { get; set; } = "Pending";

        [Column(TypeName = "decimal(14,2)")]
        public decimal TotalAmount { get; set; }

        public string? StorageBlobRef { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}