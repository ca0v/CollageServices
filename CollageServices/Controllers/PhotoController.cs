namespace ImageRipper.Controllers;

using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

[ApiController]
[Route("[controller]")]
public class PhotoController : ControllerBase
{

    private DB DB;
    private const string _storagePath = "./photos";

    private readonly ILogger<PhotoController> _logger;

    public PhotoController(ILogger<PhotoController> logger)
    {
        _logger = logger;
        Directory.CreateDirectory(_storagePath);
        Directory.CreateDirectory(Path.Combine(_storagePath, "logs"));
        DB = new DB();
        DB.CreateDatabase();
        _logger.LogTrace("PhotoController created");
    }

    [HttpGet("list")]
    public IEnumerable<PhotoInfo> GetAllPhotos()
    {
        _logger.LogTrace("GetAllPhotos");
        return DB.GetPhotoInfos();
    }

    // download the image url posted to this function
    // only accept json content
    [Consumes("application/json")]
    [HttpGet("save")]
    public async Task<IActionResult> SavePhoto(string id, string filename, string url, string created, int width, int height)
    {
        _logger.LogTrace("SavePhoto", id);

        // make sure the id matches a base64 regex
        if (!isValidId(id))
        {
            _logger.LogError("Invalid id: {id}", id);
            return BadRequest();
        }

        // make sure the filename matches a file regex
        if (!Regex.IsMatch(filename, @"^[a-zA-Z0-9_\-\.]+$"))
        {
            _logger.LogError("Invalid filename: {filename}", filename);
            return BadRequest();
        }

        // make sure the created date is in zulu time
        if (!Regex.IsMatch(created, @"^\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}Z$"))
        {
            _logger.LogError("Invalid created date: {created}", created);
            return BadRequest();
        }

        // make sure the image doesn't already exist
        try
        {
            var photo = DB.GetPhotoInfo(id);
            _logger.LogError("Photo already exists: {id}", id);
            return BadRequest();
        }
        catch (Exception e)
        {
            _logger.LogTrace(e, "SavePhoto");
        }

        // save the image
        try
        {
            var client = new HttpClient();
            var response = await client.GetAsync(url);
            var bytes = await response.Content.ReadAsByteArrayAsync();
            // the client should not be controlling the file name but wanting to keep it 1-1 with the google for now
            var fullPath = Path.Combine(_storagePath, filename);
            await System.IO.File.WriteAllBytesAsync(fullPath, bytes);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "SavePhoto");
            return BadRequest();
        }


        // save the image info
        try
        {
            DB.SavePhoto(new PhotoInfo { id = id, filename = filename, created = created, width = width, height = height });
        }
        catch (Exception e)
        {
            _logger.LogError(e, "SavePhoto");
            return BadRequest();
        }

        // return the image info
        return Ok();
    }


    // service method to receive a json object and store it in the database
    [Consumes("application/json")]
    [HttpPost("save")]
    public IActionResult SaveCollage([FromQuery] string collageId, CollageState data)
    {
        _logger.LogTrace("SaveCollage", collageId);
        // return data as part of the response
        DB.SaveCollage(data);
        return Ok(data);
    }

    // delete the image with the given id
    [HttpGet("delete", Name = "DeletePhotoById")]
    public IActionResult Delete(string id)
    {
        // make sure the id matches a base64 regex
        if (!isValidId(id))
        {
            _logger.LogError("Invalid id: {id}", id);
            return BadRequest();
        }

        // lookup the path to the image in the database
        var photoInfo = DB.GetPhotoInfo(id);

        if (photoInfo.filename == null)
        {
            return NotFound();
        }

        // delete the image
        var path = Path.Combine(_storagePath, photoInfo.filename);
        System.IO.File.Delete(path);

        // delete the image info
        DB.DeletePhoto(id);

        return Ok();
    }

    private static bool isValidId(string id)
    {
        return Regex.IsMatch(id, @"^[a-zA-Z0-9_\-]+$");
    }

    // get the image with the given id
    [HttpGet("get", Name = "GetPhotoById")]
    public IActionResult Get(string id)
    {
        // make sure the id matches a base64 regex
        if (!isValidId(id))
        {
            _logger.LogError("Invalid id: {id}", id);
            return BadRequest();
        }

        // lookup the path to the image in the database
        var photo = DB.GetPhotoInfo(id);

        if (photo.filename == null)
        {
            return NotFound();
        }

        // get the image
        var path = Path.Combine(_storagePath, photo.filename);
        if (!System.IO.File.Exists(path))
        {
            return NotFound();
        }

        var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
        return File(stream, "image/png");
    }


    [HttpPost("saveRecording")]
    public async Task<IActionResult> SaveRecording(IFormFile audioFile)
    {
        _logger.LogTrace("SaveRecording");

        if (audioFile == null)
        {
            return BadRequest();
        }

        var path = Path.Combine(_storagePath, "test.occ");
        using (var stream = new FileStream(path, FileMode.Create))
        {
            await audioFile.CopyToAsync(stream);
        }

        return Ok(audioFile.FileName ?? "no file name");
    }

}