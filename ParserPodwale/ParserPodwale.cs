using HtmlAgilityPack;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Parsers
{
    public class PodwaleParser : Parser
    {
        public PodwaleParser()
        { 
            
        }
        public async Task<Website> GetDishes(string url)
        {
            
            List<Category> categories = new List<Category>();

            HtmlDocument doc = await GetSiteContent(url);

            HtmlNode panelsNode = doc.DocumentNode.SelectNodes("//div[@class='vc_tta-panels']").First();

            List<HtmlNode> dayNodes = new List<HtmlNode>();

            dayNodes.Add(panelsNode.SelectNodes("//div[@id='tb-mon']").First());
            dayNodes.Add(panelsNode.SelectNodes("//div[@id='tb-tue']").First());
            dayNodes.Add(panelsNode.SelectNodes("//div[@id='tb-wed']").First());
            dayNodes.Add(panelsNode.SelectNodes("//div[@id='tb-thu']").First());
            dayNodes.Add(panelsNode.SelectNodes("//div[@id='tb-fri']").First());
            dayNodes.Add(panelsNode.SelectNodes("//div[@id='tb-sat']").First());

            int aDay = 1;

            foreach (var day in dayNodes)
            {

                day.SelectNodes(".//h2[@class='title']")
                    .ToList()
                    .ForEach(x =>
                     {
                         bool found = false;

                         foreach (var categ in categories)
                         {
                             if (categ.CategoryName == x.InnerText)
                             {
                                 found = true;
                                 break;
                             }
                         }

                         if(!found)
                            categories.Add(new Category { CategoryName = x.InnerText, Dishes = new List<Dish>() });

                     });

                int cat = 1;

                foreach (var item in day.SelectNodes(".//div[contains(@class, 'wpb_text_column')]").ToList())
                {
                    List<HtmlNode> titleNodes = CheckInnerText(item.SelectNodes(".//h4").ToList());
                    List<HtmlNode> descNodes;
                    
                    try
                    { 
                        descNodes = CheckInnerText(item.SelectNodes(".//p[@class='menu_desc']").ToList());
                    }
                    catch(Exception e)
                    {
                        List<HtmlNode> descNodesTemp = new List<HtmlNode>();
                        descNodes = item.SelectNodes(".//p[@style='text-align: center;']").ToList();

                        for (int c = 0; c < descNodes.Count(); c += 3)
                        {
                            descNodesTemp.Add(descNodes[c]);
                        }

                        descNodes = CheckInnerText(descNodesTemp);

                    }

                    List<HtmlNode> priceNodes = item.SelectNodes(".//span[@class='highlight']").ToList();

                    List<Dish> dishes = new List<Dish>();

                    for (int j = 0; j < Math.Min(priceNodes.Count(), Math.Min(titleNodes.Count(), descNodes.Count())); j++)
                    {
                        dishes.Add(new Dish
                        {
                            Name = titleNodes[j].InnerText,
                            Description = descNodes[j].InnerText,
                            Price = GetPrice(priceNodes[j].InnerText),
                            Day = (DayOfWeek)aDay
                        });
                        
                    }

                    categories[cat].Dishes = categories[cat].Dishes.Concat(dishes).ToList();

                    cat++;
                }
                aDay++;
            }

            categories.RemoveAll(x => x.Dishes.Count() < 1);
            return new Website { Categories = categories, Url = @"http://www.stolowkapodwale.pl" };
        }


    }

}
