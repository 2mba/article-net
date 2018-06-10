using System;
using System.Collections.Generic;

namespace Tumba.ArticleNet
{
    public class Article
    {
        public string Title { get; set; }
        
        public string RawText { get; set; }   
        
        public DateTime? PubDate { get; set; }
        
        public List<string> Authors { get; set; } 
    }
}