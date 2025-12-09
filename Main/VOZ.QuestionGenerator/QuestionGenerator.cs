using Microsoft.EntityFrameworkCore;
using VOZ.QuestionGenerator.Database;
using VOZ.QuestionGenerator.Entities;

namespace VOZ.QuestionGenerator;

internal class QuestionGenerator(QuestionGeneratorDbContext _questionGeneratorDbContext) : IQuestionGenerator
{
    private Question[]? _questions;
    private int _questionCounter;

    public Question GetNextQuestion()
    {
        if (_questions is null)
        {
            throw new InvalidOperationException("Questions no set up!");
        }

        if (_questionCounter >= _questions.Length)
        {
            throw new InvalidOperationException("No more questions available!");
        }

        // This is correct, '_questionCounter++' returns the initial value before the increment.
        var question = _questions[_questionCounter++];
        var questionAnswers = question.Answers.ToArray();
        Random.Shared.Shuffle(questionAnswers);
        question.Answers = questionAnswers;
        return question;
    }

    public async Task SetUpQuestionsAsync(CancellationToken cancellationToken)
    {
        _questionCounter = 0;

        var questionsArray = await _questionGeneratorDbContext
            .Questions
            .Include(question => question.Answers)
            .Include(question => question.QuestionImage)
            .ToArrayAsync(cancellationToken);

        Random.Shared.Shuffle(questionsArray);
        _questions = questionsArray;
    }
}
