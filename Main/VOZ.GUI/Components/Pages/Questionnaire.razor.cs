using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using VOZ.GUI.Resources.Translations;
using VOZ.QuestionGenerator;
using VOZ.QuestionGenerator.Entities;

namespace VOZ.GUI.Components.Pages;

public class QuestionnaireBase : ComponentBase
{
    private List<Question> answeredQuestions = [];

    [Inject]
    protected IStringLocalizer<VOZTranslations> Localizer { get; set; } = default!;

    [Inject]
    protected IQuestionGenerator QuestionGenerator { get; set; } = default!;

    protected bool IsLoading = true;

    protected string Text = string.Empty;

    protected QuestionImage? PotentialImage;

    protected Answer[] Answers = [];

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await QuestionGenerator.SetUpQuestionsAsync(CancellationToken.None);
            IsLoading = false;

            if (QuestionGenerator.GetNextQuestion() is { } nextQuestion)
            {
                // Shuffle questions order
                Answers = nextQuestion.Answers.ToArray();
                Random.Shared.Shuffle(Answers);
                nextQuestion.Answers = Answers;

                Text = nextQuestion.Text;
                PotentialImage = nextQuestion.QuestionImage;
                answeredQuestions.Add(nextQuestion);
            }

            StateHasChanged();
        }
    }
}
