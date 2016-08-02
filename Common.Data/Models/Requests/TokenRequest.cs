
namespace Common.Data.Models.Requests
{
    public class TokenRequest
    {
        public string Code { get; set; }
        public string TargetUrl { get; set; }

        public bool IsValid()
        {
            return !string.IsNullOrEmpty(TargetUrl) 
                && !string.IsNullOrEmpty(Code);
        }
    }
}
