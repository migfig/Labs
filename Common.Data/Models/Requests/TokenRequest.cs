using System;
using System.Text;

namespace Common.Data.Models.Requests
{
    public class TokenRequest
    {
        public string Code { get; set; }
        public string TargetUrl { get; set; }

        public bool IsValid()
        {
            var codeDate = DateTime.UtcNow;
            
            return !string.IsNullOrEmpty(TargetUrl) 
                && !string.IsNullOrEmpty(Code)
                && DateTime.TryParse(Encoding.UTF8.GetString(Convert.FromBase64String(Code)), out codeDate)
                && DateTime.UtcNow.Subtract(codeDate).Seconds <= 60;
        }
    }
}
