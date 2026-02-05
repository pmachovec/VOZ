using VOZ.Shared.Database.Entities;

namespace VOZ.Shared.Services;

internal interface IQuestionService
{
    Task<Question[]> GetQuestionsWithAnswersAndImagesAsync(CancellationToken cancellationToken);

    Task<Question[]> GetQuestionsWithAnswersAndImagesAsync(HashSet<int> subcategoriesIds, CancellationToken cancellationToken);
}
