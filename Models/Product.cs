using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MigrationTestApp.Models
{
    public class Product
    {
        public long Id { get; set; }

        [Required, StringLength(50)]
        public string Sku { get; set; } = string.Empty;

        [Required, StringLength(150)]
        public string Name { get; set; } = string.Empty;

        [Column(TypeName = "decimal(12,2)")]
        public decimal Price { get; set; }

        [StringLength(50)]
        public string Category { get; set; } = string.Empty;
    }
}