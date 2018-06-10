using System.Diagnostics.CodeAnalysis;
using Tumba.ArticleNet;
using Tumba.ArticleNet.Extractors;

namespace ArticleNet.Tests.Utils
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    internal class ExtractorTestHelper
    {

        private readonly IExtractor[] extractors;
        private readonly string html;

        public ExtractorTestHelper(string html, params IExtractor[] extractors)
        {
            this.extractors = extractors;
            this.html = html;
        }

        public ExtractorContext Execute()
        {
            var htmlDocument = HtmlDocumentUtils.CreateHtmlDocument(html);
            var context = new ExtractorContext(htmlDocument);

            foreach (var extractor in extractors)
            {
                extractor.Execute(context);
            }
            return context;
        }
    }
}