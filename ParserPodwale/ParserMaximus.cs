using HtmlAgilityPack;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Parsers
{
    public class ParserMaximus : Parser
    {
        public ParserMaximus() 
        {

        }
        public async Task<Website> GetDishes(string url)
        {
           
            List<Category> categories = new List<Category>();
            List<Dish> dishes = new List<Dish>();

            HtmlDocument doc = await GetSiteContent(url);

            HtmlNode contentNode = doc.DocumentNode.SelectNodes("//div[@id='content_col_left']").First();
            //HtmlNode categoryNode = doc.DocumentNode.SelectNodes("//div[@id='main_sub_menu']").First().SelectNodes(".//a[@class='active']").First();
            //List <HtmlNode> trNodes = contentNode.SelectNodes(".//tr").ToList();
            List<HtmlNode> pNodes = contentNode.SelectNodes(".//p").ToList();

            int day = 1;
            foreach (var p in pNodes)
            {
                
                if (p != null && !string.IsNullOrWhiteSpace(p.InnerText))
                {
                    dishes.Add(new Dish {
                        Name = Regex.Replace(p.InnerText, @"Pon|Wt|ŚR|CZW.|Pt", ""),
                        Day = (DayOfWeek)day
                    });
                }
                day++;
            }
                categories.Add(new Category { CategoryName = "Danie dnia - MAXIMUS", Dishes = dishes});

            //    List<HtmlNode> tdNodes = tr.SelectNodes(".//td").ToList();

            //    if(tdNodes != null && tdNodes.Count() > 1 && !string.IsNullOrWhiteSpace(tdNodes[0].InnerText) && !string.IsNullOrWhiteSpace(tdNodes[1].InnerText))
            //    {
            //        dishes.Add(new Dish
            //        {
            //            Name = Regex.Replace(tdNodes[0].InnerText, @"\t|\n|\r", "").Trim(),
            //            Price = GetPrice(Regex.Replace(tdNodes[1].InnerText, @"\t|\n|\r", "").Trim()),
            //            Price2 = tdNodes.Count() > 2 ? GetPrice(Regex.Replace(tdNodes[2].InnerText, @"\t|\n|\r", "").Trim()) : 0,
            //        });
            //    }
            //}

            //categories.Add(new Category { CategoryName = categoryNode.InnerText, Dishes = dishes });

            return new Website { Url = @"http://maximus.bielsko.pl/", Categories = categories };
        }
    }
}
