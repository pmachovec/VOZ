using VOZ.QuestionGenerator.Entities;

namespace VOZ.GUI.Components.Pages.QuestionnaireComponents;

public interface IAnswerButton
{
    event EventHandler<Answer> AnswerEvent;

    void ReactToAnswer(object? _1, EventArgs _2);

    void ReactToNextQuestion(object? _1, EventArgs _2);
}
