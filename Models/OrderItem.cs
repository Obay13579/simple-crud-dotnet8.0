using System.ComponentModel.DataAnnotations.Schema;

namespace MigrationTestApp.Models
{
    public class OrderItem
    {
        public long Id { get; set; }

        public long OrderId { get; set; }
        public Order? Order { get; set; }

        public long ProductId { get; set; }
        public Product? Product { get; set; }

        public int Quantity { get; set; }

        [Column(TypeName = "decimal(12,2)")]
        public decimal UnitPrice { get; set; }
    }
}