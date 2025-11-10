using System.ComponentModel.DataAnnotations.Schema;
using VOZ.Database.Constants;

namespace VOZ.Database.Entities;

[Table(TableNames.SUBCATEGORIES)]
public sealed class Subcategory
{
    [Column(ColumnNames.ID)]
    public int Id { get; set; }

    [Column(ColumnNames.CATEGORY_ID)]
    public int CategoryId { get; set; }

    [Column(ColumnNames.NAME)]
    public string Name { get; set; } = null!;

    public Category Category { get; set; } = null!;

    public ICollection<Question> Questions { get; set; } = new List<Question>();
}
