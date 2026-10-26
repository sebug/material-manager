using material_manager.Entities;

namespace material_manager;

public class ArticleRepository : IArticleRepository
{
    private readonly IMaterialDatabase Database;
    public ArticleRepository(IMaterialDatabase database)
    {
        Database = database;
    }

    public Task Ensure(Article article)
    {
        return Database.EnsureArticle(article);
    }

    public Task<List<Article>> GetAll()
    {
        return Database.GetAllArticles();
    }

    public Task<Article?> GetByEAN(string ean)
    {
        return Database.GetByEAN(ean);
    }
}
