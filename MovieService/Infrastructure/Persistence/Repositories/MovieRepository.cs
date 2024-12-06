
using MongoDB.Bson;
using MongoDB.Driver;
using MovieService.Core.Entity.Model;
using MovieService.Core.Interfaces.Repository;
using MovieService.Infrastructure.Persistence.Context;

namespace MovieService.Infrastructure.Persistence.Repositories;

public class MovieRepository : MongoDBRepository<movies>, IMovieRepository
{
    public MovieRepository(MongoDBContext context) : base(context, "movies")
    {

    }
    
    //hàm này dê lấy tất cả dto, phục vụ cho bên movie schedule
    public async Task<List<movies>> GetAllMovieAsyncById(List<string> movieIds)
    {
        var filter = Builders<movies>.Filter.In(x => x.Id, movieIds);
        var movies = await _collection.Find(filter).ToListAsync();
        return movies;
    }
    
    //hàm tìm movie nâng cao
    public async Task<Dictionary<string, object>> GetAllAdvance(
        int page
        , int limit
        , List<string> name
        , List<string> director
        , List<string> actor
        , List<string> author
        , List<string> dub
        , List<string> subtitle
        , List<string> genres)
    {
        // tạo một list để lưu các sub filter
        var subFilters = new List<FilterDefinition<movies>>();

        // param name
        if (name != null && name.Count > 0)
        {
            var nameFilters = name.Select(n => Builders<movies>.Filter.Regex(x => x.Name, new BsonRegularExpression(n, "i")));
            subFilters.Add(Builders<movies>.Filter.Or(nameFilters));
        }
        
        // param director
        if (director != null && director.Count > 0)
        {
            var directorFilters = director.Select(d => Builders<movies>.Filter.Regex(x => x.Director, new BsonRegularExpression(d, "i")));
            subFilters.Add(Builders<movies>.Filter.Or(directorFilters));
        }
        // param actor
        if (actor != null && actor.Count > 0)
        {
            var actorFilters = actor.Select(a => Builders<movies>.Filter.Regex(x => x.Actors, new BsonRegularExpression(a, "i")));
            subFilters.Add(Builders<movies>.Filter.Or(actorFilters));
        }
        // param author
        if (author != null && author.Count > 0)
        {
            var authorFilters = author.Select(a => Builders<movies>.Filter.Regex(x => x.Author, new BsonRegularExpression(a, "i")));
            subFilters.Add(Builders<movies>.Filter.Or(authorFilters));
        }
        // param dub
        if (dub != null && dub.Count > 0)
        {
            var dubFilters = dub.Select(d => Builders<movies>.Filter.Regex(x => x.Dub, new BsonRegularExpression(d, "i")));
            subFilters.Add(Builders<movies>.Filter.Or(dubFilters));
        }
        // param subtitle
        if (subtitle != null && subtitle.Count > 0)
        {
            var subtitleFilters = subtitle.Select(s => Builders<movies>.Filter.Regex(x => x.SubTitle, new BsonRegularExpression(s, "i")));
            subFilters.Add(Builders<movies>.Filter.Or(subtitleFilters));
        }
        // param genres
        if (genres != null && genres.Count > 0)
        {
            subFilters.Add(Builders<movies>.Filter.AnyIn(x => x.Genres, genres));
        }
        
        // gộp các filter lại bằng or
        var combinedFilter = subFilters.Count > 0 ? Builders<movies>.Filter.Or(subFilters) : Builders<movies>.Filter.Empty;
        
        // lấy total record
        var totalRecords = await _collection.CountDocumentsAsync(combinedFilter);

        // lấy data
        var records = await _collection
            .Find(combinedFilter)
            .Skip((page - 1) * limit)
            .Limit(limit)
            .ToListAsync();

        // gộp lại thành 1 dictionary
        return new Dictionary<string, object>
        {
            { "totalRecords", totalRecords },
            { "records", records },
            { "limit", limit },
            { "page", page }
        };
    }
}