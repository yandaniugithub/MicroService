using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Yak.Cap.RabbitMQ.Models
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SkuId { get; set; }

        /// <summary>
        /// SKU名称
        /// </summary>
        [Required]
        [Column(TypeName = "VARCHAR(50)")]
        public string SkuName { get; set; }

        /// <summary>
        /// 库存
        /// </summary>
        [Required]
        public int Stock { get; set; }
    }
}
