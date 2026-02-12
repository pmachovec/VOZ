using System.ComponentModel.DataAnnotations.Schema;
using VOZ.Shared.Database.Constants;

namespace VOZ.Shared.Database.Entities;

[Table(TableNames.SUBCATEGORIES)]
public sealed class Subcategory
{
    [Column(ColumnNames.ID)]
    public int Id { get; init; }

    [Column(ColumnNames.CATEGORY_ID)]
    public int CategoryId { get; init; }

    [Column(ColumnNames.NAME)]
    public string Name { get; init; } = default!;

    public Category Category { get; init; } = default!;

    public ICollection<Question> Questions { get; init; } = [];
}
