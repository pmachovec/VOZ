using System.ComponentModel.DataAnnotations.Schema;
using VOZ.Database.Constants;

namespace VOZ.Database.Entities;

[Table(TableNames.QUESTION_IMAGES)]
public sealed class QuestionImage
{
    [Column(ColumnNames.ID)]
    public int Id { get; set; }

    [Column(ColumnNames.QUESTION_ID)]
    public int QuestionId { get; set; }

    [Column(ColumnNames.IMAGE)]
    public byte[]? Image { get; set; }

    [Column(ColumnNames.MIME_TYPE)]
    public string? MimeType { get; set; }

    [Column(ColumnNames.BBOX)]
    public string? Bbox { get; set; }

    public Question Question { get; set; } = default!;
}
