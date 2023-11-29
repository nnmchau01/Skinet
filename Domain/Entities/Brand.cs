using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Brand : Audit
    {
        [Required] [MaxLength(500)] public string Name { get; set; }
        [Required] [MaxLength(255)] public string Code { get; set; }
    }
}