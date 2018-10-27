using HtmlAgilityPack;
using RebisCrawler.Models;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RebisCrawler.CrawlerAll
{
    internal class Crawler
    {
        private string _url;
        private HttpClient _httpClient;
        private HtmlDocument _htmlDocument;

        public Crawler(string url)
        {
            _url = url;
            _httpClient = new HttpClient();
            _htmlDocument = new HtmlDocument();
        }

        public async Task<Book> GetBook()
        {
            var response = await _httpClient.GetByteArrayAsync(_url);
            var page = Encoding.UTF8.GetString(response);

            _htmlDocument.LoadHtml(page);

            var book = new Book
            {
                Title = GetTitle(),
                Details = GetDetails(),
                BookPrice = GetPrice(),
                EBookPrice = GetPrice(true),
                Description = GetDescription()
            };
            return book;
        }

        private TitleModel GetTitle()
        {
            var title = _htmlDocument.DocumentNode.SelectNodes("//h1[contains(@itemprop, 'name')]");
            var author = _htmlDocument.DocumentNode.SelectNodes("//a[contains(@class, 'author title')]");

            var titleText = title[0].InnerText.Replace("&#243;", "ó").TrimStart('\r', '\n');
            var index = titleText.IndexOf("\r\n");
            titleText = titleText.Remove(index);

            return new TitleModel
            {
                Title = PrepareText(titleText),
                Author = PrepareText(author[0].InnerText)
            };
        }

        private DescriptionModel GetDescription()
        {
            var description = _htmlDocument.DocumentNode.SelectNodes
                ("//div[contains(@itemprop, '" + PrepareCorrectName(nameof(DescriptionModel.Description)) + "')]");
            var bookDescription = _htmlDocument.DocumentNode.SelectNodes
                ("//div[contains(@class, '" + PrepareCorrectName(nameof(DescriptionModel.BookDescription)) + "')]");
            var authorBiogram = _htmlDocument.DocumentNode.SelectNodes
                 ("//div[contains(@class, '" + PrepareCorrectName(nameof(DescriptionModel.AuthorBiogram)) + "')]");

            return new DescriptionModel
            {
                Description = PrepareText(description[0].InnerText),
                BookDescription = PrepareText(bookDescription[1].InnerText),
                AuthorBiogram = PrepareText(authorBiogram[0].InnerText)
            };
        }

        private PriceModel GetPrice(bool eBook = false)
        {
            var bookFilter = _htmlDocument.DocumentNode.SelectNodes
                ("//h4[contains(@data-option, '" + PrepareCorrectName(nameof(PriceModel.BookFilter)) + "')]");
            var price = _htmlDocument.DocumentNode.SelectNodes
                ("//span[contains(@class, '" + PrepareCorrectName(nameof(PriceModel.Price)) + "')]");
            var oldPrice = _htmlDocument.DocumentNode.SelectNodes
                ("//span[contains(@class, 'oldPrice')]");
            var oldPriceInfo = _htmlDocument.DocumentNode.SelectNodes
                ("//span[contains(@class, 'oldPriceInfo')]");
            return new PriceModel
            {
                BookFilter = PrepareText(bookFilter[eBook ? 1 : 0].InnerText),
                Price = PrepareText(price[eBook ? 1 : 0].InnerText),
                OldPrice = PrepareText(oldPrice[eBook ? 1 : 0].InnerText),
                OldPriceInfo = PrepareText(oldPriceInfo[eBook ? 1 : 0].InnerText)
            };
        }

        private DetailsModel GetDetails()
        {
            return new DetailsModel
            {
                BookSerie = GetDdElement(nameof(DetailsModel.BookSerie)),
                BookCover = GetDdElement(nameof(DetailsModel.BookCover)),
                BookEdition = GetDdElement(nameof(DetailsModel.BookEdition)),
                BookFormatsize = GetDdElement(nameof(DetailsModel.BookFormatsize)),
                BookIsbn = GetDdElement(nameof(DetailsModel.BookIsbn)),
                BookOrigin = GetDdElement(nameof(DetailsModel.BookOrigin)),
                BookOriginalEditionDate = GetDdElement(nameof(DetailsModel.BookOriginalEditionDate)),
                BookPagescount = GetDdElement(nameof(DetailsModel.BookPagescount)),
                BookTranslator = GetDdElement(nameof(DetailsModel.BookTranslator)),
            };
        }

        private string GetDdElement(string name)
        {
            var nameCorrect = PrepareCorrectName(name);

            var ddElement = _htmlDocument.DocumentNode.SelectNodes("//dd[contains(@class, '" + nameCorrect + "')]");

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

        private static string PrepareCorrectName(string name)
        {
            return string.Concat(name
                .Select(c => char.IsUpper(c) ?
                "-" + c.ToString().ToLower() :
                c.ToString().ToLower()))
                .TrimStart('-');
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
