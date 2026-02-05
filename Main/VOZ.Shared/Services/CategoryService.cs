using Microsoft.EntityFrameworkCore;
using VOZ.Shared.Database;
using VOZ.Shared.Database.Entities;

namespace VOZ.Shared.Services;

internal sealed class CategoryService(VOZDbContext _vozDbContext) : ICategoryService
{
    public async Task<IEnumerable<Category>> GetCategoriesWithSubcategoriesAsync(CancellationToken cancellationToken)
    {
        if (!_vozDbContext.Categories.Any())
        {
            throw new InvalidDataException("No categories available in the database!");
        }

        var categoriesWithSubcategories = await _vozDbContext
            .Categories
            .Where(category => category.Subcategories.Count > 0)
            .Include(category => category.Subcategories)
            .ToArrayAsync(cancellationToken);

        return categoriesWithSubcategories.Length == 0
            ? throw new InvalidDataException("No categories with subcategories available in the database!")
            : categoriesWithSubcategories;
    }
}
