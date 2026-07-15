using System.ComponentModel.DataAnnotations.Schema;

namespace MigrationTestApp.Models
{
    [Table("order_items")]
    public class OrderItem
    {
        public long Id { get; set; }

        [Column("order_id")]
        public long OrderId { get; set; }
        public Order? Order { get; set; }

        [Column("product_id")]
        public long ProductId { get; set; }
        public Product? Product { get; set; }

        public int Quantity { get; set; }

        [Column("unit_price", TypeName = "decimal(12,2)")]
        public decimal UnitPrice { get; set; }
    }
}