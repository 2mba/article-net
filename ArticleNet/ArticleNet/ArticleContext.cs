using System;

namespace Tumba.ArticleNet
{
    public class ArticleContext
    {
        private Uri url;

        public Uri Url
        {
            get => url;
            
            set
            {
                if (!url.IsAbsoluteUri)
                {
                    throw new ArgumentException("Absolute uri is expected");
                }
                
                url = value;
            }
        }
    }
}