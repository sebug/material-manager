using SQLite;

namespace material_manager.Entities;

public class Article
{
    [PrimaryKey]
    [AutoIncrement]
    public int ArticleID { get; set; }

    public string? ArticleReference { get; set; }
    public string? SupplierReference { get; set; }
    public string? Label { get; set; }
    public string? Mnemonic { get; set; }
    [Indexed]
    public string EAN13 { get; set; } = String.Empty;
    public string? SerialNumber { get; set; }
    public int Amount { get; set; }
    public DateOnly? NextControl { get; set; }
}
