using Microsoft.EntityFrameworkCore;
using VOZ.QuestionGenerator.Database;
using VOZ.QuestionGenerator.Entities;

namespace VOZ.QuestionGenerator;

internal class QuestionGenerator(QuestionGeneratorDbContext _questionGeneratorDbContext) : IQuestionGenerator
{
    private Question[]? _questions;
    private int _pointer;

    public Question? GetNextQuestion()
    {
        if (_questions is null)
        {
            throw new InvalidOperationException("Questions no set up!");
        }

        // This is correct, '_pointer++' returns the initial value before the increment.
        return _pointer >= _questions.Length ? null : _questions[_pointer++];
    }

    public Question? GetPreviousQuestion()
    {
        if (_questions is null)
        {
            throw new InvalidOperationException("Questions no set up!");
        }

        // This is correct, '_pointer--' returns the initial value before the decrement.
        return _pointer <= 0 ? null : _questions[_pointer--];
    }

    public async Task SetUpQuestionsAsync(CancellationToken cancellationToken)
    {
        _pointer = 0;

        var questionsArray = await _questionGeneratorDbContext
            .Questions
            .ToArrayAsync(cancellationToken);

        _questions =  Random.Shared.GetItems(
            questionsArray,
            _questionGeneratorDbContext.Questions.Count()
        );
    }
}
