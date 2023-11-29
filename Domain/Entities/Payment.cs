using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Payment : Audit
    {
        [Required] public decimal Amount { get; set; }
        [Required] public string PaymentMethod { get; set; }
        [Required] public string TransactionId { get; set; }
        [Required] public string PaymentCode { get; set; }
        public DateTime TransactionDate { set; get; }
        public decimal Fee { set; get; }
        public Guid StatusId { get; set; }
    }
}