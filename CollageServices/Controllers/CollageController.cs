namespace ImageRipper.Controllers;

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class CollageController : ControllerBase
{

    private DB DB;
    private const string _storagePath = "./photos";

    private readonly ILogger<PhotoController> _logger;

    public CollageController(ILogger<PhotoController> logger)
    {
        _logger = logger;
        Directory.CreateDirectory(_storagePath);
        Directory.CreateDirectory(Path.Combine(_storagePath, "logs"));
        DB = new DB();
        DB.CreateDatabase();
        _logger.LogTrace("PhotoController created");
    }

    // service method to receive a json object and store it in the database
    [Consumes("application/json")]
    [HttpPost("save")]
    public IActionResult SaveCollage([FromQuery] string id, CollageState data)
    {
        _logger.LogTrace("SaveCollage", id);
        // return data as part of the response
        DB.SaveCollage(data);
        return Ok(data);
    }

    // list all collage entries
    [HttpGet("list")]
    public IEnumerable<CollageState> GetAllCollages()
    {
        _logger.LogTrace("GetAllCollages");
        return DB.GetCollages();
    }

}