using ArticleNet.Tests.Utilities;
using FluentAssertions;
using NUnit.Framework;
using Tumba.ArticleNet;
using Tumba.ArticleNet.Extractors;

namespace ArticleNet.Tests.Extractors
{
    [TestFixture]
    public class OpenGraphExtractorTest
    {
        private const string HtmlWithOpenGraph =
            "<html>" +
            "<head>" +
            "<title>Html Title</title>" +
            "<meta property=\"og:title\" content=\"The Rock\" />" +
            "<meta property=\"og:type\" content=\"video.movie\" />" +
            "<meta property=\"og:url\" content=\"http://www.imdb.com/title/tt0117500/\" />" +
            "<meta property=\"og:image\" content=\"http://ia.media-imdb.com/images/rock.jpg\" />" +
            "</head>" +
            "</html>";

        [Test]
        public void ParseSimpleHtml()
        {
            var htmlDocument = TestUtilites.CreateHtmlDocument(HtmlWithOpenGraph);
            var context = new ExtractorContext(htmlDocument);

            var extractor = new OpengraphExtractor();
            extractor.Execute(context);

            context.OpenGraph.Should().ContainKeys("title", "type", "url", "image");
            
            context.OpenGraph["title"].Should().Be("The Rock");
            context.OpenGraph["type"].Should().Be("video.movie");
            context.OpenGraph["url"].Should().Be("http://www.imdb.com/title/tt0117500/");
            context.OpenGraph["image"].Should().Be("http://ia.media-imdb.com/images/rock.jpg");
        }
    }
}