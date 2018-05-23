using System;
using FluentAssertions;
using NUnit.Framework;
using Tumba.ArticleNet;
using Tumba.ArticleNet.Extractors;

namespace ArticleNet.Tests
{
    [TestFixture]
    public class ArticleExtractorTests
    {

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

            public void Execute(ExtractorContext context, Action<ExtractorContext> next)
            {
                context.Title = title;
                next(context);
            }
        }
    }
}