using System.ComponentModel.DataAnnotations.Schema;
using VOZ.QuestionGenerator.Constants;

namespace VOZ.QuestionGenerator.Entities;

[Table(TableNames.CATEGORIES)]
public sealed class Category
{
    [Column(ColumnNames.ID)]
    public int Id { get; set; }

    [Column(ColumnNames.NAME)]
    public string Name { get; set; } = default!;

    public ICollection<Subcategory> Subcategories { get; set; } = new List<Subcategory>();
}
