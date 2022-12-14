namespace ImageRipper;
using Microsoft.Data.Sqlite;
using Newtonsoft.Json;

public class DB
{
    private readonly string dbName = "photo.sqlite";
    private string connectionString
    {
        get { return $"Data Source={dbName}"; }
    }

    public DB()
    {
        // create the database if it doesn't exist
        if (!File.Exists(dbName))
        {
            CreateDatabase();
        }
        else
        {
            var fullPathToDb = Path.GetFullPath(dbName);
            //throw new Exception($"Database already exists at {fullPathToDb}");
        }
    }

    public void CreateDatabase()
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            // create the table
            var createTable = connection.CreateCommand();
            createTable.CommandText = "CREATE TABLE IF NOT EXISTS photos (id TEXT PRIMARY KEY, filename TEXT, created TEXT, width INTEGER, height INTEGER)";
            createTable.ExecuteNonQuery();

            // create a generic blob storage for storing any json data, specifically a collage
            createTable = connection.CreateCommand();
            createTable.CommandText = "CREATE TABLE IF NOT EXISTS collages (id TEXT PRIMARY KEY, data TEXT)";
            createTable.ExecuteNonQuery();

            // create recordings table
            createTable = connection.CreateCommand();
            createTable.CommandText = "CREATE TABLE IF NOT EXISTS recordings (id TEXT PRIMARY KEY, title TEXT)";
            createTable.ExecuteNonQuery();
        }
    }

    public void SaveCollage(CollageState collage)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            // if the collage already exists, update it
            var update = connection.CreateCommand();
            update.CommandText = "UPDATE collages SET data = @data WHERE id = @id";
            update.Parameters.AddWithValue("@id", collage.id);
            update.Parameters.AddWithValue("@data", JsonConvert.SerializeObject(collage));
            var rows = update.ExecuteNonQuery();
            if (rows == 0)
            {
                // insert a row
                var insert = connection.CreateCommand();
                insert.CommandText = "INSERT INTO collages (id, data) VALUES (@id, @data)";
                insert.Parameters.AddWithValue("@id", collage.id);
                insert.Parameters.AddWithValue("@data", JsonConvert.SerializeObject(collage));
                insert.ExecuteNonQuery();
            }

        }
    }

    public void SavePhoto(PhotoInfo photo)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            // insert a row
            var insert = connection.CreateCommand();
            insert.CommandText = "INSERT INTO photos (id, filename, created, width, height) VALUES (@id, @filename, @created, @width, @height)";
            insert.Parameters.AddWithValue("@id", photo.id);
            insert.Parameters.AddWithValue("@filename", photo.filename);
            insert.Parameters.AddWithValue("@created", photo.created);
            insert.Parameters.AddWithValue("@width", photo.width);
            insert.Parameters.AddWithValue("@height", photo.height);
            insert.ExecuteNonQuery();
        }
    }

    public IEnumerable<PhotoInfo> GetPhotoInfos()
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var select = connection.CreateCommand();
            select.CommandText = "SELECT * FROM photos ORDER BY created ASC";
            var reader = select.ExecuteReader();
            var photos = new List<PhotoInfo>();
            while (reader.Read())
            {
                photos.Add(new PhotoInfo
                {
                    id = reader.GetString(0),
                    filename = reader.GetString(1),
                    created = reader.GetString(2),
                    width = reader.GetInt32(3),
                    height = reader.GetInt32(4)
                });
            }
            return photos;
        }
    }

    public PhotoInfo GetPhotoInfo(string id)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var select = connection.CreateCommand();
            select.CommandText = "SELECT * FROM photos WHERE id = @id";
            select.Parameters.AddWithValue("@id", id);
            var reader = select.ExecuteReader();
            if (reader.Read())
            {
                return new PhotoInfo
                {
                    id = reader.GetString(0),
                    filename = reader.GetString(1),
                    created = reader.GetString(2),
                    width = reader.GetInt32(3),
                    height = reader.GetInt32(4)
                };
            }
            throw new Exception($"Photo with id {id} not found");
        }
    }

    internal void DeletePhoto(string id)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var delete = connection.CreateCommand();
            delete.CommandText = "DELETE FROM photos WHERE id = @id";
            delete.Parameters.AddWithValue("@id", id);
            delete.ExecuteNonQuery();
        }
    }

    internal IEnumerable<ImageRipper.Test.Collage> GetCollages()
    {
        var context = new ImageRipper.Test.PhotoContext();
        return context.Collages.ToList();
    }

    internal void SaveRecording(string id, string title)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            // insert a row
            var insert = connection.CreateCommand();
            insert.CommandText = "INSERT INTO recordings (id, title) VALUES (@id, @title)";
            insert.Parameters.AddWithValue("@id", id);
            insert.Parameters.AddWithValue("@title", title);
            insert.ExecuteNonQuery();
        }
    }

    internal IList<ImageRipper.Test.Recording> GetRecordings()
    {
        var context = new ImageRipper.Test.PhotoContext();
        return context.Recordings.ToList();
    }

    internal void DeleteRecording(string id)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var delete = connection.CreateCommand();
            delete.CommandText = "DELETE FROM recordings WHERE id = @id";
            delete.Parameters.AddWithValue("@id", id);
            if (0 == delete.ExecuteNonQuery())
            {
                throw new Exception($"Recording with id {id} not found");
            };
        }
    }
}
