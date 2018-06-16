using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using FluentAssertions;
using NUnit.Framework;
using Tumba.ArticleNet;
using Tumba.ArticleNet.Extractors;
using Tumba.ArticleNet.Pipeline;

namespace ArticleNet.Tests.Functional
{
    [TestFixture]
    public class ArticleExtractorTests
    {
        
        [Test, Explicit]
        public void ExtractArticleFromExternalHtmlTest()
        {
            const string urlAddress = "https://www.kinopoisk.ru/film/7107/";
            var html = LoadHtml(urlAddress);

            if (html == null)
            {
                Assert.NotNull(html, "Html should be not null");
            }
            
            var articleExtractor = ArticleExtractor.Create(ExtractorConfiguration.Default);
            var article = articleExtractor.Extract(html);

            article.Should().NotBeNull();
            
            Console.WriteLine("Loaded article:" +  
                              "\nTitle = "+ article.Title + 
                              "\nAuthors = "+ string.Join(",", article.Authors ?? Enumerable.Empty<string>()));
        }
        
        [Test]
        public void SingleTitleExtractorTest()
        {
            var articleExtractor = new ArticleExtractor(new ExtractorPipelineConfigurer(), new ExtractorConfiguration());
            var article = articleExtractor.Extract("");

            article.Should().NotBeNull();
            article.Title.Should().NotBeNull();
            article.Title.Should().Be("Title");
        }
        
        [Test]
        public void DoubleTitleExtractorTest()
        {
            var articleExtractor = new ArticleExtractor(new DoubleTitleExtractorPipelineConfigurer(), new ExtractorConfiguration());
            var article = articleExtractor.Extract("");

            article.Should().NotBeNull();
            article.Title.Should().NotBeNull();
            article.Title.Should().Be("Title2");
        }

        private static string LoadHtml(string url)
        {
            var request = (HttpWebRequest) WebRequest.Create(url);
            var response = (HttpWebResponse) request.GetResponse();

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var receiveStream = response.GetResponseStream() ?? throw new NullReferenceException("Stream is null");

                var readStream = response.CharacterSet == null 
                    ? new StreamReader(receiveStream) 
                    : new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));

                var data = readStream.ReadToEnd();

                response.Close();
                readStream.Close();

                return data;
            }
            else
            {
                Console.WriteLine("Unable to load html. Status: " + response.StatusCode);
                return null;
            }
        } 
        

        private class ExtractorPipelineConfigurer : IExtractorPipelineConfigurer
        {
            public IExtractor[] Configure(ExtractorConfiguration config)
            {
                return new IExtractor[] { new SimpleTitleExtractor("Title") };
            }
        }

        private class DoubleTitleExtractorPipelineConfigurer : IExtractorPipelineConfigurer
        {
            public IExtractor[] Configure(ExtractorConfiguration config)
            {
                return new IExtractor[] { new SimpleTitleExtractor("Title"), new SimpleTitleExtractor("Title2") };
            }
        }
        
        private class SimpleTitleExtractor: IExtractor
        {
            private readonly string title;

            public SimpleTitleExtractor(string title)
            {
                this.title = title;
            }

            public void Execute(ExtractorContext context)
            {
                context.Title = title;
            }
        }
    }
}