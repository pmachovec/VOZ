using System.ComponentModel.DataAnnotations.Schema;
using VOZ.Database.Constants;

namespace VOZ.Database.Entities;

[Table(TableNames.QUESTIONS)]
public sealed class Question
{
    [Column(ColumnNames.ID)]
    public int Id { get; set; }

    [Column(ColumnNames.CATEGORY_ID)]
    public int CategoryId { get; set; }

    [Column(ColumnNames.SUBCATEGORY_ID)]
    public int? SubcategoryId { get; set; }

    [Column(ColumnNames.TEXT)]
    public string Text { get; set; } = default!;

    [Column(ColumnNames.PAGE_NUMBER)]
    public int? PageNumber { get; set; }

    public ICollection<Answer> Answers { get; set; } = new List<Answer>();

    public QuestionImage? QuestionImage { get; set; }

    public Subcategory Subcategory { get; set; } = default!;

    public Category Category => Subcategory.Category;
}
