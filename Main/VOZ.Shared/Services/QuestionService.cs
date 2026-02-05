using Microsoft.EntityFrameworkCore;
using VOZ.Shared.Database;
using VOZ.Shared.Database.Entities;

namespace VOZ.Shared.Services;

[System.Diagnostics.CodeAnalysis.SuppressMessage(
    "Style",
    "IDE0046:Convert to conditional expression",
    Justification = "It makes the code hard to read."
)]
internal sealed class QuestionService(VOZDbContext _vozDbContext) :IQuestionService
{
    public async Task<Question[]> GetQuestionsWithAnswersAndImagesAsync(CancellationToken cancellationToken)
    {
        if (!_vozDbContext.Questions.Any())
        {
            throw new InvalidDataException("No questions available in the database!");
        }

        return await _vozDbContext
            .Questions
            .Include(question => question.Answers)
            .Include(question => question.QuestionImage)
            .ToArrayAsync(cancellationToken);
    }

    public async Task<Question[]> GetQuestionsWithAnswersAndImagesAsync(HashSet<int> subcategoriesIds, CancellationToken cancellationToken)
    {
        if (!_vozDbContext.Questions.Any())
        {
            throw new InvalidDataException("No questions available in the database!");
        }

        return await _vozDbContext
            .Questions
            .Where(question => subcategoriesIds.Contains(question.Subcategory.Id))
            .Include(question => question.Answers)
            .Include(question => question.QuestionImage)
            .ToArrayAsync(cancellationToken);
    }
}
