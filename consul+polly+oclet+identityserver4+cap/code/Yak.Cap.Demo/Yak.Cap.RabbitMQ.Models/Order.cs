using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yak.Cap.RabbitMQ.Models
{
    [Table("Orders")]
    public class Order
    {
        [Key, MaxLength(50)]
        public string Id { get; set; }
        [MaxLength(50), Required]
        public string SkuName { get; set; }
        [Required]
        public double SkuId { get; set; }
        [Required]
        public int Num { get; set; }
    }
}
