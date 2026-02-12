using System.ComponentModel.DataAnnotations.Schema;
using VOZ.Shared.Database.Constants;

namespace VOZ.Shared.Database.Entities;

[Table(TableNames.QUESTION_IMAGES)]
public sealed class QuestionImage
{
    [Column(ColumnNames.ID)]
    public int Id { get; init; }

    [Column(ColumnNames.QUESTION_ID)]
    public int QuestionId { get; init; }

    [Column(ColumnNames.IMAGE)]
    public byte[]? Image { get; init; }

    [Column(ColumnNames.MIME_TYPE)]
    public string? MimeType { get; init; }

    public Question Question { get; init; } = default!;
}
