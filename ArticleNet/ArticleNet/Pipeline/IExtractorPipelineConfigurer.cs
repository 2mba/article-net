using Tumba.ArticleNet.Extractors;

namespace Tumba.ArticleNet.Pipeline
{
    internal interface IExtractorPipelineConfigurer
    {
        IExtractor[] Configure(ExtractorConfiguration config);
    }
}