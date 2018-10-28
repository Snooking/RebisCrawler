namespace RebisCrawler.Models
{
    internal class Book
    {
        public string CategoryPath { get; set; }
        public string ImagePath { get; set; }
        public TitleModel Title { get; set; }
        public DescriptionModel Description { get; set; }
        public DetailsModel Details { get; set; }
        public PriceModel BookPrice { get; set; }
        public PriceModel EBookPrice { get; set; }
    }
}
