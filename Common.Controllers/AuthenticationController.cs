using Common.Data.Models.Requests;
using Common.Data.Models.Responses;
using System;
using System.Text;
using System.Web.Http;
using System.Web.Http.Description;

namespace Common.Controllers
{
    [RoutePrefix("api/authentication")]
    public class AuthenticationController: ApiController
    {
        [Route("authenticate"), HttpPost]
        [ResponseType(typeof(AuthenticationResponse))]
        public IHttpActionResult Authenticate([FromBody] AuthenticationRequest request)
        {
            if(request == null || !request.IsValid())
                return BadRequest("Invalid request provided");

            var expires = DateTime.UtcNow.AddMinutes(1).ToString("yyyy-MM-dd hh:mm:ss");

            return Created("Authorization Code Created", new AuthenticationResponse
            {
                Code = Convert.ToBase64String(Encoding.UTF8.GetBytes(expires)),
                TargetUrl = request.TargetUrl
            });
        }

        [Route("token/{code:string}"), HttpPost]
        [ResponseType(typeof(TokenResponse))]
        public IHttpActionResult GetToken([FromUri] string code, [FromBody] TokenRequest request)
        {
            if (request == null || !request.IsValid())
                return BadRequest("Invalid request provided");

            var expires = DateTime.UtcNow.AddMinutes(30).ToString("yyyy-MM-dd hh:mm:ss");

            return Created("Token Granted", new TokenResponse
            {
                Token = Convert.ToBase64String(Encoding.UTF8.GetBytes(expires)),
                ExpirationDate = expires,
                TargetUrl = request.TargetUrl
            });
        }
    }
}
