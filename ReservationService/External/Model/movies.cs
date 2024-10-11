namespace ReservationService.External.Model;

public class movies
{
    public string Id { get; set; }
    
    public required string Name { get; set; }
    public string? Director { get; set; }
    public string? Actors { get; set; }
    public string? Author { get; set; }
    public string? Description { get; set; }
    public string? Dub { get; set; }
    public string? SubTitle { get; set; }
    
    public List<string>? Genres { get; set; }

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