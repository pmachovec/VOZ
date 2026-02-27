using Microsoft.EntityFrameworkCore;
using VOZ.Shared.Database;
using VOZ.Shared.Database.Entities;

namespace VOZ.Shared.Repositories;

internal sealed class CategoryRepository(VOZDbContext _vozDbContext) : ICategoryRepository
{
    public async Task<IEnumerable<Category>> GetCategoriesWithSubcategoriesAsync(CancellationToken cancellationToken)
    {
        if (!await _vozDbContext.Categories.AnyAsync(cancellationToken))
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
