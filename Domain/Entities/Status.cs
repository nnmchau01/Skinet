using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Status : Audit
    {
        [Required] public string Type { get; set; }
        [Required] public string Display { get; set; }
        [Required] public string Code { get; set; }
    }
}