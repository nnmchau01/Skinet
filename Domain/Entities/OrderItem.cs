using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class OrderItem : Audit
    {
        [Required] public int Quantity { get; set; }
        [Required] public decimal Price { set; get; }
        public Guid OrderId { get; set; }
        public Guid ProductId { get; set; }
        public Guid PromotionId { get; set; }
    }
}