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

        public async Task<Book> GetBook()
        {
            var page = await _httpClient.GetStringAsync(_url);

            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(page);

            var book = new Book
            {
                Title = GetTitle(htmlDocument),
                Details = GetDetails(htmlDocument),
                Description = GetDescription(htmlDocument)
            };
            return book;
        }

        private static TitleModel GetTitle(HtmlDocument htmlDocument)
        {
            var titleSectionHtml = htmlDocument.DocumentNode.SelectNodes("//title");

            var titleSection = titleSectionHtml[0].InnerText;

            var titleSectionArray = titleSection.Split(',');

            var title = titleSectionArray[0].Replace("&quot", string.Empty).Trim(' ', ';');
            var author = titleSectionArray[1].Trim(' ', ';');

            return new TitleModel
            {
                Title = title,
                Author = author
            };
        }

        private static DescriptionModel GetDescription(HtmlDocument htmlDocument)
        {

            return new DescriptionModel
            {

            };
        }

        private static DetailsModel GetDetails(HtmlDocument htmlDocument)
        {
            return new DetailsModel
            {
                BookSerie = GetDdElement(htmlDocument, nameof(DetailsModel.BookSerie)),
                BookCover = GetDdElement(htmlDocument, nameof(DetailsModel.BookCover)),
                BookEdition = GetDdElement(htmlDocument, nameof(DetailsModel.BookEdition)),
                BookFormatsize = GetDdElement(htmlDocument, nameof(DetailsModel.BookFormatsize)),
                BookIsbn = GetDdElement(htmlDocument, nameof(DetailsModel.BookIsbn)),
                BookOrigin = GetDdElement(htmlDocument, nameof(DetailsModel.BookOrigin)),
                BookOriginalEditionDate = GetDdElement(htmlDocument, nameof(DetailsModel.BookOriginalEditionDate)),
                BookPagescount = GetDdElement(htmlDocument, nameof(DetailsModel.BookPagescount)),
                BookTranslator = GetDdElement(htmlDocument, nameof(DetailsModel.BookTranslator)),
            };
        }

        private static string GetDdElement(HtmlDocument htmlDocument, string name)
        {
            var nameCorrect = string.Concat(name
                .Select(c => char.IsUpper(c) ?
                "-" + c.ToString().ToLower() :
                c.ToString().ToLower()))
                .TrimStart('-');

            var ddElement = htmlDocument.DocumentNode.SelectNodes("//dd[contains(@class, '" + nameCorrect + "')]");

            var currentNode = ddElement?[0];

            while (currentNode != null)
            {
                if (currentNode.NodeType == HtmlNodeType.Element && currentNode.Name == "dd")
                {
                    return PrepareText(currentNode.InnerText);
                }

                currentNode = currentNode.NextSibling;
            }

            return null;
        }

        private static string PrepareText(string text)
        {
            var correctText = text.Trim().Replace("\r\n", string.Empty); ;
            while (correctText.Contains("  "))
            {
                correctText = correctText.Replace("  ", " ");
            }

            return correctText;
        }
    }
}
