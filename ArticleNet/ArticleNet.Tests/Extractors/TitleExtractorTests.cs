using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using Tumba.ArticleNet;
using Tumba.ArticleNet.Extractors;

namespace ArticleNet.Tests.Extractors
{
    [TestFixture]
    public class TitleExtractorTests
    {
        private const string HeadlineMetaTagTemplate = 
            "<html>" +
            "<head>" +
            "<meta name=\"headline\" content=\"{0}\">" +
            "</head>" +
            "</html>";
        
        private const string TitleTagTemplate = 
            "<html>" +
            "<head>" +
            "</head>" +
            "<body>" +
            "<title>{0}</title>" +
            "</body>" +
            "</html>";
        
        [TestCase("my title", "my title")]
        [TestCase("hi my title", "my title")]
        [TestCase("www.example.com my title", "my title")]
        [TestCase("www.example.com my title", "my title")]
        [TestCase("| my title 1 | my title 2 |", "my title 1 | my title 2")]
        public void ShouldExtractFromOpenGraph(string title, string expectedTitle)
        {
            var htmlDocument = HtmlDocumentUtils.CreateHtmlDocument("<html></hmtl>");
            var context = new ExtractorContext(htmlDocument)
            {
                OpenGraph = new Dictionary<string, string>()
                {
                    {"title", title},
                    {"site_name", "hi"}
                },
                Domain = "www.example.com"
            };

            var extractor = new TitleExtractor();
            extractor.Execute(context);

            context.Title.Should().Be(expectedTitle);
        }
        
        [Test]
        public void ShouldExtractFromHeadlineMetaTag()
        {
            var htmlContent = string.Format(HeadlineMetaTagTemplate, "my title");
            var htmlDocument = HtmlDocumentUtils.CreateHtmlDocument(htmlContent);
            var context = new ExtractorContext(htmlDocument);

            var extractor = new TitleExtractor();
            extractor.Execute(context);

            context.Title.Should().Be("my title");
        }
        
        [Test]
        public void ShouldExtractFromTitleTag()
        {
            var htmlContent = string.Format(TitleTagTemplate, "my title");
            var htmlDocument = HtmlDocumentUtils.CreateHtmlDocument(htmlContent);
            var context = new ExtractorContext(htmlDocument);

            var extractor = new TitleExtractor();
            extractor.Execute(context);

            context.Title.Should().Be("my title");
        }
    }
}