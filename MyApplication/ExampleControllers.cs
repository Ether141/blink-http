using BlinkDatabase.General;
using BlinkHttp.Authentication;
using BlinkHttp.Authentication.Session;
using BlinkHttp.Http;
using System.Data;

namespace MyApplication;

public class BookDO
{
    public string Name { get; set; }
    public int AuthorId { get; set; }
    public int LibraryId { get; set; }
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
    public IHttpResult GetAllBooks([FromQuery, Optional] int start, [FromQuery, Optional] int end)
    {
        IEnumerable<Book> allBooks = repo.Select();
        IEnumerable<Book> result;

        if (start != 0 || end != 0)
        {
            if (start > 0 && end == 0)
            {
                end = allBooks.Count();
            }

            if (start < 0 || end > allBooks.Count() || start >= end)
            {
                return NotFound();
            }
            result = [.. allBooks.Skip(start).Take(end - start)];
        }
        else
        {
            result = allBooks;
        }

        return JsonResult.FromObject(result.Select(b => new { b.Id, b.Name, b.Author, LibraryId = b.Library.Id }));
    }

    [HttpGet("get/{id}")]
    public IHttpResult GetBook([FromQuery] int id)
    {
        Book? book = repo.SelectSingle(b => b.Id == id);

        if (book == null)
        {
            return NotFound();
        }

        return JsonResult.FromObject(new { book.Id, book.Name, book.Author, LibraryId = book.Library.Id });
    }

    [HttpPost("add")]
    public IHttpResult AddBook([FromBody] BookDO book)
    {
        Book bookToInsert = new Book() { Name = book.Name, Library = new Library() { Id = book.LibraryId }, Author = new Author() { Id = book.AuthorId } };
        bool success = repo.Insert(bookToInsert) == 1;

        return success ? Created() : InternalServerError();
    }

    [HttpDelete("delete")]
    public IHttpResult DeleteBook([FromBody] int id)
    {
        bool success = repo.Delete(b => b.Id == id) == 1;

        return success ? Ok() : InternalServerError();
    }
}

[Route("user")]
internal class UserController : Controller
{
    private readonly IAuthenticationProvider authenticationProvider;
    private readonly IAuthorizer authorizer;

    public UserController(IAuthorizer authorizer, IUserInfoProvider userInfoProvider)
    {
        this.authorizer = authorizer;
        authenticationProvider = new AuthenticationProvider(userInfoProvider);
    }

    [HttpPost]
    public IHttpResult Login([FromBody] string username, [FromBody] string password)
    {
        CredentialsValidationResult validationResult = authenticationProvider.ValidateCredentials(username, password, out IUser? user);

        if (validationResult != CredentialsValidationResult.Success)
        {
            return JsonResult.FromObject(new { result = validationResult.ToString() }, System.Net.HttpStatusCode.Unauthorized);
        }

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
}