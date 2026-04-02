using Microsoft.EntityFrameworkCore;
using VOZ.Shared.Database;
using VOZ.Shared.Database.Entities;

namespace VOZ.Shared.Repositories;

[System.Diagnostics.CodeAnalysis.SuppressMessage(
    "Style",
    "IDE0046:Convert to conditional expression",
    Justification = "It makes the code hard to read."
)]
internal sealed class QuestionRepository(VOZDbContext _vozDbContext) : IQuestionRepository
{
    public async Task<Question[]> GetQuestionsWithAnswersAndImagesAsync(CancellationToken cancellationToken)
    {
        if (!await _vozDbContext.Questions.AnyAsync(cancellationToken))
        {
            throw new InvalidDataException("No questions available in the database!");
        }

        return await _vozDbContext
            .Questions
            .Include(question => question.Answers)
            .Include(question => question.QuestionImage)
            .ToArrayAsync(cancellationToken);
    }

    public async Task<Question[]> GetQuestionsWithAnswersAndImagesAsync(ISet<int> subcategoriesIds, CancellationToken cancellationToken)
    {
        if (!await _vozDbContext.Questions.AnyAsync(cancellationToken))
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
