using System.ComponentModel.DataAnnotations.Schema;

namespace BackendLi.Entities
{
    
    [Table("token")]
    public class Token
    {
        public int Id { get; set; }
        public string? TokenValue { get; set; }
        public bool IsValid { get; set; }
        public DateTime ExpirationDate { get; set; }

        public Token(string tokenValue)
        {
            TokenValue = tokenValue;
        }

        public Token(string tokenValue, bool isValid, DateTime expirationDate)
        {
            TokenValue = tokenValue;
            IsValid = isValid;
            ExpirationDate = expirationDate;
        }
    }
}