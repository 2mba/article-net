using System;

namespace Tumba.ArticleNet
{
    internal interface IHtmlDocument
    {
        
    }

    internal class HtmlDocument : IHtmlDocument
    {
        private HtmlAgilityPack.HtmlDocument document;

        private HtmlDocument(HtmlAgilityPack.HtmlDocument document)
        {
            this.document = document;
        }

        public static bool TryCreate(string html, out HtmlDocument document, out string message)
        {
            try
            {
                var htmlDocument = new HtmlAgilityPack.HtmlDocument();
                htmlDocument.LoadHtml(html);
                document = new HtmlDocument(htmlDocument);
                message = null;
                return true;
            }
            catch (Exception ex)
            {
                document = null;
                message = ex.Message;
                return false;
            }
        }
    }
}