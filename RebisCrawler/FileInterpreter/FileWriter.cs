using RebisCrawler.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RebisCrawler.FileInterpreter
{
    internal class FileWriter
    {
        private string _path;
        private Dictionary<string, string> dictionary;
        private string[] _randomPrice;

        public FileWriter(string path)
        {
            _path = path;
            _randomPrice = new string[]
            {
                "39.99",
                "34.99",
                "29.99",
                "49.99"
            };
            using (StreamReader stream = new StreamReader(_path))
            {
                var template = stream.ReadLine().Split(';');
                dictionary = template.ToDictionary(key => key, value => string.Empty);
            }
        }

        public void WriteToFile(Book book)
        {
            var random = new Random();
            dictionary["sku"] = book.Details.BookIsbn;
            dictionary["categories"] = book.CategoryPath;
            dictionary["name"] = book.Title.Title;
            dictionary["description"] = book.Description.BookDescription;
            dictionary["short_description"] = book.Description.Description;
            dictionary["visibility"] = "Catalog, Search";
            dictionary["price"] = book.BookPrice.OldPrice?.Replace(" zł", string.Empty)?? _randomPrice[random.Next(0,3)];
            dictionary["special_price"] = book.BookPrice.Price?.Replace(" zł", string.Empty);
            dictionary["url_key"] = book.Title.Title.Replace(' ', '-');
            dictionary["meta_title"] = book.Title.Title;
            dictionary["meta_keywords"] = book.Title.Title;
            dictionary["meta_description"] = book.Title.Title;
            dictionary["base_image"] = book.ImagePath;
            dictionary["small_image"] = book.ImagePath;
            dictionary["thumbnail_image"] = book.ImagePath;
            dictionary["swatch_image"] = book.ImagePath;
            dictionary["additional_images"] = book.ImagePath;
            dictionary["qty"] = random.Next(5, 20).ToString();
            dictionary["attribute_set_code"] = "Default";
            dictionary["product_type"] = "simple";
            dictionary["product_websites"] = "base";
            dictionary["product_online"] = "1";
            dictionary["tax_class_name"] = "Taxable Goods";
            dictionary["display_product_options_in"] = "Block after Info Column";
            dictionary["gift_message_available"] = "Use config";
            dictionary["use_config_min_qty"] = "1";
            dictionary["is_qty_decimal"] = "0";
            dictionary["allow_backorders"] = "0";
            dictionary["use_config_backorders"] = "1";
            dictionary["min_cart_qty"] = "1.0000";
            dictionary["use_config_min_sale_qty"] = "1";
            dictionary["max_cart_qty"] = "10000.0000";
            dictionary["use_config_max_sale_qty"] = "1";
            dictionary["is_in_stock"] = "1";
            dictionary["notify_on_stock_below"] = "'1.0000";
            dictionary["use_config_notify_stock_qty"] = "1";
            dictionary["manage_stock"] = "1";
            dictionary["use_config_manage_stock"] = "1";
            dictionary["use_config_qty_increments"] = "1";
            dictionary["qty_increments"] = "1.0000";
            dictionary["use_config_enable_qty_inc"] = "1";
            dictionary["enable_qty_increments"] = "0";
            dictionary["is_decimal_divided"] = "0";
            dictionary["website_id"] = "0";
            dictionary["additional_attributes"] = "autor=" + book.Title.Author 
                + ",isbn=" + book.Details.BookIsbn
                + ",liczba_stron=" + book.Details.BookPagescount
                + ",oryginalny_tytul=" + book.Details.BookOrigin
                + ",seria=" + book.Details.BookSerie;
            using (StreamWriter writer = new StreamWriter(_path, true))
            {
                foreach (var item in dictionary)
                {
                    writer.Write(item.Value + ";");
                }
                writer.WriteLine();
            }
        }
    }
}
