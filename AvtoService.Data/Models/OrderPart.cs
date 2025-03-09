using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvtoService.Data.Models
{
    public class OrderPart
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("order_id")]
        public int OrderId { get; set; }

        [Column("part_id")]
        public int PartId { get; set; }

        [Column("quantity")]
        public int Quantity { get; set; } = 1;
        public Order Order { get; set; }
        public Part Part { get; set; }
    }
}
