namespace ImageRipper;
using Microsoft.Data.Sqlite;

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
}
