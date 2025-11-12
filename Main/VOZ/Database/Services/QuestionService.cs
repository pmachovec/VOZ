using VOZ.Database.Entities;

namespace VOZ.Database.Services;

internal class QuestionService(VozDbContext _vozDbContext) : IQuestionService
{
    private int _pointer;

    private readonly Question[] _questions = Random.Shared.GetItems(
        _vozDbContext.Questions.ToArray(),
        _vozDbContext.Questions.Count()
    );

    // This is correct, '_pointer++' returns the initial value before the increment.
    public Question? GetNextQuestion() => _pointer >= _questions.Length ? null : _questions[_pointer++];

    // This is correct, '_pointer--' returns the initial value before the decrement.
    public Question? GetPreviousQuestion() => _pointer <= 0 ? null : _questions[_pointer--];
}
