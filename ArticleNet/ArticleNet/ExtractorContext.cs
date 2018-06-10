using System.Collections.Generic;
using HtmlAgilityPack;

namespace Tumba.ArticleNet
{
    internal class ExtractorContext
    {
        public ExtractorContext(HtmlDocument htmlDocument)
        {
            HtmlDocument = htmlDocument;
        }

        public HtmlDocument HtmlDocument { get; }

        public string Domain { get; set; }
        
        public string Title { get; set; }
        
        public Dictionary<string, string> OpenGraph { get; set; }
        
        public List<string> Authors { get; set; }
    }
}