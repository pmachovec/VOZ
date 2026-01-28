using System.ComponentModel.DataAnnotations.Schema;
using VOZ.Database.Constants;

namespace VOZ.Database.Entities;

[Table(TableNames.QUESTIONS)]
public sealed class Question
{
    [Column(ColumnNames.ID)]
    public int Id { get; init; }

    [Column(ColumnNames.CATEGORY_ID)]
    public int CategoryId { get; init; }

    [Column(ColumnNames.SUBCATEGORY_ID)]
    public int? SubcategoryId { get; init; }

    [Column(ColumnNames.TEXT)]
    public string Text { get; init; } = default!;

    [Column(ColumnNames.PAGE_NUMBER)]
    public int? PageNumber { get; init; }

    public ICollection<Answer> Answers { get; set; } = new List<Answer>();

    public QuestionImage? QuestionImage { get; init; }

    public Subcategory Subcategory { get; init; } = default!;

    public Category Category => Subcategory.Category;
}
