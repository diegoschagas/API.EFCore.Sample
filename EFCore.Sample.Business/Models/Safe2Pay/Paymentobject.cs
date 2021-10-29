﻿namespace EFCore.Sample.Business.Models.Safe2Pay
{
    public class Paymentobject
    {
        public string Token { get; set; }
        public string CardNumber { get; set; }
        public string Brand { get; set; }
        public int? Installments { get; set; }
        public object ReturnCode { get; set; }
        public object Message { get; set; }
        public string BankSlipNumber { get; set; }
        public string DueDate { get; set; }
        public string DigitableLine { get; set; }
        public string Barcode { get; set; }
        public string BankSlipUrl { get; set; }
    }
}
