using material_manager.Entities;
using SQLite;

namespace material_manager;

public class MaterialDatabase : IMaterialDatabase
{
    protected SQLiteAsyncConnection? Database;
    /// <summary>
    /// We need to lock the initialization not that we get a database back with only half the tables intialized
    /// </summary>
    private readonly SemaphoreSlim _initializationLock = new SemaphoreSlim(1, 1);

    private async Task Init()
    {
        await _initializationLock.WaitAsync();
        try
        {
            if (Database != null)
            {
                return;
            }
            Database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);

            await Database.CreateTableAsync<Article>();
        }
        finally
        {
            _initializationLock.Release();
        }
    }

public async Task EnsureArticle(Article article)
    {
        await Init();
        int result = 0;
        await Database!.RunInTransactionAsync(conn =>
        {
            var existing = conn.Table<Article>().FirstOrDefault(
                art => art.EAN13 == article.EAN13
            );
            if (existing != null)
            {
                article.ArticleID = existing.ArticleID;
                conn.Update(article);
            }
            else
            {
                result = conn.Insert(article);
            }
        }); 
    }

    public async Task<List<Article>> GetAllArticles()
    {
        await Init();
        return await Database!.Table<Article>().ToListAsync();
    }

    public async Task<Article?> GetByEAN(string ean)
    {
        await Init();
        return await Database!.Table<Article>().FirstOrDefaultAsync(a => a.EAN13 == ean);
    }

    public async Task<bool> HasArticles()
    {
        await Init();
        return (await Database!.Table<Article>().CountAsync() > 0);
    }
}
