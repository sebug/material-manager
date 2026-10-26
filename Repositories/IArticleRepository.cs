using material_manager.Entities;

namespace material_manager;

public interface IArticleRepository
{
    Task Ensure(Article article);
    Task<List<Article>> GetAll();
    Task<Article?> GetByEAN(string ean);
}
