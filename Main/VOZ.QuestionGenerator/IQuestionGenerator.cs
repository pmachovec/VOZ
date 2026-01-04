using VOZ.QuestionGenerator.Entities;

namespace VOZ.QuestionGenerator;

public interface IQuestionGenerator
{
    /// <summary>
    /// Total count of available questions after setup.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// Throw when setup has not been performed.
    /// </exception>
    int QuestionsCount { get; }

    /// <summary>
    /// Sets up the generator with all questions from the database.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token for asynchronous operation.</param>
    /// <returns>Empty task for the asynchronous operation.</returns>
    Task SetUpQuestionsAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Sets up the generator with questions from subcategories specified by the parameter.
    /// </summary>
    /// <param name="subcategoriesIds">Non-empty set of subcategories IDs.</param>
    /// <param name="cancellationToken">Cancellation token for asynchronous operation.</param>
    /// <returns>Empty task for the asynchronous operation.</returns>
    /// <exception cref="ArgumentException">
    /// Throw when the subcategoriesIds set is empty.
    /// </exception>
    Task SetUpQuestionsAsync(HashSet<int> subcategoriesIds, CancellationToken cancellationToken);

    /// <summary>
    /// Returns next question of the set up generator.
    /// </summary>
    /// <returns>Next question of the set up generator.</returns>
    /// <exception cref="InvalidOperationException">
    /// Throw when setup has not been performed or when there are no more questions left.
    /// It's the responsibility of the consumer to watch the number of questions.
    /// </exception>
    Question GetNextQuestion();

    /// <summary>
    /// Returns available categories of questions, which have at least one subcategory.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token for the asynchronous operation.</param>
    /// <returns>Task with available categories of questions.</returns>
    /// <exception cref="InvalidDataException">
    /// Throw when no categories with subcategories are available in the database.
    /// </exception>
    Task<IEnumerable<Category>> GetCategoriesWithSubcategoriesAsync(CancellationToken cancellationToken);
}
