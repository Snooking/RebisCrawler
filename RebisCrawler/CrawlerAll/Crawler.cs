using HtmlAgilityPack;
using RebisCrawler.Models;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace RebisCrawler.CrawlerAll
{
    internal class Crawler
    {
        private string _url;
        private HttpClient _httpClient;

        public Crawler(string url)
        {
            _url = url;
            _httpClient = new HttpClient();
        }

        public async Task<string> DownloadPage() => await _httpClient.GetStringAsync(_url);

        public async Task<Book> GetBook()
        {
            var page = await DownloadPage();

            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(page);

            var titleSectionHtml = htmlDocument.DocumentNode.SelectNodes("//title");

            var titleSection = titleSectionHtml[0].InnerText;

            var titleSectionArray = titleSection.Split(',');

            var title = titleSectionArray[0].Replace("&quot", string.Empty).Trim(' ', ';');
            var author = titleSectionArray[1].Trim(' ', ';');
            //var title = titleSection

            //System.Console.WriteLine(title);

            return new Book
            {

            };
        }

        private static HtmlNode GetNextDDSibling(HtmlNode dtElement)
        {
            var currentNode = dtElement;

            while (currentNode != null)
            {
                currentNode = currentNode.NextSibling;

                if (currentNode.NodeType == HtmlNodeType.Element && currentNode.Name == "dd")
                    return currentNode;
            }

            return null;
        }
    }
}
