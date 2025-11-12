using VOZ.Database.Entities;

namespace VOZ.Database.Services;

internal interface IQuestionService
{
    Question? GetNextQuestion();

    Question? GetPreviousQuestion();
}

