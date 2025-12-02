using VOZ.QuestionGenerator.Entities;

namespace VOZ.QuestionGenerator;

public interface IQuestionGenerator
{
    Question GetNextQuestion();

    Task SetUpQuestionsAsync(CancellationToken cancellationToken);
}

