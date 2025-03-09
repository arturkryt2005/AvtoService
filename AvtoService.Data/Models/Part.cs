using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvtoService.Data.Models
{
    public class Part
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("cost")]
        public decimal Cost { get; set; }
        public ICollection<OrderPart> OrderParts { get; set; }
    }
}
