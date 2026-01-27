using Microsoft.EntityFrameworkCore;
using VOZ.Database;
using VOZ.Database.Entities;

namespace VOZ.Generator;

internal class QuestionGenerator(VOZDbContext _vozDbContext)
{
    private Question[]? _questions;
    private int _questionCounter;

    /// <summary>
    /// Total count of available questions after setup.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// Throw when setup has not been performed.
    /// </exception>
    public int QuestionsCount =>
        _questions?.Length ?? throw new InvalidOperationException("Questions no set up!");

    /// <summary>
    /// Sets up the generator with all questions from the database.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token for asynchronous operation.</param>
    /// <returns>Empty task for the asynchronous operation.</returns>
    public async Task SetUpQuestionsAsync(CancellationToken cancellationToken)
    {
        _questions = await _vozDbContext
            .Questions
            .Include(question => question.Answers)
            .Include(question => question.QuestionImage)
            .ToArrayAsync(cancellationToken);

        _questionCounter = _questions.Length;
    }

    /// <summary>
    /// Sets up the generator with questions from subcategories specified by the parameter.
    /// </summary>
    /// <param name="subcategoriesIds">Non-empty set of subcategories IDs.</param>
    /// <param name="cancellationToken">Cancellation token for asynchronous operation.</param>
    /// <returns>Empty task for the asynchronous operation.</returns>
    /// <exception cref="ArgumentException">
    /// Throw when the subcategoriesIds set is empty.
    /// </exception>
    public async Task SetUpQuestionsAsync(HashSet<int> subcategoriesIds, CancellationToken cancellationToken)
    {
        if (subcategoriesIds.Count == 0)
        {
            throw new ArgumentException("Empty subcategories IDs!");
        }

        _questions = await _vozDbContext
            .Questions
            .Where(question => subcategoriesIds.Contains(question.Subcategory.Id))
            .Include(question => question.Answers)
            .Include(question => question.QuestionImage)
            .ToArrayAsync(cancellationToken);

        _questionCounter = _questions.Length;
    }

    /// <summary>
    /// Returns next random question from the set up generator.
    /// </summary>
    /// <returns>Next random question from the set up generator.</returns>
    /// <exception cref="InvalidOperationException">
    /// Throw when setup has not been performed or when there are no more questions left.
    /// It's the responsibility of the consumer to watch the number of questions.
    /// </exception>
    public Question GetNextQuestion()
    {
        if (_questions is null)
        {
            throw new InvalidOperationException("Questions no set up!");
        }

        if (_questionCounter <= 0)
        {
            throw new InvalidOperationException("No more questions available!");
        }

        // Get random index of the remaining part of the questions array and then decrement the counter.
        // This is correct, '_questionCounter--' returns the initial value before the decrement.
        var randomIndex = Random.Shared.Next(_questionCounter--);

        // Get the question at the random index position.
        var question = _questions[randomIndex];

        // Swap the question at the end of the remaining part with the one selected by the random index.
        // For the swap, the question counter must be already decremented.
        _questions[randomIndex] = _questions[_questionCounter];

        // Randomly shuffle answers of the question,
        var questionAnswers = question.Answers.ToArray();
        Random.Shared.Shuffle(questionAnswers);
        question.Answers = questionAnswers;

        return question;
    }

    /// <summary>
    /// Returns available categories of questions, which have at least one subcategory.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token for the asynchronous operation.</param>
    /// <returns>Task with available categories of questions.</returns>
    /// <exception cref="InvalidDataException">
    /// Throw when no categories with subcategories are available in the database.
    /// </exception>
    public async Task<IEnumerable<Category>> GetCategoriesWithSubcategoriesAsync(CancellationToken cancellationToken)
    {
        if (!_vozDbContext.Categories.Any())
        {
            throw new InvalidDataException("No categories available in the database!");
        }

        var categoriesWithSubcategories = await _vozDbContext
            .Categories
            .Where(category => category.Subcategories.Count > 0)
            .Include(category => category.Subcategories)
            .ToArrayAsync(cancellationToken);

        return categoriesWithSubcategories.Length == 0
            ? throw new InvalidDataException("No categories with subcategories available in the database!")
            : categoriesWithSubcategories;
    }
}
