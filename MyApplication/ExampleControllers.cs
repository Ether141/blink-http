using BlinkDatabase.General;
using BlinkHttp.Authentication;
using BlinkHttp.Authentication.Session;
using BlinkHttp.Background;
using BlinkHttp.Http;
using System.Data;
using System.Net.Http.Headers;

namespace MyApplication;

public class BookDO
{
    public string Name { get; set; }
    public int AuthorId { get; set; }
    public int LibraryId { get; set; }
}

[Route("app")]
internal class ApiController : Controller
{
    private readonly IBackgroundServices backgroundServices;

    public ApiController(IBackgroundServices backgroundServices)
    {
        this.backgroundServices = backgroundServices;
    }

    [HttpGet("search?query={query}&limit={limit}")]
    public IHttpResult Search([FromQuery] string query, [FromQuery, Optional] int? limit)
    {
        return JsonResult.FromObject(new { query, limit });
    }

    [NoCors]
    [HttpGet]
    public async Task<IHttpResult> Get()
    {
        await Task.Delay(1000);
        return Ok();
    }

    [NoCors]
    [HttpPost("emailserv")]
    public async Task<IHttpResult> StartStopEmailService()
    {
        string message;

        if (backgroundServices.IsRunning<EmailBackgroundService>())
        {
            message = "Email service was stopped.";
            await backgroundServices.StopServiceAsync<EmailBackgroundService>();
        }
        else
        {
            message = "Email service was started";
            backgroundServices.StartService<EmailBackgroundService>();
        }

        return JsonResult.FromObject(new { message });
    }

    [HttpPost]
    public IHttpResult Post()
    {
        return Ok();
    }
}

[Route("book")]
internal class BooksController : Controller
{
    private readonly IRepository<Book> repo;

    public BooksController(IRepository<Book> repo, IDatabaseConnection db)
    {
        this.repo = repo;
    }

    [HttpGet("all?start={start}&end={end}")]
    public IHttpResult GetAllBooks([FromQuery, Optional] int? start, [FromQuery, Optional] int? end)
    {
        IEnumerable<Book> allBooks = repo.Select();
        IEnumerable<Book> result;

        if (start != null || end != null)
        {
            start ??= 0;
            end ??= allBooks.Count();

            if (start < 0 || end > allBooks.Count() || start >= end)
            {
                return BadRequest();
            }

            result = [.. allBooks.Skip((int)start).Take((int)end - (int)start)];
        }
        else
        {
            result = allBooks;
        }

        return JsonResult.FromObject(result.Select(b => new { b.Id, b.Name, b.Author, LibraryId = b.Library.Id }));
    }

    [HttpGet("{id}")]
    public IHttpResult GetBook([FromQuery] int id)
    {
        Book? book = repo.SelectSingle(b => b.Id == id);

        if (book == null)
        {
            return NotFound();
        }

        return JsonResult.FromObject(new { book.Id, book.Name, book.Author, LibraryId = book.Library.Id });
    }

    [HttpPost]
    public IHttpResult AddBook([FromBody] BookDO book)
    {
        Book bookToInsert = new Book() { Name = book.Name, Library = new Library() { Id = book.LibraryId }, Author = new Author() { Id = book.AuthorId } };
        repo.Insert(bookToInsert);

        return Created();
    }

    [HttpPut]
    public IHttpResult UpdateBook([FromBody] int id, [FromBody] BookDO book)
    {
        Book? bookFromDb = repo.SelectSingle(b => b.Id == id);

        if (bookFromDb == null)
        {
            return NotFound();
        }

        bookFromDb.Name = book.Name;
        bookFromDb.Author!.Id = book.AuthorId;
        bookFromDb.Library!.Id = book.LibraryId;
        repo.Update(bookFromDb);
        return Ok();
    }

    [HttpDelete("{id}")]
    public IHttpResult DeleteBook([FromQuery] int id)
    {
        bool success = repo.Delete(b => b.Id == id) == 1;
        return success ? Ok() : InternalServerError();
    }
}

[Route("user")]
[Authorize]
internal class UserController : Controller
{
    private readonly IAuthenticationProvider authenticationProvider;
    private readonly IAuthorizer authorizer;
    private readonly IRepository<User> repository;

    public UserController(IAuthorizer authorizer, IUserInfoProvider userInfoProvider, IRepository<User> repository)
    {
        this.authorizer = authorizer;
        authenticationProvider = new AuthenticationProvider(userInfoProvider);
        this.repository = repository;
    }

    [HttpPost("create")]
    public IHttpResult Create()
    {
        MyApplication.User user = new MyApplication.User() { Username = "bartek", Email = "bartek@gmail.com", PasswordHash = "hashed_password_11", Profile = new UserProfile() { Id = 11 }, Role = new Role() { Id = 1 }, BirthDate = DateTime.Now, CreatedAt = DateTime.Now };
        repository.Insert(user);
        Console.WriteLine(user.Id);
        return Created();
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public IHttpResult Login([FromBody] string username, [FromBody] string password)
    {
        CredentialsValidationResult validationResult = CredentialsValidationResult.Success;

        if (validationResult != CredentialsValidationResult.Success)
        {
            return JsonResult.FromObject(new { result = validationResult.ToString() }, System.Net.HttpStatusCode.Unauthorized);
        }

        IUser user = new User() { Id = new Guid("7fff15d8-ef6e-4868-baa4-fab6cad68697"), Username = "ewa_art", Role = new Role() { Id = 1, RoleName = "user" } };
        ((SessionManager)authorizer).CreateSession(user!.Id, Response);

        return JsonResult.FromObject(new { user!.Id, user!.Username, roles = string.Join(", ", user!.Roles) });
    }

    [HttpPost("get")]
    public IHttpResult Get()
    {
        return JsonResult.FromObject(Request!.Cookies["session_id"]?.Value!);
    }

    [HttpPost("logout")]
    public IHttpResult Logout()
    {
        ((SessionManager)authorizer).InvalidSession(Request!);
        return JsonResult.FromObject(new { result = "success" });
    }
}