using Microsoft.EntityFrameworkCore;
using VOZ.QuestionGenerator.Database;
using VOZ.QuestionGenerator.Entities;

namespace VOZ.QuestionGenerator;

internal class QuestionGenerator(QuestionGeneratorDbContext _questionGeneratorDbContext) : IQuestionGenerator
{
    private Question[]? _questions;
    private int _questionCounter;

    public int QuestionsCount =>
        _questions == null ? throw new InvalidOperationException("Questions no set up!") : _questions.Length;

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

    public async Task SetUpQuestionsAsync(HashSet<int> subcategoriesIds, CancellationToken cancellationToken)
    {
        _questionCounter = 0;

        var questionsArray = await _questionGeneratorDbContext
            .Questions
            .Where(question => subcategoriesIds.Contains(question.Subcategory.Id))
            .Include(question => question.Answers)
            .Include(question => question.QuestionImage)
            .ToArrayAsync(cancellationToken);

        Random.Shared.Shuffle(questionsArray);
        _questions = questionsArray;
    }

    public Question GetNextQuestion()
    {
        if (_questions is null)
        {
            throw new InvalidOperationException("Questions no set up!");
        }

        if (_questionCounter >= QuestionsCount)
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

    public async Task<IEnumerable<Category>> GetCategoriesAsync(CancellationToken cancellationToken) =>
        await _questionGeneratorDbContext
            .Categories
            .Include(category => category.Subcategories)
            .ToArrayAsync(cancellationToken);
}
