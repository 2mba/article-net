using System;
using HtmlAgilityPack;
using Tumba.ArticleNet.Extractors;
using Tumba.ArticleNet.Pipeline;

namespace Tumba.ArticleNet
{
    public class ArticleExtractor
    {
        private readonly ArticleBuilder articleBuilder;
        private readonly IExtractor[] extractors;
        
        internal ArticleExtractor(IExtractorPipelineConfigurer extractorPipelineConfigurer, ExtractorConfiguration config)
        {
            this.articleBuilder = ArticleBuilder.Create();
            this.extractors = extractorPipelineConfigurer.Configure(config);
        }

        public static ArticleExtractor Create(ExtractorConfiguration config)
        {
            return new ArticleExtractor(new ExtractorPipelineConfigurer(), config);
        }

        public Article Extract(string html, ArticleContext articleContext = null)
        {
            if (!TryCreateHtmlDocument(html, out var document, out var errorMessage))
            {
                throw new ArticleExtractorException(errorMessage);
            }

            var context = new ExtractorContext(document);
            
            if (articleContext != null)
            {
                if (articleContext.Url != null)
                {
                    context.Domain = articleContext.Url.Host;
                }
            }

            foreach (var extractor in extractors)
            {
                extractor.Execute(context);
            }

            return articleBuilder.Build(context);
        }

        private static bool TryCreateHtmlDocument(string html, out HtmlDocument document, out string message)
        {
            try
            {
                document = new HtmlDocument();
                document.LoadHtml(html);
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