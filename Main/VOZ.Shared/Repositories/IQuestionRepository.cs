using VOZ.Shared.Database.Entities;

namespace VOZ.Shared.Repositories;

internal interface IQuestionRepository
{
    Task<Question[]> GetQuestionsWithAnswersAndImagesAsync(CancellationToken cancellationToken);

    Task<Question[]> GetQuestionsWithAnswersAndImagesAsync(ISet<int> subcategoriesIds, CancellationToken cancellationToken);
}
