using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Image : Audit
    {
        [MaxLength(500)] public string? OriginLinkImage { get; set; }
        [MaxLength(500)] public string? LocalLinkImage { get; set; }
        public Guid ProductId { get; set; }
    }
}