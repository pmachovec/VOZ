using System.ComponentModel.DataAnnotations.Schema;
using VOZ.QuestionGenerator.Constants;

namespace VOZ.QuestionGenerator.Entities;

[Table(TableNames.SUBCATEGORIES)]
public sealed class Subcategory
{
    [Column(ColumnNames.ID)]
    public int Id { get; set; }

    [Column(ColumnNames.CATEGORY_ID)]
    public int CategoryId { get; set; }

    [Column(ColumnNames.NAME)]
    public string Name { get; set; } = default!;

    public Category Category { get; set; } = default!;

    public ICollection<Question> Questions { get; set; } = new List<Question>();
}
