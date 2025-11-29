namespace VOZ.GUI.Components.Pages.QuestionnaireComponents;

public interface IAnswerButton
{
    event EventHandler AnswerEvent;

    void ReactToAnswer(object? _1, EventArgs _2);
}
