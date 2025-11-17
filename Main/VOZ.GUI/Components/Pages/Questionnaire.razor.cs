using Microsoft.AspNetCore.Components;
using VOZ.QuestionGenerator;

namespace VOZ.GUI.Components.Pages;

public class QuestionnaireBase : ComponentBase
{
    [Inject]
    protected IQuestionGenerator QuestionGenerator { get; set; } = default!;
}
