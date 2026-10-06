using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignABear.Repositories.Models
{
    [Table("product_dynamic_stats")]
    public class ProductDynamicStats
    {
        [Key]
        [Column("product_id")]
        public Guid ProductId { get; set; }

        [Column("view_count_in_10min")]
        public long ViewCountIn10Min { get; set; } = 0;

        [Column("discount_rate")]
        public double DiscountRate { get; set; } = 0;

        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; } = null!;
    }
}
