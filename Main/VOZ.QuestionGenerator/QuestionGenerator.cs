using Microsoft.EntityFrameworkCore;
using VOZ.QuestionGenerator.Database;
using VOZ.QuestionGenerator.Entities;

namespace VOZ.QuestionGenerator;

internal class QuestionGenerator(QuestionGeneratorDbContext _questionGeneratorDbContext) : IQuestionGenerator
{
    private Question[]? _questions;
    private int _questionCounter;

    public int QuestionsCount =>
        _questions?.Length ?? throw new InvalidOperationException("Questions no set up!");

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
        if (subcategoriesIds.Count == 0)
        {
            throw new ArgumentException("Empty subcategories IDs!");
        }

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

    public async Task<IEnumerable<Category>> GetCategoriesWithSubcategoriesAsync(CancellationToken cancellationToken)
    {
        if (!_questionGeneratorDbContext.Categories.Any())
        {
            throw new InvalidDataException("No categories available in the database!");
        }

        var categoriesWithSubcategories = await _questionGeneratorDbContext
            .Categories
            .Where(category => category.Subcategories.Count > 0)
            .Include(category => category.Subcategories)
            .ToArrayAsync(cancellationToken);

        return categoriesWithSubcategories.Length == 0
            ? throw new InvalidDataException("No categories with subcategories available in the database!")
            : categoriesWithSubcategories;
    }
}
