﻿using Tumba.ArticleNet.Extractors;

namespace Tumba.ArticleNet.Pipeline
{
    internal class ExtractorPipelineConfigurer : IExtractorPipelineConfigurer
    {
        private readonly OpengraphExtractor opengraphExtractor = new OpengraphExtractor();
        private readonly TitleExtractor titleExtractor = new TitleExtractor();
        private readonly AuthorsExtractor authorsExtractor = new AuthorsExtractor();

        public IExtractor[] Configure(ExtractorConfiguration config)
        {
            return new IExtractor[]
            {
                opengraphExtractor,
                titleExtractor,
                authorsExtractor
            };
        }
    }
}