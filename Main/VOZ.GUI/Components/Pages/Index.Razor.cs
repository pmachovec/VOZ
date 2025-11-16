using Microsoft.AspNetCore.Components;
using VOZ.QuestionGenerator;

namespace VOZ.GUI.Components.Pages;

public class IndexBase : ComponentBase
{
    [Inject]
    protected IQuestionGenerator QuestionGenerator { get; set; } = default!;
}
