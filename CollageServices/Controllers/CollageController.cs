namespace ImageRipper.Controllers;

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class CollageController : ControllerBase
{

    private DB DB;

    private readonly ILogger<CollageController> _logger;

    public CollageController(ILogger<CollageController> logger)
    {
        _logger = logger;
        DB = new DB();
        DB.CreateDatabase();
        _logger.LogTrace("CollageController created");
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