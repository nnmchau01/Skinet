using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Order : Audit
    {
        [Required] [MaxLength(500)] public string Code { get; set; }
        [Required] [MaxLength(500)] public string CustomerName { get; set; }
        [Required] [MaxLength(500)] public string Address { get; set; }
        [Required] [MaxLength(100)] public string PhoneNumber { get; set; }
        [Required] public decimal TotalAmount { get; set; }
        [MaxLength(1000)] public string? Note { get; set; }
        [MaxLength(1000)] public string? CancelReason { get; set; }
        [Required] public Guid PaymentId { get; set; }
        [Required] public Guid StatusId { get; set; }
    }
}