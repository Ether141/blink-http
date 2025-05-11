using BlinkHttp.Http;
using System.Text;

namespace BlinkHttp.Swagger;

[Route("docs")]
internal class SwaggerController : Controller
{
    private readonly SwaggerUI swaggerUI;

    public SwaggerController(SwaggerUI swaggerUI)
    {
        this.swaggerUI = swaggerUI;
    }

    [HttpGet("swagger.json")]
    public IHttpResult GetSwagger()
    {
        string json = swaggerUI.GetJson() ?? "";
        return new JsonResult(json);
    }

    [HttpGet("swagger")]
    public IHttpResult SwaggerPage()
    {
        byte[] data = Encoding.UTF8.GetBytes(swaggerUI.GetHtml());
        return new HttpResult(data, MimeTypes.TextHtml, null, System.Net.HttpStatusCode.OK);
    }
}
