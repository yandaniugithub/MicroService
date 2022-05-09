using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yak.Cap.RabbitMQ.Models
{
    [Table("Sys_User")]
    public class Sys_User
    {
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        
        public string Name { get; set; }
        /// <summary>
        /// 手机号码
        /// </summary>
        public string C_Mobile { get; set; }
    }
}
