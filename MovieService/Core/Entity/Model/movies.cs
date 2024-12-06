using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MovieService.Core.Entity.Model;

public class movies
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    
    public required string Name { get; set; }
    public string? Director { get; set; }
    public string? Actors { get; set; }
    public string? Author { get; set; }
    public string? Description { get; set; }
    public string? Dub { get; set; }
    public string? SubTitle { get; set; }
    public int? Duration { get; set; }  //thời lượng phim tính bằng phút
    
    [BsonRepresentation(BsonType.String)]
    public List<string>? Genres { get; set; }
    
    //tạm thời lưu ảnh trong thư mục của server
    public string? Poster { get; set; }

    public movies(
        string name,
        string? director,
        string? actors,
        string? author,
        string? description,
        string? dub,
        string? subTitle,
        List<string>? genres
        )
    {
        Name = name;
        Director = director;
        Actors = actors;
        Author = author;
        Description = description;
        Dub = dub;
        SubTitle = subTitle;
        Genres = genres;
    }

    public movies(string id, string name, string? director, string? actors, string? author, string? description, string? dub, string? subTitle, List<string>? genres)
    {
        Id = id;
        Name = name;
        Director = director;
        Actors = actors;
        Author = author;
        Description = description;
        Dub = dub;
        SubTitle = subTitle;
        Genres = genres;
    }

    public movies()
    {
        
    }
}