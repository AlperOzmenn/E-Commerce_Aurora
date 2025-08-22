namespace ECommerce.MVC.ViewModels.PaymentVMs
{
    public class PaymentVM
    {
        public string CardName { get; set; }
        public string CardNumber { get; set; }
        public string ExpirationMonth { get; set; }
        public string ExpirationYear { get; set; }
        public string CVV { get; set; }
        public decimal Amount { get; set; }
    }
}
