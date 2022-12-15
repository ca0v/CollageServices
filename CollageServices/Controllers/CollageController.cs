namespace ImageRipper.Controllers;

using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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
        _logger.LogTrace("CollageController created");
    }

    // service method to receive a json object and store it in the database
    [Consumes("application/json")]
    [HttpPost("save")]
    public IActionResult SaveCollage([FromQuery] string id, CollageData data)
    {
        _logger.LogTrace("SaveCollage", id);

        // need to stop doing client-side id generation...but till then
        if (data.Id != id)
        {
            throw new Exception("Id mismatch");
        }
        DB.SaveCollage(data);
        return Ok(data);
    }

    // list all collage entries
    [HttpGet("list")]
    public IEnumerable<CollageData> GetAllCollages()
    {
        _logger.LogTrace("GetAllCollages");
        return DB.GetCollages();
    }

}