using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Property : Audit
    {
        [Required] public string Name { get; set; }
        [Required] public string Value { get; set; }
        [Required] public Guid ProductId { get; set; }
    }
}