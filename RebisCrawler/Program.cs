using RebisCrawler.CrawlerAll;
using System;
using System.Threading.Tasks;

namespace RebisCrawler
{
    class Program
    {
        private const string Url = "https://www.rebis.com.pl/pl/book-mowa-ciala-klamcow-lillian-glass,HCHB06139.html";

        static void Main(string[] args)
        {
            Task.Run(async () =>
            {
                var crawler = new Crawler(Url);
                var book = await crawler.GetBook();
                Console.WriteLine(book.Title.Title);
                Console.WriteLine(book.Title.Author);
                Console.WriteLine("BookCover: " + book.Details.BookCover);
                Console.WriteLine("BookSerie: " + book.Details.BookSerie);
                Console.WriteLine("BookEdition: " + book.Details.BookEdition);
                Console.WriteLine("BookFormatsize: " + book.Details.BookFormatsize);
                Console.WriteLine("BookIsbn: " + book.Details.BookIsbn);
                Console.WriteLine("BookOrigin: " + book.Details.BookOrigin);
                Console.WriteLine("BookOriginalEditionDate: " + book.Details.BookOriginalEditionDate);
                Console.WriteLine("BookPagescount: " + book.Details.BookPagescount);
                Console.WriteLine("BookTranslator: " + book.Details.BookTranslator);
                Console.ReadLine();
            }).GetAwaiter().GetResult();
        }
    }
}