using HtmlAgilityPack;
using Tumba.ArticleNet;
using Tumba.ArticleNet.Extractors;

namespace ArticleNet.Tests.Utilities
{
    internal static class TestUtilites
    {
        public static HtmlDocument CreateHtmlDocument(string content)
        {
            var document = new HtmlDocument();
            document.LoadHtml(content);
            return document;
        }
        
        public static ExtractorContext ExecuteExtractor(string html, params IExtractor[] extractors) {
            var htmlDocument = CreateHtmlDocument(html);
            var context = new ExtractorContext(htmlDocument);

            foreach (var extractor in extractors)
            {
                extractor.Execute(context);
            }
            return context;
        }
    }
}