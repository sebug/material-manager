using material_manager.Entities;

namespace material_manager;

public interface IMaterialDatabase
{
    Task EnsureArticle(Article article);
    Task<List<Article>> GetAllArticles();
    Task<Article?> GetByEAN(string ean);
    Task<bool> HasArticles();
}
