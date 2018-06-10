using System;
using System.Collections.Generic;

namespace Tumba.ArticleNet.Extractors
{
    internal class AuthorsExtractor: IExtractor
    {
        
        public void Execute(ExtractorContext context)
        {
            var document = context.HtmlDocument;
            var authors = new List<string>();
            context.Authors = authors;

            var authorNodes = document.DocumentNode.SelectNodes("//*[@itemprop=\"author\"]");

            if (authorNodes == null)
            {
                return;
            }
            
            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var node in authorNodes)
            {
                var nameNode = node.SelectSingleNode(".//*[@itemprop=\"name\"]");

                if (nameNode != null && nameNode.HasChildNodes)
                {
                    authors.Add(nameNode.FirstChild.InnerText);
                }
            }
        }
    }
}