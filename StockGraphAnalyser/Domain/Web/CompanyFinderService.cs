namespace StockGraphAnalyser.Domain.Web
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using HtmlAgilityPack;

    public class SymbolFinderService 
    {
        private readonly HttpClient client = new HttpClient();

        /// <summary>
        /// Gets all symbols.
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> GetAllSymbols()
        {
            var alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
            var symbolList = new Dictionary<string, string>();
            var document = new HtmlDocument();
            foreach (var letter in alphabet)
            {
                var responseStream = WebRequest.Create("http://uk.advfn.com/exchanges/LSE/" + letter).GetResponse().GetResponseStream();
                new StreamReader(responseStream).ReadToEnd();
                document.LoadHtml(new StreamReader(responseStream).ReadToEnd());
                var tickerRows = document.DocumentNode.SelectNodes("//tr[@class='even'or @class='odd' or @class='odd first']").Skip(1);
                tickerRows.ToList().ForEach(r => symbolList.Add(r.ChildNodes.ElementAt(1).InnerText, r.ChildNodes.ElementAt(0).InnerText));
            }

            return symbolList;
        }
    }
}