using System.ComponentModel.DataAnnotations.Schema;
using VOZ.QuestionGenerator.Constants;

namespace VOZ.QuestionGenerator.Entities;

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

    [Column(ColumnNames.HEIGHT)]
    public float Height { get; set; }

    [Column(ColumnNames.WIDTH)]
    public float Width { get; set; }

    public Question Question { get; set; } = default!;
}
