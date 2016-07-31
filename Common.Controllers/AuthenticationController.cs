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
        /// <summary>
        /// Add Category
        /// </summary>
        /// <param name="request">Category values</param>
        /// <returns>added Category</returns>
        [Route("token"), HttpPost]
        [ResponseType(typeof(AuthenticationResponse))]
        public IHttpActionResult GetToken([FromBody] AuthenticationRequest request)
        {
            if(request == null || !request.IsValid())
                return BadRequest("Invalid request provided");

            var expires = DateTime.UtcNow.AddMinutes(30)
                .Subtract(new DateTime(1970, 1, 1))
                .TotalSeconds.ToString();

            return Created("Token Created", new AuthenticationResponse
            {
                Token = Convert.ToBase64String(Encoding.UTF8.GetBytes(expires)),
                ExpiretionDate = expires
            });
        }
    }
}
