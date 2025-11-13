using VOZ.QuestionGenerator.Database;
using VOZ.QuestionGenerator.Entities;

namespace VOZ.QuestionGenerator;

internal class QuestionGenerator(QuestionGeneratorDbContext _questionGeneratorDbContext) : IQuestionGenerator
{
    private int _pointer;

    private readonly Question[] _questions = Random.Shared.GetItems(
        _questionGeneratorDbContext.Questions.ToArray(),
        _questionGeneratorDbContext.Questions.Count()
    );

    // This is correct, '_pointer++' returns the initial value before the increment.
    public Question? GetNextQuestion() => _pointer >= _questions.Length ? null : _questions[_pointer++];

    // This is correct, '_pointer--' returns the initial value before the decrement.
    public Question? GetPreviousQuestion() => _pointer <= 0 ? null : _questions[_pointer--];
}
