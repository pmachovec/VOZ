using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using VOZ.GUI.Resources.Translations;
using VOZ.QuestionGenerator;

namespace VOZ.GUI.Components.Pages;

public class QuestionnaireBase : ComponentBase
{
    [Inject]
    protected IStringLocalizer<VOZTranslations> Localizer { get; set; } = default!;

    [Inject]
    protected IQuestionGenerator QuestionGenerator { get; set; } = default!;

    protected string Text = string.Empty;

    protected override void OnInitialized() => Text = $"{Localizer[VOZTranslations.QuestionsLoading]}...";

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await QuestionGenerator.SetUpQuestionsAsync(CancellationToken.None);
            Text = QuestionGenerator.GetNextQuestion()?.Text ?? string.Empty;
            StateHasChanged();
        }
    }
}
