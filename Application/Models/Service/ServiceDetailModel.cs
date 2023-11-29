using Application.Common.Models;
using Domain.Entities;

namespace Application.Models.Service;

public class ServiceDetailModel : AuditModel
{
    public string ServiceName { get; set; }
    public string ServiceCode { get; set; }
    public double Price { get; set; }
}