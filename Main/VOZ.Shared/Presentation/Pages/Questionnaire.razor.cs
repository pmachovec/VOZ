using Microsoft.AspNetCore.Components;

namespace VOZ.Shared.Presentation.Pages;

public class QuestionnaireBase : ComponentBase
{
    [Inject]
    private QuestionnaireParams QuestionnaireParams { get; set; } = default!;

    protected bool IsLoading { get; private set; } = true;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await QuestionnaireParams.SetUpQuestionsTask;
            IsLoading = false;
            StateHasChanged();
        }
    }
}
