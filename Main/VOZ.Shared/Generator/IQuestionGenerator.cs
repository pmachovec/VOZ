using VOZ.Shared.Database.Entities;

namespace VOZ.Shared.Generator;

internal interface IQuestionGenerator
{
    int QuestionsCount { get; }

    Task SetUpQuestionsAsync(CancellationToken cancellationToken);

    Task SetUpQuestionsAsync(HashSet<int> subcategoriesIds, CancellationToken cancellationToken);

    Question GetNextQuestion();
}
