using VOZ.QuestionGenerator.Entities;

namespace VOZ.QuestionGenerator;

public interface IQuestionGenerator
{
    int QuestionsCount { get; }

    Task SetUpQuestionsAsync(CancellationToken cancellationToken);

    Task SetUpQuestionsAsync(HashSet<int> subcategoriesIds, CancellationToken cancellationToken);

    Question GetNextQuestion();

    Task<IEnumerable<Category>> GetCategoriesAsync(CancellationToken cancellationToken);
}
