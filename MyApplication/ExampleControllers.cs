using BlinkDatabase.Annotations;
using BlinkDatabase.General;
using BlinkHttp.Authentication;
using BlinkHttp.Authentication.Session;
using BlinkHttp.Configuration;
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

    [HttpGet("get/{id}")]
    public IHttpResult GetBook([FromQuery] int id, [FromBody] decimal x)
    {
        Console.WriteLine(x);

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
        repo.Insert(bookToInsert);

        return Created();
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
    private readonly IRepository<User> repository;

    public UserController(IAuthorizer authorizer, IUserInfoProvider userInfoProvider, IRepository<User> repository)
    {
        this.authorizer = authorizer;
        authenticationProvider = new AuthenticationProvider(userInfoProvider);
        this.repository = repository;
    }

    [HttpPost]
    public IHttpResult Create()
    {
        MyApplication.User user = new MyApplication.User() { Username = "bartek", Email = "bartek@gmail.com", PasswordHash = "hashed_password_11", Profile = new UserProfile() { Id = 11 }, Role = new Role() { Id = 1 }, BirthDate = DateTime.Now, CreatedAt = DateTime.Now };
        repository.Insert(user);
        Console.WriteLine(user.Id);
        return Created();
    }

    [HttpPost]
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

    [HttpPost]
    [Authorize]
    public IHttpResult Get()
    {
        return JsonResult.FromObject(Request!.Cookies["session_id"]?.Value!);
    }

    [HttpPost]
    [Authorize]
    public IHttpResult Logout()
    {
        ((SessionManager)authorizer).InvalidSession(Request!);
        return JsonResult.FromObject(new { result = "success" });
    }
}

[Route("restaurant")]
public class RestaurantController : Controller
{
    private readonly IFilesProvider filesProvider;
    private readonly IRepository<Restaurant> restaurantRepo;

    public RestaurantController(IFilesProvider filesProvider, IRepository<Restaurant> restaurantRepo)
    {
        this.filesProvider = filesProvider;
        this.restaurantRepo = restaurantRepo;
    }

    [HttpGet("get/{id}")]
    public IHttpResult Get([FromQuery] int id)
    {
        Restaurant? restaurant = restaurantRepo.SelectSingle(x => x.Id == id);
        return restaurant != null ? JsonResult.FromObject(new RestaurantMapper().Map(restaurant)) : NotFound();
    }

    [HttpGet("all?start={start}&end={end}")]
    public IHttpResult All([FromQuery, Optional] int? start, [FromQuery, Optional] int? end)
    {
        IEnumerable<Restaurant> restaurants = restaurantRepo.Select();

        if (start != null || end != null)
        {
            start ??= 0;
            end ??= restaurants.Count();

            if (start < 0 || end > restaurants.Count() || start >= end)
            {
                return BadRequest();
            }

            restaurants = [.. restaurants.Skip((int)start).Take((int)end - (int)start)];
        }

        return JsonResult.FromObject(new RestaurantMapper().MapCollection(restaurants));
    }

    [HttpGet("banner/{id}")]
    public IHttpResult GetBanner([FromQuery] int id)
    {
        Restaurant? restaurant = restaurantRepo.SelectSingle(x => x.Id == id);

        if (restaurant == null)
        {
            return NotFound();
        }

        byte[]? file = filesProvider.LoadFile(restaurant.BannerPath);

        if (file == null)
        {
            return NotFound();
        }

        string mimeType = MimeTypes.GetMimeTypeForExtension(Path.GetExtension(restaurant.BannerPath)) ?? MimeTypes.ImageJpeg;
        return FileResult.Inline(file, mimeType);
    }
}

[Table("product")]
public class Product
{
    [Id]
    [Column("id")]
    public int Id { get; set; }

    [Column("name")]
    public string Name { get; set; }

    [Column("price")]
    public decimal Price { get; set; }

    [Column("description")]
    public string Description { get; set; }

    [Relation("id")]
    [Column("restaurant_id")]
    public Restaurant Restaurant { get; set; }
}

[Table("restaurant")]
public class Restaurant
{
    [Id]
    [Column("id")]
    public int Id { get; set; }

    [Column("name")]
    public string Name { get; set; }

    [Column("opinion")]
    public decimal Opinion { get; set; }

    [Column("opinion_number")]
    public int OpinionNumber { get; set; }

    [Column("tags")]
    public string Tags { get; set; }

    [Column("banner_path")]
    public string BannerPath { get; set; }

    [Relation("restaurant_id")]
    [Column("id")]
    public List<Product>? Products { get; set; }
}

public class FilesProvider : IFilesProvider
{
    private readonly IConfiguration configuration;

    public string MainPath => configuration["server:content_path"]!;

    public FilesProvider(IConfiguration configuration)
    {
        this.configuration = configuration;
    }

    public byte[]? LoadFile(string path)
    {
        path = Path.Combine(MainPath, path);

        if (!File.Exists(path))
        {
            return null;
        }

        return File.ReadAllBytes(path);
    }
}

public interface IFilesProvider
{
    byte[]? LoadFile(string path);
}

internal class RestaurantDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Opinion { get; set; }
    public int OpinionNumber { get; set; }
    public string Tags { get; set; }
    public string BannerPath { get; set; }
}


internal class RestaurantMapper
{
    public RestaurantDTO Map(Restaurant from) => new RestaurantDTO()
    {
        Id = from.Id,
        Name = from.Name,
        Opinion = from.Opinion,
        OpinionNumber = from.OpinionNumber,
        Tags = from.Tags,
        BannerPath = from.BannerPath
    };

    public Restaurant ReverseMap(RestaurantDTO from) => throw new NotImplementedException();

    public IEnumerable<RestaurantDTO> MapCollection(IEnumerable<Restaurant> from)
    {
        List<RestaurantDTO> result = [];

        foreach (Restaurant item in from)
        {
            result.Add(Map(item));
        }

        return result;
    }

    public IEnumerable<Restaurant> ReverseMapCollection(IEnumerable<RestaurantDTO> from) => throw new NotImplementedException();
}