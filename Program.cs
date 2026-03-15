using System;                      // פותר את השגיאה של Console ו-Exception
using System.Collections.Generic;    // פותר את השגיאה של List
using System.IO;                     // פותר את השגיאה של File
using System.Linq;                   // פותר את השגיאה של ה-Where וה-Select
using Newtonsoft.Json;               // פותר את השגיאה של JsonConvert

namespace targil
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // 1. קריאת הקובץ (ודא שהקובץ books.json נמצא בתיקייה הנכונה)
                if (!File.Exists("books.json"))
                {
                    Console.WriteLine("Error: books.json not found!");
                    return;
                }

                string jsonContent = File.ReadAllText("books.json");

                // 2. עיבוד המידע
                var allBooks = JsonConvert.DeserializeObject<List<Book>>(jsonContent);

                if (allBooks == null) return;

                var processedBooks = allBooks
                    .Where(b => b != null && !string.IsNullOrEmpty(b.Author))
                    .Where(b => !b.Author.Contains("Peter", StringComparison.OrdinalIgnoreCase))
                    .Where(b => b.PublishDate.DayOfWeek != DayOfWeek.Saturday)
                    .Select(b => {
                        b.Price = Math.Ceiling(b.Price);
                        return b;
                    })
                    .OrderBy(b => b.Title)
                    .ToList();

                // 3. הגדרת היעדים (CSV ו-Elastic)
                var exporters = new List<IBookExporter>
                {
                    new CsvExporter(),
                    new ElasticExporter()
                };

                // 4. שמירה
                foreach (var exporter in exporters)
                {
                    exporter.Export(processedBooks);
                }

                Console.WriteLine("\nDone! Success!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}