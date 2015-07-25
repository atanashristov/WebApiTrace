using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebApiTraceExample.Controllers
{
    public class ExceptionController : ApiController
    {
        public void Get()
        {
            var msg = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.InternalServerError,
                ReasonPhrase = "There was some error!",
            };

            throw new HttpResponseException(msg);
        }
    }
}
