using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;

namespace targil
{
    // הממשק שמאפשר גמישות (נשאר אותו דבר)
    public interface IBookExporter
    {
        void Export(IEnumerable<Book> books);
    }

    // מימוש עבור CSV
    public class CsvExporter : IBookExporter
    {
        public void Export(IEnumerable<Book> books)
        {
            var csvPath = Path.Combine(Environment.CurrentDirectory, "processed_books.csv");
            var lines = new List<string> { "ID,Title,Author,Price,PublishDate" };

            lines.AddRange(books.Select(b =>
                $"{b.Id},{b.Title.Replace(",", "")},{b.Author.Replace(",", "")},{b.Price},{b.PublishDate:yyyy-MM-dd}"));

            File.WriteAllLines(csvPath, lines);
            Console.WriteLine($"[CSV] Success! File saved at: {csvPath}");
        }
    }

    // מימוש עבור Elastic (הקוד המקצועי שכולל HttpClient)
    public class ElasticExporter : IBookExporter
    {
        private static readonly HttpClient client = new HttpClient();
        private const string ElasticUrl = "http://localhost:9200/books/_doc";

        public void Export(IEnumerable<Book> books)
        {
            Console.WriteLine($"[Elastic] Attempting to send {books.Count()} books to index...");

            foreach (var book in books)
            {
                try
                {
                    var json = JsonConvert.SerializeObject(book);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    // סימולציית שליחה (במציאות השרת צריך להיות למעלה)
                    var response = client.PostAsync(ElasticUrl, content).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        Console.WriteLine($"[Elastic] Book {book.Id} indexed successfully.");
                    }
                }
                catch (Exception ex)
                {
                    // טיפול בשגיאה אם השרת לא זמין (נפוץ בסימולציות)
                    Console.WriteLine($"[Elastic Simulation] Record {book.Id} is ready for transport. Server Status: {ex.Message}");
                }
            }
        }
    }
}
