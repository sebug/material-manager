using material_manager.Entities;

namespace material_manager;

public interface IArticleRepository
{
    Task EnsureArticle(Article article);
    Task<List<Article>> GetAllArticles();
}
