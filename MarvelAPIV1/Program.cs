
namespace MarvelAPIV1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddAuthorization();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            var characters = new List<Character>
            {
                new Character ("Maya Lopez", "Echo", "Protagonist"),
                new Character ("Agatha Harkness", "Agatha: Darkhold Diaries", "Antagonist"),
                new Character ("Peter Parker", "Your Friendly Neighborhood Spider-Man", "Protagonist")
            };

            var episodes = new List<Episode>
            {
                new Episode("Echo", 1, "Origin Story"),
                new Episode("Agatha: Darkhold Diaries", 1, "The Witch Returns"),
                new Episode("Your Friendly Neighborhood Spider-Man", 1, "The Spider's Bite")
            };

            var locations = new List<Location>
            {
                new Location("New York City", "Echo"),
                new Location("Westview", "Agatha: Darkhold Diaries"),
                new Location("New York City", "Your Friendly Neighborhood Spider-Man")
            };
            
            //Get Endpoints
            app.MapGet("/characters", () => characters).WithName("GetCharacters").WithOpenApi();
            app.MapGet("/episodes", () => episodes).WithName("GetEpisodes").WithOpenApi();
            app.MapGet("/locations", () => locations).WithName("GetLocations").WithOpenApi();

            // Post Endpoints
            app.MapPost("/characters", (Character character) =>
            {
                characters.Add(character);
                return Results.Created($"/characters/{character.Name}", character);
            }).WithName("CreateCharacter").WithOpenApi();

            app.MapPost("/episodes", (Episode episode) =>
            {
                episodes.Add(episode);
                return Results.Created($"/episodes/{episode.SeriesName}/{episode.EpisodeNumber}", episode);
            }).WithName("CreateEpisode").WithOpenApi();

            app.MapPost("/locations", (Location location) =>
            {
                locations.Add(location);
                return Results.Created($"/locations/{location.SeriesName}", location);
            }).WithName("CreateLocation").WithOpenApi();

            // Put Endpoints
            app.MapPut("/characters/{name}", (string name, Character updatedCharacter) =>
            {
                var character = characters.FirstOrDefault(c => c.Name == name);
                if (character is null)
                {
                    characters.Remove(character);
                    characters.Add(updatedCharacter);
                    return Results.NoContent();
                }
                return Results.NotFound();
            }).WithName("UpdateCharacter").WithOpenApi();

            app.MapPut("/episodes/{seriesName}/{episodeNumber}", (string seriesName, int episodeNumber, Episode updatedEpisode) =>
            {
                var episode = episodes.FirstOrDefault(e => e.SeriesName == seriesName && e.EpisodeNumber == episodeNumber);
                if (episode != null)
                {
                    episodes.Remove(episode);
                    episodes.Add(updatedEpisode);
                    return Results.NoContent();
                }
                return Results.NotFound();
            }).WithName("UpdateEpisode").WithOpenApi();

            app.MapPut("/locations/{name}", (string name, Location updatedLocation) =>
            {
                var location = locations.FirstOrDefault(l => l.Name == name);
                if (location != null)
                {
                    locations.Remove(location);
                    locations.Add(updatedLocation);
                    return Results.NoContent();
                }
                return Results.NotFound();
            }).WithName("UpdateLocation").WithOpenApi();

            // Delete Endpoints
            app.MapDelete("/characters/{name}", (string name) =>
            {
                var character = characters.FirstOrDefault(c => c.Name == name);
                if (character != null)
                {
                    characters.Remove(character);
                    return Results.NoContent();
                }
                return Results.NotFound();
            }).WithName("DeleteCharacter").WithOpenApi();

            app.MapDelete("/episodes/{seriesName}/{episodeNumber}", (string seriesName, int episodeNumber) =>
            {
                var episode = episodes.FirstOrDefault(e => e.SeriesName == seriesName && e.EpisodeNumber == episodeNumber);
                if (episode != null)
                {
                    episodes.Remove(episode);
                    return Results.NoContent();
                }
                return Results.NotFound();
            }).WithName("DeleteEpisode").WithOpenApi();

            app.MapDelete("/locations/{name}", (string name) =>
            {
                var location = locations.FirstOrDefault(l => l.Name == name);
                if (location != null)
                {
                    locations.Remove(location);
                    return Results.NoContent();
                }
                return Results.NotFound();
            }).WithName("DeleteLocation").WithOpenApi();



            app.Run();
        }

        public record Character(string Name, string SeriesName, string Role);
        public record Episode(string SeriesName, int EpisodeNumber, string Title);
        public record Location(string Name, string SeriesName);
    }
}
