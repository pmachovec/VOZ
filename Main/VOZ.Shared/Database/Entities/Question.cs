using System.ComponentModel.DataAnnotations.Schema;
using VOZ.Shared.Database.Constants;

namespace VOZ.Shared.Database.Entities;

[Table(TableNames.QUESTIONS)]
public sealed class Question
{
    [Column(ColumnNames.ID)]
    public int Id { get; init; }

    [Column(ColumnNames.SUBCATEGORY_ID)]
    public int? SubcategoryId { get; init; }

    [Column(ColumnNames.TEXT)]
    public string Text { get; init; } = default!;

    [Column(ColumnNames.PAGE_NUMBER)]
    public int? PageNumber { get; init; }

    public ICollection<Answer> Answers { get; set; } = [];

    public QuestionImage? QuestionImage { get; init; }

    public Subcategory Subcategory { get; init; } = default!;
}
