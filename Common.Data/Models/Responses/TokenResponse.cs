
namespace Common.Data.Models.Responses
{
    public class TokenResponse
    {
        public string Token { get; set; }
        public string ExpirationDate { get; set; }
        public string TargetUrl { get; set; }
    }
}
