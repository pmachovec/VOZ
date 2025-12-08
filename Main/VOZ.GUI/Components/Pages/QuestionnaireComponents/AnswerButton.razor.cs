using Microsoft.AspNetCore.Components;
using VOZ.GUI.Constants;
using VOZ.QuestionGenerator.Entities;

namespace VOZ.GUI.Components.Pages.QuestionnaireComponents;

public class AnswerButtonBase : ComponentBase
{
    [Parameter, EditorRequired]
    public required Answer ButtonAnswer { get; init; }

    [Parameter, EditorRequired]
    public required Action<Answer> ParentReactToAnswer { get; init; }

    [Parameter, EditorRequired]
    public required Action<AnswerButtonBase> RegisterAnswerButton { get; init; }

    protected string ButtonClass = CssClasses.BTN_LIGHT;

    protected string ButtonDisabled = string.Empty;

    protected override void OnInitialized() => RegisterAnswerButton(this);

    public void ReactToSubmittedAnswer(object? _, Answer? submittedAnswer)
    {
        if (submittedAnswer is null)
        {
            // Displayed question is to be answered - enable the button to be clickable.
            ButtonDisabled = string.Empty;
            ButtonClass = CssClasses.BTN_LIGHT;
        }
        else
        {
            // Displayed question has been answered - disable the button and style it according to the submitted response.
            ButtonDisabled = CssClasses.DISABLED;

            if (ButtonAnswer.IsCorrect)
            {
                // This button is a correct answer button.
                // When an answer is submitted, all correct answer buttons turn green, regardless if they were clicked or not or if the answer is correct or not.
                ButtonClass = CssClasses.BTN_SUCCESS;
            }
            else if (submittedAnswer.Id == ButtonAnswer.Id)
            {
                // This button is a wrong answer button.
                // A wrong answer button turns red only if it was clicked, i.e., if its answer was submitted.
                ButtonClass = CssClasses.BTN_DANGER;
            }
        }
    }
}
