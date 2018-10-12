using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Parsers
{
    public class Parser
    {
        public Parser()
        {

        }

        protected async Task<HtmlDocument> GetSiteContent(string url)
        {
            HttpClient http = new HttpClient();
            var response = await http.GetByteArrayAsync(url);

            string source = Encoding.GetEncoding("utf-8").GetString(response, 0, response.Length - 1);
            source = WebUtility.HtmlDecode(source);

            HtmlDocument res = new HtmlDocument();
            res.LoadHtml(source);

            return res;
        }

        protected List<HtmlNode> CheckInnerText(List<HtmlNode> list)
        {
            List<int> indexToDelete = new List<int>();

            for (int i = 0; i < list.Count(); i++)
            {
                if (string.IsNullOrWhiteSpace(list[i].InnerText))
                    indexToDelete.Add(i);
            }

            foreach (var item in indexToDelete)
            {
                list.RemoveAt(item);
            }

            return list;
        }

        protected decimal GetPrice(string str)
        {
            bool firstDigit = false;
            bool decimalPoint = false;
            string result = "";

            for (int i = 0; i < str.Count(); i++)
            {
                if (char.IsDigit(str[i]))
                {
                    firstDigit = true;
                    result += str[i];
                }
                else if ((str[i] == ',' ||str[i] == '.') && firstDigit && !decimalPoint)
                {
                    result += ',';
                    decimalPoint = true;
                }
                else if (!char.IsDigit(str[i]) && !firstDigit)
                {
                    break;
                }
            }

            decimal price = 0;
            decimal.TryParse(result, out price);

            return price;
        }
    }
}
