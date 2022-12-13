namespace ImageRipper.Controllers;

using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

[ApiController]
[Route("[controller]")]
public class AudioController : ControllerBase
{
    private readonly ILogger<PhotoController> _logger;
    private DB DB;
    private const string _storagePath = "./photos";


    public AudioController(ILogger<PhotoController> logger)
    {
        _logger = logger;
        DB = new DB();
        DB.CreateDatabase();
        _logger.LogTrace("AudioController created");
    }

    [HttpPost("save")]
    public async Task<IActionResult> SaveRecording([FromQuery] string id, IFormFile audioFile)
    {
        _logger.LogTrace("SaveRecording");

        if (audioFile == null)
        {
            return BadRequest();
        }

        // id must contain only digits [0-9]
        if (!Regex.IsMatch(id, @"^\d+$"))
        {
            _logger.LogError("Invalid id: {id}", id);
            return BadRequest();
        }

        var path = Path.Combine(_storagePath, $"{id}.mp3");
        using (var stream = new FileStream(path, FileMode.Create))
        {
            await audioFile.CopyToAsync(stream);
        }

        return Ok(audioFile.FileName ?? "no file name");
    }

    [HttpGet("get")]
    public IActionResult GetRecording([FromQuery] string id)
    {
        _logger.LogTrace("GetRecording");

        // id must contain only digits [0-9]
        if (!Regex.IsMatch(id, @"^\d+$"))
        {
            _logger.LogError("Invalid id: {id}", id);
            return BadRequest();
        }

        var path = Path.Combine(_storagePath, $"{id}.mp3");
        if (!System.IO.File.Exists(path))
        {
            return NotFound();
        }

        var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
        return File(stream, "audio/mpeg");
    }

    // get all recordings
    [HttpGet("list")]
    public IActionResult GetRecordings()
    {
        _logger.LogTrace("GetRecordings");
        var recordingInfos = new List<RecordingInfo>();
        var files = Directory.GetFiles(_storagePath, "*.mp3");
        foreach (var file in files)
        {
            var id = Path.GetFileNameWithoutExtension(file);
            recordingInfos.Add(new RecordingInfo { id = id, title = id });
        }
        return Ok(recordingInfos);
    }

    // delete recording
    [HttpGet("delete")]
    public IActionResult DeleteRecording([FromQuery] string id)
    {
        _logger.LogTrace("DeleteRecording");

        // id must contain only digits [0-9]
        if (!Regex.IsMatch(id, @"^\d+$"))
        {
            _logger.LogError("Invalid id: {id}", id);
            return BadRequest();
        }

        var path = Path.Combine(_storagePath, $"{id}.mp3");
        if (!System.IO.File.Exists(path))
        {
            return NotFound();
        }

        System.IO.File.Delete(path);
        return Ok();
    }
}

