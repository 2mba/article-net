using ArticleNet.Tests.Utilities;
using FluentAssertions;
using NUnit.Framework;
using Tumba.ArticleNet.Extractors;

namespace ArticleNet.Tests.Extractors
{
    [TestFixture]
    public class AuthorsExtractorTest
    {
        private const string HtmlWithAuthor =
            "<html>" +
            "<head>" +
            "<title>Html Title</title>" +
            "</head>" +
        
            "<span itemprop=\"author\" itemscope itemtype=\"http://schema.org/Person\">" + 
            "    <span itemprop=\"name\">John Smith</span>" + 
            "</span>" +
        
            "</html>";

        private const string HtmlWithTwoAuthor =
            "<html>" +
            "<head>" +
            "<title>Html Title</title>" +
            "</head>" +
        
            "<span itemprop=\"name\">Gomer Simpson</span>" + // It is not author node name  
            "<span itemprop=\"author\" itemscope itemtype=\"http://schema.org/Person\">" + 
            "    <span itemprop=\"name\">John Smith</span>" + 
            "</span>" +
        
            "<div itemscope itemtype=\"http://schema.org/Article\">"+
            "    <h1 itemprop=\"name\">What's in a Name?</h1>" + 
            "    <span itemprop=\"author\">" + 
            "        <span itemprop=\"name\">Bill Gates</span>" + 
            "    </span>" +
            "</div>"+
            "</html>";
        
        private const string HtmlWithNoAuthor =
            "<html>" +
            "<head>" +
            "<title>Html Title</title>" +
            "</head>" +
        
            "<span>" + 
            "    <span>John Smith</span>" + 
            "</span>" +
            
            "</html>";
        
        [Test]
        public void ParseOneAuthorHtml()
        {
            var context = TestUtilites.ExecuteExtractor(HtmlWithAuthor, new AuthorsExtractor());

            context.Authors
                .Should()
                .HaveCount(1)
                .And
                .Contain("John Smith");
        }
        
        [Test]
        public void ParseTwoAuthorHtml()
        {
            var context = TestUtilites.ExecuteExtractor(HtmlWithTwoAuthor, new AuthorsExtractor());
            
            context.Authors
                .Should()
                .HaveCount(2)
                .And
                .Contain("John Smith", "Bill Gates");
        }
        
        [Test]
        public void ParseNoAuthorHtml()
        {
            var context = TestUtilites.ExecuteExtractor(HtmlWithNoAuthor, new AuthorsExtractor());
            
            context.Authors
                .Should()
                .HaveCount(0);
        }
    }
}