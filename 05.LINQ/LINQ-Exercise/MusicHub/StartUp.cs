namespace MusicHub
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using Data;
    using Initializer;
    using Microsoft.EntityFrameworkCore;

    public class StartUp
    {
        public static void Main(string[] args)
        {
            MusicHubDbContext context = 
                new MusicHubDbContext();

            DbInitializer.ResetDatabase(context);


            //Console.WriteLine(ExportAlbumsInfo(context,9));
            Console.WriteLine(ExportSongsAboveDuration(context,4));
        }

        public static string ExportAlbumsInfo(MusicHubDbContext context, int producerId)
        {
            var sb = new StringBuilder();

            var albums = context
                .Albums
                .ToList()
                .Where(a => a.ProducerId == producerId)
                .Select(a => new
                {
                    AlbumName = a.Name,
                    ReleaseDate = a.ReleaseDate.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture),
                    ProducerName = a.Producer.Name,
                    Songs = a.Songs.Select(s => new
                    {
                        SongName = s.Name,
                        Price = s.Price,
                        Writer = s.Writer.Name
                    })
                        .ToList()
                        .OrderByDescending(s => s.SongName)
                        .ThenBy(s => s.Writer)
                        .ToList(),
                    AlbumPrice = a.Price
                })
                .OrderByDescending(a => a.AlbumPrice)
                .ToList();

            foreach (var album in albums)
            {
                sb.AppendLine($"-AlbumName: {album.AlbumName}");
                sb.AppendLine($"-ReleaseDate: {album.ReleaseDate}");
                sb.AppendLine($"-ProducerName: {album.ProducerName}");
                sb.AppendLine("-Songs:");

                var i = 1;

                foreach (var song in album.Songs)
                {
                    sb.AppendLine($"---#{i}");
                    sb.AppendLine($"---SongName: {song.SongName}");
                    sb.AppendLine($"---Price: {song.Price:f2}");
                    sb.AppendLine($"---Writer: {song.Writer}");

                    i++;
                }

                sb.AppendLine($"-AlbumPrice: {album.AlbumPrice:f2}");
            }

            return sb.ToString().Trim();
        }

        public static string ExportSongsAboveDuration(MusicHubDbContext context, int duration)
        {
            var db = context.Songs.ToList().Where(x => x.Duration.TotalSeconds > duration).Select(x => new
            {
                Writer = x.Writer.Name,
                x.Name,
                Preformer = x.SongPerformers.Select(x => new 
                {
                    FirstName = x.Performer.FirstName,
                    LastName = x.Performer.LastName
                }),
                Producer = x.Album.Producer.Name,
                x.Duration
            }).OrderBy(x => x.Name).ThenBy(x => x.Writer).ThenBy(x => x.Preformer).ToList();

            var sb = new StringBuilder();
            var i = 1;

            foreach (var song in db)
            {
                sb.AppendLine($"-Song #{i}");
                sb.AppendLine($"---SongName: {song.Name}");
                sb.AppendLine($"---Writer: {song.Writer}");
                foreach (var item in song.Preformer)
                {
                    sb.AppendLine($"---Performer: {item.FirstName} {item.LastName}");
                }
                sb.AppendLine($"---AlbumProducer: {song.Producer}");
                sb.AppendLine($"---Duration: {song.Duration}");

                i++;
            }
            return sb.ToString().Trim();
        }
    }
}
