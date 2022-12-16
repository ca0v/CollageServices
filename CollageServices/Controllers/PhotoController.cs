namespace ImageRipper.Controllers;

using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

[ApiController]
[Route("[controller]")]
public partial class PhotoController : ControllerBase
{

    private DB DB;
    private const string _storagePath = "./photos";

    private readonly ILogger<PhotoController> _logger;

    public PhotoController(ILogger<PhotoController> logger)
    {
        _logger = logger;
        Directory.CreateDirectory(_storagePath);
        DB = new DB();
        _logger.LogTrace("PhotoController created");
    }

    [HttpGet("list")]
    public IEnumerable<Photo> GetAllPhotos()
    {
        _logger.LogTrace("GetAllPhotos");
        var photoInfos = DB.GetPhotoInfos();
        foreach (var photoInfo in photoInfos)
        {
            if (photoInfo.Filename == null)
            {
                photoInfo.Cached = false;
            }
            else
            {
                var path = Path.Combine(_storagePath, photoInfo.Filename);
                photoInfo.Cached = System.IO.File.Exists(path);
            }
        }
        return photoInfos;
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

        // if the image does not exist, save it now, it will fail if file already exists
        try
        {
            await SaveImage(filename, url);
            _logger.LogTrace("Saved image: {filename}", filename);
        }
        catch (Exception e)
        {
            _logger.LogError("Failed to save image: {filename} {error}", filename, e.Message);
            return BadRequest();
        }

        // save the image info, it will fail if already exists
        try
        {
            DB.SavePhoto(new Photo { Id = id, Filename = filename, Created = created, Width = width, Height = height });
        }
        catch (Exception)
        {
            _logger.LogError("Failed to save photo info: {id}", id);
            // still ok because the image was missing and now it has been saved
            return Ok();

        }

        // return the image info
        return Ok();
    }

    private static async Task SaveImage(string filename, string url)
    {
        var fullPath = Path.Combine(_storagePath, filename);
        if (System.IO.File.Exists(fullPath))
        {
            throw new Exception("File already exists: " + fullPath);
        }
        var client = new HttpClient();
        var response = await client.GetAsync(url);
        var bytes = await response.Content.ReadAsByteArrayAsync();
        // the client should not be controlling the file name but wanting to keep it 1-1 with the google for now
        await System.IO.File.WriteAllBytesAsync(fullPath, bytes);
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

        if (photoInfo.Filename == null)
        {
            return NotFound();
        }

        // delete the image
        var path = Path.Combine(_storagePath, photoInfo.Filename);
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

        if (photo.Filename == null)
        {
            return NotFound();
        }

        // get the image
        var path = Path.Combine(_storagePath, photo.Filename);
        if (!System.IO.File.Exists(path))
        {
            return NotFound();
        }

        var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
        return File(stream, "image/png");
    }
}