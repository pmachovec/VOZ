using VOZ.Shared.Database.Entities;

namespace VOZ.Shared.Application;

internal interface IQuestionGenerator
{
    int QuestionsCount { get; }

    Task SetUpQuestionsAsync(CancellationToken cancellationToken);

    Task SetUpQuestionsAsync(ISet<int> subcategoriesIds, CancellationToken cancellationToken);

    Question GetNextQuestion();
}
