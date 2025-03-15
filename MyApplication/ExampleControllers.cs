using BlinkDatabase;
using BlinkDatabase.General;
using BlinkDatabase.PostgreSql;
using BlinkHttp.Authentication;
using BlinkHttp.Authentication.Additional;
using BlinkHttp.Authentication.Session;
using BlinkHttp.Configuration;
using BlinkHttp.Http;
using System.Data;
using System.Data.Common;

namespace MyApplication;

[Route("book")]
internal class BooksController : Controller
{
    private readonly IRepository<Book> repo;

    public BooksController(IRepository<Book> repo, IDatabaseConnection db)
    {
        this.repo = repo;
    }

    [HttpGet("all")]
    public IHttpResult GetAllBooks()
    {
        IEnumerable<Book> allBooks = repo.Select();
        return JsonResult.FromObject(allBooks.Select(b => new { b.Id, b.Name, b.Author, LibraryId = b.Library.Id }));
    }

    [HttpGet("{id}")]
    public IHttpResult GetBook([FromQuery] int id)
    {
        Book? book = repo.SelectSingle(b => b.Id == id);

        if (book == null)
        {
            return JsonResult.FromObject(new { result = "not_found" }, System.Net.HttpStatusCode.NotFound);
        }

        return JsonResult.FromObject(new { book.Id, book.Name, book.Author, LibraryId = book.Library.Id });
    }
}

[Route("user")]
internal class UserController : Controller
{
    private readonly IAuthenticationProvider authenticationProvider;
    private readonly IAuthorizer authorizer;
    private readonly LoginAttemptsGuard guard;

    public UserController(IAuthorizer authorizer, IUserInfoProvider userInfoProvider, LoginAttemptsGuard guard)
    {
        this.authorizer = authorizer;
        authenticationProvider = new AuthenticationProvider(userInfoProvider);
        this.guard = guard;
    }

    [HttpPost]
    public IHttpResult Login([FromBody] string username, [FromBody] string password)
    {
        string ip = Request!.RemoteEndPoint.Address.ToString();

        if (guard.ReachedAttemptsLimit(ip))
        {
            return JsonResult.FromObject(new { result = "ReachedMaxFailedLoginAttempts" }, System.Net.HttpStatusCode.Unauthorized);
        }

        CredentialsValidationResult validationResult = authenticationProvider.ValidateCredentials(username, password, out IUser? user);

        if (validationResult != CredentialsValidationResult.Success)
        {
            guard.RegisterFailedAttempt(ip);
            return JsonResult.FromObject(new { result = validationResult.ToString() }, System.Net.HttpStatusCode.Unauthorized);
        }

        guard.ResetFailedAttempts(ip);
        ((SessionManager)authorizer).CreateSession(user!.Id, Response);

        return JsonResult.FromObject(new { user!.Id, user!.Username, roles = string.Join(", ", user!.Roles) });
    }

    [HttpPost]
    [Authorize]
    public IHttpResult Logout()
    {
        ((SessionManager)authorizer).InvalidSession(Request!);
        return JsonResult.FromObject(new { result = "success" });
    }

    [HttpPost]
    [Authorize]
    public IHttpResult Get()
    {
        return JsonResult.FromObject(User!);
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
