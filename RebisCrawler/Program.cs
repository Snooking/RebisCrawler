using RebisCrawler.CrawlerAll;
using System;
using System.Threading.Tasks;

namespace RebisCrawler
{
    class Program
    {
        private const string Url = "https://www.rebis.com.pl/pl/book-odrebna-rzeczywistosc-carlos-castaneda,HCHB04117.html";

        static void Main(string[] args)
        {
            Task.Run(async () =>
            {
                var crawler = new Crawler(Url);
                var book = await crawler.GetBook();
                //Console.WriteLine(book.Title);
                //Console.WriteLine(book.Author);
                //Console.WriteLine(book.Description);
                //Console.WriteLine(book.Series);
                //Console.WriteLine(book.Translator);
                Console.ReadLine();
            }).GetAwaiter().GetResult();
        }
    }
}