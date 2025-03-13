using BlinkDatabase;
using BlinkDatabase.PostgreSql;
using BlinkHttp.Authentication;
using BlinkHttp.Authentication.Session;
using BlinkHttp.Http;

namespace MyApplication;

[Route("book")]
internal class BooksController : Controller
{
    private PostgreSqlRepository<Book> repo;

    public override void Initialize()
    {
        repo = new PostgreSqlRepository<Book>((PostgreSqlConnection)Context.DatabaseConnection!);
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
    [HttpPost]
    public IHttpResult Login([FromBody] string username, [FromBody] string password)
    {
        (CredentialsValidationResult result, _, IUser? user) = ((SessionManager)Context.Authorizer!)
            .Login(username, password, Context.Request.RemoteEndPoint.Address.ToString(), Context.Response);

        return result == CredentialsValidationResult.Success ? 
            JsonResult.FromObject(new { user!.Id, user!.Username, roles = string.Join(", ", user!.Roles) }) : 
            JsonResult.FromObject(new { success = false }, System.Net.HttpStatusCode.Unauthorized);
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
        return JsonResult.FromObject(Context.User!);
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
