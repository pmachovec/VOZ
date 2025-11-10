using System.ComponentModel.DataAnnotations.Schema;
using VOZ.Database.Constants;

namespace VOZ.Database.Entities;

[Table(TableNames.ANSWERS)]
public sealed class Answer
{
    [Column(ColumnNames.ID)]
    public int Id { get; set; }

    [Column(ColumnNames.QUESTION_ID)]
    public int QuestionId { get; set; }

    [Column(ColumnNames.LABEL)]
    public string Label { get; set; } = default!;

    [Column(ColumnNames.TEXT)]
    public string Text { get; set; } = default!;

    [Column(ColumnNames.IS_CORRECT)]
    public bool IsCorrect { get; set; }

    public Question Question { get; set; } = default!;
}
