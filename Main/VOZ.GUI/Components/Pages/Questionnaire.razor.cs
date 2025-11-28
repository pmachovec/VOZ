using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using VOZ.GUI.Constants;
using VOZ.GUI.Entities;
using VOZ.GUI.Resources.Translations;
using VOZ.QuestionGenerator;
using VOZ.QuestionGenerator.Entities;

namespace VOZ.GUI.Components.Pages;

public class QuestionnaireBase : ComponentBase
{
    private Question? _nextQuestion;
    private List<AnsweredQuestion> _answeredQuestions = [];

    [Inject]
    protected IStringLocalizer<VOZTranslations> Localizer { get; set; } = default!;

    [Inject]
    protected IQuestionGenerator QuestionGenerator { get; set; } = default!;

    protected Answer[] Answers = [];

    public string ButtonCorrect = CssClasses.BTN_LIGHT;

    protected string ButtonDisabled = string.Empty;

    public string ButtonWrong = CssClasses.BTN_LIGHT;

    protected bool IsLoading = true;

    protected QuestionImage? PotentialImage;

    protected string Text = string.Empty;

    protected string Verdict = string.Empty;

    protected string VerdictClass = string.Empty;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await QuestionGenerator.SetUpQuestionsAsync(CancellationToken.None);
            IsLoading = false;
            SetUpNextQuestion();
            StateHasChanged();
        }
    }

    protected void SubmitAnswer(Answer answer)
    {
        if (_nextQuestion is null)
        {
            return;
        }

        var answeredQuestion = new AnsweredQuestion(_nextQuestion, answer);
        _answeredQuestions.Add(answeredQuestion);

        if (answer.IsCorrect)
        {
            Verdict = $"{Localizer[VOZTranslations.Nice]}!!!";
            VerdictClass = CssClasses.TEXT_SUCCESS;
        }
        else
        {
            Verdict = $"{Localizer[VOZTranslations.Badly]}!!!";
            VerdictClass = CssClasses.TEXT_DANGER;
            ButtonWrong = CssClasses.BTN_DANGER;
        }

        ButtonCorrect = CssClasses.BTN_SUCCESS;
        ButtonDisabled = CssClasses.DISABLED;
        StateHasChanged();
    }

    // Designed for PNG images.
    // If different mime types are present in the DB, other specific implementations must be created.
    protected static (int Width, int Height)? GetImageDimensions(byte[] bytes)
    {
        if (bytes.Length < 24)
        {
            return null;
        }

        var width = (bytes[16] << 24) | (bytes[17] << 16) | (bytes[18] << 8) | bytes[19];
        var height = (bytes[20] << 24) | (bytes[21] << 16) | (bytes[22] << 8) | bytes[23];

        return (width, height);
    }

    private void SetUpNextQuestion()
    {
        if (QuestionGenerator.GetNextQuestion() is not { } nextQuestion)
        {
            return;
        }

        _nextQuestion = nextQuestion;
        var shuffledAnswers = nextQuestion.Answers.ToArray();
        Random.Shared.Shuffle(shuffledAnswers);
        _nextQuestion.Answers = shuffledAnswers;
        Text = _nextQuestion.Text;
        PotentialImage = _nextQuestion.QuestionImage;
        Answers = shuffledAnswers;
    }
}
