namespace ECommerce.Application.Exceptions
{
    public class BusinessRuleValidationException : Exception
    {
        public BusinessRuleValidationException(string message) : base(message)
        {
            
        }
        public BusinessRuleValidationException(string code, string message) : base ($"{code}: {message}")
        {
            Code = code;
        }
        public string? Code { get; set; }
    }
}
