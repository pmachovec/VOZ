using Microsoft.EntityFrameworkCore;
using VOZ.QuestionGenerator.Database;
using VOZ.QuestionGenerator.Entities;

namespace VOZ.QuestionGenerator;

internal class QuestionGenerator(QuestionGeneratorDbContext _questionGeneratorDbContext) : IQuestionGenerator
{
    private Question[]? _questions;

    public int QuestionCounter { get; private set; }

    public Question? GetNextQuestion()
    {
        if (_questions is null)
        {
            throw new InvalidOperationException("Questions no set up!");
        }

        // This is correct, 'QuestionCounter++' returns the initial value before the increment.
        return QuestionCounter >= _questions.Length ? null : _questions[QuestionCounter++];
    }

    public Question? GetPreviousQuestion()
    {
        if (_questions is null)
        {
            throw new InvalidOperationException("Questions no set up!");
        }

        // This is correct, 'QuestionCounter--' returns the initial value before the decrement.
        return QuestionCounter <= 0 ? null : _questions[QuestionCounter--];
    }

    public async Task SetUpQuestionsAsync(CancellationToken cancellationToken)
    {
        QuestionCounter = 0;

        var questionsArray = await _questionGeneratorDbContext
            .Questions
            .Include(question => question.Answers)
            .Include(question => question.QuestionImage)
            .ToArrayAsync(cancellationToken);

        Random.Shared.Shuffle(questionsArray);
        _questions = questionsArray;
    }
}
