using material_manager.Entities;

namespace material_manager.Services;

public interface IArticlesListReader
{
    List<ArticleLine>  ReadExcelFile(byte[] bytes);
}
