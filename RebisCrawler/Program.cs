using RebisCrawler.CrawlerCore;
using RebisCrawler.FileInterpreter;
using RebisCrawler.Models;
using System;
using System.Text;
using System.Threading.Tasks;

namespace RebisCrawler
{
    class Program
    {
        private const string Url = "https://www.rebis.com.pl/pl/book-mowa-ciala-klamcow-lillian-glass,HCHB06139.html";

        public static void Main(string[] args)
        {
            var bookCrawler = new BookCrawler();
            var libraryCrawler = new LibraryCrawler();
            libraryCrawler.GetCategoriesAsync().Wait();
            var a = 1;
        }

        private void writeAllInfo(Book book)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.WriteLine(book.CategoryPath);
            Console.WriteLine(book.ImagePath);
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
            Console.WriteLine();
            Console.WriteLine("BookFilter: " + book.BookPrice.BookFilter);
            Console.WriteLine("Price: " + book.BookPrice.Price);
            Console.WriteLine("OldPrice: " + book.BookPrice.OldPrice);
            Console.WriteLine("OldPriceInfo: " + book.BookPrice.OldPriceInfo);
            Console.WriteLine();
            Console.WriteLine("eBookFilter: " + book.EBookPrice.BookFilter);
            Console.WriteLine("ePrice: " + book.EBookPrice.Price);
            Console.WriteLine("eOldPrice: " + book.EBookPrice.OldPrice);
            Console.WriteLine("eOldPriceInfo: " + book.EBookPrice.OldPriceInfo);
            Console.WriteLine();
            Console.WriteLine("Description: " + book.Description.Description);
            Console.WriteLine("BookDescription: " + book.Description.BookDescription);
            Console.WriteLine("AuthorBiogram: " + book.Description.AuthorBiogram);
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
        }
    }
}