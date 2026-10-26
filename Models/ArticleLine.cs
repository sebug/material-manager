namespace material_manager;

public record ArticleLine(string ArticleReference, string SupplierReference, string Mnemonic, string Label, string EAN13,
    string? SerialNumber, int Amount, DateOnly? NextControl)
{

}
