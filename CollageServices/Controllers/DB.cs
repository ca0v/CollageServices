namespace ImageRipper;

public class DB
{
    public void SaveCollage(Collage collage)
    {
        var context = new PhotoContext();
        // if the collage already exists, update it otherwise add it
        var existing = context.Collages.Find(collage.Id);
        if (existing != null)
        {
            existing.Note = collage.Note;
            existing.Title = collage.Title;
            existing.Data = collage.Data;
            context.Collages.Update(existing);
        }
        else
        {
            context.Collages.Add(collage);
        }
        context.SaveChanges();
    }

    public void SavePhoto(Photo photo)
    {
        var context = new PhotoContext();
        context.Photos.Add(photo);
        context.SaveChanges();
    }

    public IEnumerable<Photo> GetPhotoInfos()
    {
        var context = new PhotoContext();
        return context.Photos.ToList();
    }

    public Photo GetPhotoInfo(string id)
    {
        var context = new PhotoContext();
        var photo = context.Photos.Find(id);
        if (photo == null)
        {
            throw new Exception($"Photo with id {id} not found");
        }
        return photo;
    }

    internal void DeletePhoto(string id)
    {
        var context = new PhotoContext();
        var photo = context.Photos.Find(id);
        if (photo == null)
        {
            throw new Exception($"Photo with id {id} not found");
        }
        context.Photos.Remove(photo);
        context.SaveChanges();
    }

    internal IEnumerable<CollageData> GetCollages()
    {
        var context = new PhotoContext();
        return context.Collages.ToList()
            .Select(c =>
            {
                var result = new CollageData
                {
                    Id = c.Id,
                    Title = c.Title,
                    Note = c.Note,
                    Data = Newtonsoft.Json.JsonConvert.DeserializeObject<CollageCellState[]>(c.Data ?? "[]"),
                };
                return result;
            });
    }

    internal void SaveRecording(string id, string title)
    {
        var context = new PhotoContext();
        var recording = new Recording { Id = id, Title = title };
        context.Recordings.Add(recording);
        context.SaveChanges();
    }

    internal IList<Recording> GetRecordings()
    {
        var context = new PhotoContext();
        return context.Recordings.ToList();
    }

    internal void DeleteRecording(string id)
    {
        var context = new PhotoContext();
        var recording = context.Recordings.Find(id);
        if (recording == null)
        {
            throw new Exception($"Recording with id {id} not found");
        }
        context.Recordings.Remove(recording);
        context.SaveChanges();
    }

    internal void UpdateRecording(string id, Recording recording)
    {
        var context = new PhotoContext();
        var existing = context.Recordings.Find(id);
        if (existing == null)
        {
            throw new Exception($"Recording with id {id} not found");
        }
        existing.Title = recording.Title;
        context.SaveChanges();
    }
}
