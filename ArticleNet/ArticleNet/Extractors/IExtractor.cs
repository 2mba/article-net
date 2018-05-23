using System;

namespace Tumba.ArticleNet.Extractors
{
    internal interface IExtractor
    {
        void Execute(ExtractorContext context, Action<ExtractorContext> next);
    }
}