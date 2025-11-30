using Microsoft.AspNetCore.Components;
using VOZ.GUI.Constants;
using VOZ.QuestionGenerator.Entities;

namespace VOZ.GUI.Components.Pages.QuestionnaireComponents;

public class WrongAnswerButtonBase : ComponentBase, IAnswerButton
{
    [Parameter, EditorRequired]
    public required Answer TheAnswer { get; init; }

    [Parameter, EditorRequired]
    public required Action<IAnswerButton> RegisterWrongAnswerButton { get; init; }

    protected string ButtonClass = CssClasses.BTN_LIGHT;

    protected string ButtonDisabled = string.Empty;

    public event EventHandler<Answer> AnswerEvent = default!;

    protected override void OnInitialized() => RegisterWrongAnswerButton?.Invoke(this);

    public void ReactToAnswer(object? _1, EventArgs _2) => ButtonDisabled = CssClasses.DISABLED;

    public void ReactToNextQuestion(object? _1, EventArgs _2)
    {
        ButtonClass = CssClasses.BTN_LIGHT;
        ButtonDisabled = string.Empty;
    }

    protected void SubmitAnswer()
    {
        // No need to set the button as disabled here. This is handled by ReactToAnswer.
        // The button reacts to its own answer. It's easier than trying not to invoke the event for the button.

        ButtonClass = CssClasses.BTN_DANGER;
        AnswerEvent?.Invoke(this, TheAnswer);
    }
}
