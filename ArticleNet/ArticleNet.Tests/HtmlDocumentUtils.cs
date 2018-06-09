using HtmlAgilityPack;

namespace ArticleNet.Tests
{
    public static class HtmlDocumentUtils
    {
        public static HtmlDocument CreateHtmlDocument(string content)
        {
            var document = new HtmlDocument();
            document.LoadHtml(content);
            return document;
        }
    }
}