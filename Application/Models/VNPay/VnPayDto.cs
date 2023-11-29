namespace Application.Models.VnPay
{
    public class VnPayDto
    {
        public int VnpAmount { get; set; }
        public string VnpBankCode { get; set; }
        public string VnpOrderInfo { get; set; }
        public string VnpOrderType { get; set; }
    }
}