namespace EFCore.Sample.Business.Models.Safe2Pay
{
    public class ResponseDetail
    {
        public int IdTransaction { get; set; }
        public string Status { get; set; }
        public string Message { get; set; }
        public string Application { get; set; }
        public string Vendor { get; set; }
        public string Reference { get; set; }
        public string PaymentDate { get; set; }
        public string CallbackUrl { get; set; }
        public string CreatedDate { get; set; }
        public float Amount { get; set; }
        public float NetValue { get; set; }
        public float TaxValue { get; set; }
        public float NegotiationTax { get; set; }
        public string PaymentMethod { get; set; }
        public Customer Customer { get; set; }
        public Paymentobject PaymentObject { get; set; }
        public float AmountPayment { get; set; }
    }
}
