using System;

namespace Tumba.ArticleNet
{
    public class ArticleExtractorException : Exception
    {
        public ArticleExtractorException(string message) : base(message)
        {
        }
    }
}