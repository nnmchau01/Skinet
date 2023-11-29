using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Promotion : Audit
    {
        [Required] public string Code { get; set; }
        public int DiscountPercent { set; get; }
    }
}