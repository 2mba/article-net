namespace Tumba.ArticleNet
{
    internal class ArticleBuilder
    {
        public Article Build(ExtractorContext context)
        {
            var article = new Article
            {
                Title = context.Title
            };

            return article;
        }

        public static ArticleBuilder Create()
        {
            return new ArticleBuilder();
        }
    }
}