using VOZ.QuestionGenerator.Entities;

namespace VOZ.QuestionGenerator;

public interface IQuestionGenerator
{
    Task SetUpQuestionsAsync(CancellationToken cancellationToken);

    Question GetNextQuestion();

    Task<IEnumerable<Category>> GetCategoriesAsync(CancellationToken cancellationToken);
}
