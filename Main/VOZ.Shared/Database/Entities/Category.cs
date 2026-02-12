using System.ComponentModel.DataAnnotations.Schema;
using VOZ.Shared.Database.Constants;

namespace VOZ.Shared.Database.Entities;

[Table(TableNames.CATEGORIES)]
public sealed class Category
{
    [Column(ColumnNames.ID)]
    public int Id { get; init; }

    [Column(ColumnNames.NAME)]
    public string Name { get; init; } = default!;

    public ICollection<Subcategory> Subcategories { get; init; } = [];
}
