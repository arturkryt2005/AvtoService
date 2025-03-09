using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvtoService.Data.Models
{
    public class OrderService
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("order_id")]
        public int OrderId { get; set; }

        [Column("service_id")]
        public int ServiceId { get; set; }

        [Column("quantity")]
        public int Quantity { get; set; } = 1;
        public Order Order { get; set; }
        public Service Service { get; set; }
    }
}
