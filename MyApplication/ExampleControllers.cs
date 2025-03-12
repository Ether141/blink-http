using BlinkHttp.Authentication;
using BlinkHttp.Authentication.Session;
using BlinkHttp.Http;

namespace MyApplication;

[Route("user")]
internal class UserController : Controller
{
    [HttpPost]
    public IHttpResult Login([FromBody] string username, [FromBody] string password)
    {
        (CredentialsValidationResult result, SessionInfo? sessionInfo) = ((SessionManager)Context.Authorizer!)
            .Login(username, password, Context.Request.RemoteEndPoint.Address.ToString(), Context.Response);
        return JsonResult.FromObject(new { result = result.ToString() });
    }

    [HttpPost]
    [Authorize]
    public IHttpResult Logout()
    {
        ((SessionManager)Context.Authorizer!).Logout(Context.Request);
        return JsonResult.FromObject(new { result = "success" });
    }

    [HttpPost]
    [Authorize]
    public IHttpResult Get()
    {
        return new TextResult("secured resource");
    }

    [HttpPost("{id}/get/{guid}")]
    public IHttpResult Get([FromQuery] int id, [FromQuery] string guid, [FromBody] UserInfo userInfo)
    {
        return JsonResult.FromObject(userInfo);
    }

    [HttpGet]
    public IHttpResult GetB([FromBody, Optional] string _str, [FromBody] int _int, [FromBody] bool _bool, [FromBody] float _float, [FromBody] double _double)
    {
        return JsonResult.FromObject(new { _str, _int, _bool, _float, _double });
    }

    [HttpGet]
    public IHttpResult GetArray([FromBody] List<int> values)
    {
        return JsonResult.FromObject(values);
    }

    [HttpGet("file/get")]
    public IHttpResult GetFile([FromBody] string fileName)
    {
        byte[] file = File.ReadAllBytes("C:/Users/ether/Desktop/image.jpg");
        return FileResult.Attachment(file, MimeTypes.ImageJpeg, fileName);
    }
}

[Authorize]
[Route("secured")]
internal class SecuredResources : Controller
{
    [HttpGet]
    public IHttpResult Get()
    {
        return new TextResult("secured resource");
    }
}
