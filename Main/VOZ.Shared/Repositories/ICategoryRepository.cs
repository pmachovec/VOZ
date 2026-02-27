using VOZ.Shared.Database.Entities;

namespace VOZ.Shared.Repositories;

internal interface ICategoryRepository
{
    /// <summary>
    /// Returns available categories of questions, which have at least one subcategory.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token for the asynchronous operation.</param>
    /// <returns>Task with available categories of questions.</returns>
    /// <exception cref="InvalidDataException">
    /// Throw when no categories with subcategories are available in the database.
    /// </exception>
    Task<IEnumerable<Category>> GetCategoriesWithSubcategoriesAsync(CancellationToken cancellationToken);
}
