using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using HtmlAgilityPack;

namespace CrawlerForSahibinden
{
    class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Sahibinden Showcase Listing \n");
            Console.WriteLine("Warning!\nAds are listed at 40 second intervals.\nA time limit has been set for sahibinden.com site security!");
            GetMainSiteLinks();
            Save();
        }
        private static void GetMainSiteLinks()
        {
            //The HtmlWeb library was used to retrieve the data.
            HtmlWeb web = new HtmlWeb();
            //MainLink has been written.
            HtmlDocument document = web.Load("https://www.sahibinden.com/");

            //Since the prices are on the ad detail page, a variable has been written to keep the detail pages.
            string detailLink = "";
            //mainlink
            string sahibindenLink = "https://www.sahibinden.com/";
            //Since there are 56 advertisements in total on the Home Page, a cycle has been written accordingly.
            for (int j = 1; j < 57; j++)
            {
                //40 second sleep after that again
                System.Threading.Thread.Sleep(40000);
                //chose ad html tags for sahibinden.com
                var link = document.DocumentNode.SelectNodes("//*[@id=\"container\"]/div[3]/div/div[3]/div[3]/ul/li[" + j + "]/a");
                int last = 0;
                //We got the href tag for the detail page link.
                link.ToList().ForEach(i => last = (i.OuterHtml.IndexOf("href=")));

                if (last != -1)
                {
                    link.ToList().ForEach(i => detailLink = (i.OuterHtml.Substring(9, last - 10)));
                    Console.WriteLine(j + ". Ad");
                    Details(sahibindenLink + detailLink);
                }
                else
                {
                    continue;
                }
            }
        }
        static List<string> Titles = new List<string>();
        static List<string> Prices = new List<string>();
        private static void Details(string url)
        {
            try
            {
                HtmlWeb web = new HtmlWeb();
                HtmlDocument document = web.Load(url);

                var title = document.DocumentNode.SelectNodes("//*[@id=\"classifiedDetail\"]/div/div[1]/h1/text()").First().InnerText;
                var price = document.DocumentNode.SelectNodes("//*[@id=\"classifiedDetail\"]/div/div[2]/div[2]/h3/text()").First().InnerText;

                Titles.Add(title);
                Prices.Add(price.Trim());

                Console.WriteLine(title);
                Console.WriteLine(price.Trim());
            }
            catch (Exception)
            {
                Console.WriteLine("No Ads Shown!");
                System.Threading.Thread.Sleep(10000);
            }
        }

        private static void Save()
        {
            StreamWriter file = new StreamWriter("..."); //path
            Titles.ForEach(titles => file.WriteLine(titles));
            Prices.ForEach(prices => file.WriteLine(prices));
            file.Close();
            Console.WriteLine("File saved successfully!");
        }
    }
}