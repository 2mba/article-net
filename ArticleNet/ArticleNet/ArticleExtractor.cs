using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using Tumba.ArticleNet;
using Tumba.ArticleNet.Extractors;
using IExtractor = Tumba.ArticleNet.Extractors.IExtractor;

namespace Tumba.ArticleNet
{
    
    public class ArticleExtractor
    {
        private readonly Action<ExtractorContext> pipeline;

        internal ArticleExtractor(IExtractorPipelineConfigurer extractorPipelineConfigurer, ExtractorConfiguration config)
        {
            var pipelineBuilder = new PipelineBuilder(extractorPipelineConfigurer);
            pipeline = pipelineBuilder.BuildPipeline(config);

        }

        public Article Extract(string html)
        {
            HtmlDocument document;
            string errorMessage;
            
            if (!TryCreateHtmlDocument(html, out document, out errorMessage))
            {
                throw new ExtractionException(errorMessage);
            }

            var context = new ExtractorContext(document);
            pipeline(context);
            
            return new Article
            {
                Title = context.Title
            };
        }

        public static ArticleExtractor Create(ExtractorConfiguration config)
        {
            return new ArticleExtractor(new ExtractorPipelineConfigurer(), config);
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
    
    internal class ExtractorContext
    {
        public ExtractorContext(HtmlDocument htmlDocument)
        {
            HtmlDocument = htmlDocument;
        }

        public HtmlDocument HtmlDocument { get; }
        
        public string Title { get; set; }
        
        public Dictionary<string, string> OpenGraph { get; set; }
    }

    internal interface IExtractorPipelineConfigurer
    {

        IExtractor[] Configure(ExtractorConfiguration config);
    }


    internal class ExtractorPipelineConfigurer : IExtractorPipelineConfigurer
    {
        private OpengraphExtractor opengraphExtractor = new OpengraphExtractor();
        private TitleExtractor titleExtractor = new TitleExtractor();

        public IExtractor[] Configure(ExtractorConfiguration config)
        {
            return new IExtractor[] { opengraphExtractor, titleExtractor };
        }
    }

    internal class PipelineBuilder
    {

        private readonly IExtractorPipelineConfigurer extractorPipelineConfigurer;

        public PipelineBuilder(IExtractorPipelineConfigurer extractorPipelineConfigurer)
        {
            this.extractorPipelineConfigurer = extractorPipelineConfigurer;
        }

        public Action<ExtractorContext> BuildPipeline(ExtractorConfiguration config)
        {
            var extractors = extractorPipelineConfigurer.Configure(config);
            return BuildPipelineDelegate(extractors);
        }

        private static Action<ExtractorContext> BuildPipelineDelegate(IExtractor[] extractors)
        {
            Action<ExtractorContext> action = c => { ExecuteNext(extractors, c, 0); };

            return action;
        }

        private static void ExecuteNext(IExtractor[] extractors, ExtractorContext c, int idx)
        {
            if (idx >= extractors.Length)
            {
                return;
            }
            
            extractors[idx].Execute(c, context => ExecuteNext(extractors, c, idx + 1));}
        }
    
   
}