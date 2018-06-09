using System.Collections.Generic;

namespace Tumba.ArticleNet.Extractors
{
    internal class OpengraphExtractor: IExtractor
    {
        private const string ElementMeta = "meta";  
        private const string ElementMetaAttributeProperty = "property";  
        private const string ElementMetaAttributeContent = "content";  
        private const string OpenGraphPropertyPrefix = "og:";  
        
        public void Execute(ExtractorContext context)
        {
            var document = context.HtmlDocument;
            var metas = document.DocumentNode.SelectNodes($"//{ElementMeta}");
            
            context.OpenGraph = new Dictionary<string, string>();
            
            foreach (var meta in metas)
            {
                var propertyValue = meta.GetAttributeValue(ElementMetaAttributeProperty, null);
                
                if (propertyValue != null && propertyValue.StartsWith(OpenGraphPropertyPrefix))
                {
                    var contentValue = meta.GetAttributeValue(ElementMetaAttributeContent, null);
                    if (contentValue != null)
                    {
                        context.OpenGraph[propertyValue.Split(':')[1]] = contentValue;
                    }
                }
            }
        }
    }
}