
namespace Common.Data.Models.Requests
{
    public class AuthenticationRequest
    {
        public string User { get; set; }
        public string Password { get; set; }
        public string ApiKey { get; set; }
        public string TargetUrl { get; set; }

        public bool IsValid()
        {
            return !string.IsNullOrEmpty(TargetUrl) 
                && (isUserValid() || !string.IsNullOrEmpty(ApiKey));
        }

        private bool isUserValid()
        {
            return !string.IsNullOrEmpty(User) && !string.IsNullOrEmpty(Password);
        }
    }
}
