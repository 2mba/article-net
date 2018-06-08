using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Tumba.ArticleNet.Extractors
{
    internal class TitleExtractor : IExtractor
    {
        private static readonly string[] TitleSplitters = { "|", "-", "»", ":" };

        public void Execute(ExtractorContext context, Action<ExtractorContext> next)
        {
            if (context.OpenGraph != null && context.OpenGraph.TryGetValue("title", out var title))
            {
                context.Title = CleanTitle(title, context);
                return;
            }

            var metaTagNodes = context.HtmlDocument.DocumentNode.SelectNodes("//meta");
            var headlineElement = metaTagNodes?.FirstOrDefault(n => n.GetAttributeValue("name", null) == "headline");

            if (headlineElement != null)
            {
                title = headlineElement.GetAttributeValue("content", null);
                if (title != null)
                {
                    context.Title = CleanTitle(title, context);
                    return;
                }
            }

            var titleElement = context.HtmlDocument.DocumentNode.SelectSingleNode("//title");
            if (titleElement != null)
            {
                title = titleElement.InnerText;
                if (title != null)
                {
                    context.Title = CleanTitle(title, context);
                    return;
                }
            }
        }

        private string CleanTitle(string title, ExtractorContext context)
        {
            // check if we have the site name in opengraph data
            if (context.OpenGraph != null && context.OpenGraph.TryGetValue("site_name", out var siteName))
            {
                title = title.Replace(siteName, string.Empty).Trim();
            }

            // try to remove the domain from url
            if (context.Domain != null)
            {
                var pattern = new Regex(context.Domain, RegexOptions.IgnoreCase);
                title = pattern.Replace(title, string.Empty);
            }
            
            // split the title in words
            var titleWords = title
                .Split(new [] {' '}, StringSplitOptions.RemoveEmptyEntries)
                .ToList();
             
            // check for an empty title 
            if (titleWords.Count == 0)
            {
                return string.Empty;
            }

            if (TitleSplitters.Contains(titleWords[0]))
            {
                titleWords.RemoveAt(0);
            }

            if (titleWords.Count > 0)
            {
                if (TitleSplitters.Contains(titleWords[titleWords.Count - 1]))
                {
                    titleWords.RemoveAt(titleWords.Count - 1);
                }
            }

            title = string.Join(" ", titleWords).Trim();
            return title;
        }    
    }
}