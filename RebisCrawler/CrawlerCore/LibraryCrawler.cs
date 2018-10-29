using HtmlAgilityPack;
using RebisCrawler.FileInterpreter;
using RebisCrawler.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RebisCrawler.CrawlerCore
{
    internal class LibraryCrawler
    {
        private const string Url = "https://www.rebis.com.pl";
        private HttpClient _httpClient;
        private HtmlDocument _htmlDocument;
        private BookCrawler _bookCrawler;

        private List<string> _links;
        private Dictionary<string, string> _categoryNamesPaths;
        private FileWriter _writer;

        public LibraryCrawler()
        {
            _htmlDocument = new HtmlDocument();
            _httpClient = new HttpClient();
            _links = new List<string>();
            _bookCrawler = new BookCrawler();
            _categoryNamesPaths = new Dictionary<string, string>();
            _categoryNamesPaths.Add(string.Empty, "Menu/KSIĄŻKI");
        }

        public async Task GetCategoriesAsync()
        {
            var response = await _httpClient.GetByteArrayAsync(Url);
            var page = Encoding.UTF8.GetString(response);

            _htmlDocument.LoadHtml(page);

            var categories = _htmlDocument.DocumentNode.SelectNodes
                ("//ul[contains(@class, 'submenu level1')]//a");

            foreach (var category in categories)
            {
                var link = category.GetAttributeValue("href", "failed");
                if (link == "failed")
                {
                    continue;
                }

                var categoryName = GetCategoryName(category.ChildNodes[0]);
                _links.Add(link);
                System.Console.WriteLine(categoryName);
                await Task.WhenAll(GetBooksAsync(Url + link, categoryName));
            }
        }

        private string GetCategoryName(HtmlNode category)
        {
            try
            {
                var toDelete = "/html[1]/html[1]/body[1]/header[1]/div[2]/div[1]/div[1]/div[2]/nav[1]/ul[1]/li[2]/ul[1]/";
                var toDeleteAtEnd = "/a[1]/h3[1]";
                var currentPath = category.XPath.Replace(toDelete, string.Empty).Replace(toDeleteAtEnd, string.Empty);
                var correctedPath = currentPath.Remove(currentPath.Length > 10 ? currentPath.Length - 12 : 0);
                var correctName = _categoryNamesPaths[correctedPath] + "/" + category.InnerText;
                _categoryNamesPaths.Add(currentPath, correctName);
                return correctName;
            }
            catch
            {
                return category.InnerText;
            }
        }

        private async Task GetBooksAsync(string url, string category)
        {
            var httpClient = new HttpClient();
            var response = await httpClient.GetByteArrayAsync(url);
            var page = Encoding.UTF8.GetString(response);

            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(page);

            var books = htmlDocument.DocumentNode.SelectNodes
                ("//div[contains(@class, 'BookListThumbnail')]//a");

            var listOfBooks = new List<Book>();

            await Task.Run(async () =>
            {
                foreach (var book in books)
                {
                    var link = book.GetAttributeValue("href", "failed");
                    _writer = new FileWriter("C:\\Users\\Snooking\\Source\\Repos\\RebisCrawler\\RebisCrawler\\template.csv");
                    listOfBooks.Add(await _bookCrawler.GetBook(Url + link, category));
                }
            });
            foreach (var book in listOfBooks)
            {
                _writer.WriteToFile(book);
            }
        }
    }
}
