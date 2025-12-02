using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using VOZ.GUI.Components.Pages.QuestionnaireComponents;
using VOZ.GUI.Constants;
using VOZ.GUI.Resources.Translations;
using VOZ.QuestionGenerator;
using VOZ.QuestionGenerator.Entities;

namespace VOZ.GUI.Components.Pages;

public class QuestionnaireBase : ComponentBase
{
    private readonly List<Answer> _submittedAnswers = [];

    private event EventHandler? _correctAnswerEvent;
    private event EventHandler? _wrongAnswerEvent;
    private event EventHandler? _nextQuestionEvent;

    [Inject]
    protected IStringLocalizer<VOZTranslations> Localizer { get; set; } = default!;

    [Inject]
    protected IQuestionGenerator QuestionGenerator { get; set; } = default!;

    protected IEnumerable<Answer> Answers = [];

    protected bool IsLoading = true;

    protected QuestionImage? PotentialImage;

    protected string NextQuestionButtonDisabled = CssClasses.DISABLED;

    protected int QuestionCounter;

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
        }
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

    protected void ClickNextQuestionButton()
    {
        QuestionCounter++;
        SetUpNextQuestion();
    }

    protected void RegisterCorrectAnswerButton(IAnswerButton answerButton)
    {
        answerButton.AnswerEvent += ReactToCorrectAnswer;
        RegisterAnswerButton(answerButton);
    }

    protected void RegisterWrongAnswerButton(IAnswerButton answerButton)
    {
        answerButton.AnswerEvent += ReactToWrongAnswer;
        RegisterAnswerButton(answerButton);
    }

    private void SetUpNextQuestion()
    {
        var nextQuestion = QuestionGenerator.GetNextQuestion();
        _nextQuestionEvent?.Invoke(this, EventArgs.Empty);
        Text = nextQuestion.Text;
        PotentialImage = nextQuestion.QuestionImage;
        Answers = nextQuestion.Answers;
        Verdict = string.Empty;
        NextQuestionButtonDisabled = CssClasses.DISABLED;
        StateHasChanged();
    }

    private void RegisterAnswerButton(IAnswerButton answerButton)
    {
        _correctAnswerEvent += answerButton.ReactToAnswer;
        _wrongAnswerEvent += answerButton.ReactToAnswer;
        _nextQuestionEvent += answerButton.ReactToNextQuestion;
    }

    private void ReactToCorrectAnswer(object? _, Answer correctAnswer)
    {
        Verdict = $"{Localizer[VOZTranslations.Nice]}!!!";
        VerdictClass = CssClasses.TEXT_SUCCESS;
        _correctAnswerEvent?.Invoke(this, EventArgs.Empty);
        StateHasChanged();
        ReactToAnswer(correctAnswer);
    }

    private void ReactToWrongAnswer(object? _, Answer wrongAnswer)
    {
        Verdict = $"{Localizer[VOZTranslations.Badly]}!!!";
        VerdictClass = CssClasses.TEXT_DANGER;
        _wrongAnswerEvent?.Invoke(this, EventArgs.Empty);
        ReactToAnswer(wrongAnswer);
    }

    private void ReactToAnswer(Answer submittedAnswer)
    {
        _submittedAnswers.Add(submittedAnswer);
        NextQuestionButtonDisabled = string.Empty;
        StateHasChanged();
    }
}
