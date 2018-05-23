using System;
using System.Runtime.Serialization;

namespace Tumba.ArticleNet
{
    public class ExtractionException : Exception
    {
        public ExtractionException(string message) : base(message)
        {
        }
    }
}