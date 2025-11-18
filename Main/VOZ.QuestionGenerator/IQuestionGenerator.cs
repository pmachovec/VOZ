using VOZ.QuestionGenerator.Entities;

namespace VOZ.QuestionGenerator;

public interface IQuestionGenerator
{
    int QuestionCounter { get; }

    Question? GetNextQuestion();

    Question? GetPreviousQuestion();

    Task SetUpQuestionsAsync(CancellationToken cancellationToken);
}

