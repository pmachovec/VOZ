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

    // To be invoked when a new value is assigned to the SubmittedAnswer property.
    // Answer buttons contain a method registered to this event, setting buttons' CSS styling properly.
    private event EventHandler<Answer?>? _submittedAnswerEvent;

    private Answer? SubmittedAnswer
    {
        get;
        set
        {
            field = value;
            _submittedAnswerEvent?.Invoke(this, value);
        }
    }

    [Inject]
    protected IStringLocalizer<VOZTranslations> Localizer { get; set; } = default!;

    [Inject]
    protected IQuestionGenerator QuestionGenerator { get; set; } = default!;

    protected int AnswerPointer;

    protected IEnumerable<Answer> Answers = [];

    protected bool IsLoading = true;

    protected QuestionImage? PotentialImage;

    protected string PreviousQuestionButtonDisabled = CssClasses.DISABLED;

    protected string NextQuestionButtonDisabled = CssClasses.DISABLED;

    protected int QuestionsCorrectCount;

    protected int QuestionsCorrectPercentage;

    protected int QuestionsTotalCount;

    protected int QuestionsWrongCount;

    protected int QuestionsWrongPercentage;

    protected string Text = string.Empty;

    protected string Verdict = string.Empty;

    protected string VerdictClass = string.Empty;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await QuestionGenerator.SetUpQuestionsAsync(CancellationToken.None);
            IsLoading = false;
            SetUpNewQuestion();
            StateHasChanged();
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

    protected void ClickPreviousQuestionButton()
    {
        var answer = _submittedAnswers[--AnswerPointer];
        SetUpQuestion(answer.Question);
        StateHasChanged();

        if (AnswerPointer <= 0)
        {
            PreviousQuestionButtonDisabled = CssClasses.DISABLED;
        }

        NextQuestionButtonDisabled = string.Empty;
        SubmittedAnswer = answer;

        if (answer.IsCorrect)
        {
            SetNiceVerdict();
        }
        else
        {
            SetBadlyVerdict();
        }

        StateHasChanged();
    }

    protected void ClickNextQuestionButton()
    {
        AnswerPointer++;
        PreviousQuestionButtonDisabled = string.Empty;
        NextQuestionButtonDisabled = CssClasses.DISABLED;
        SetUpNewQuestion();
        StateHasChanged();
    }

    protected void RegisterAnswerButton(AnswerButtonBase answerButtonBase) =>
        _submittedAnswerEvent += answerButtonBase.ReactToSubmittedAnswer;

    protected void ReactToSubmittedAnswer(Answer submittedAnswer)
    {
        if (submittedAnswer.IsCorrect)
        {
            SetNiceVerdict();
            QuestionsCorrectCount++;
        }
        else
        {
            SetBadlyVerdict();
            QuestionsWrongCount++;
        }

        _submittedAnswers.Add(submittedAnswer);
        SubmittedAnswer = submittedAnswer;
        NextQuestionButtonDisabled = string.Empty;
        QuestionsTotalCount++;
        QuestionsCorrectPercentage = (int)Math.Round(QuestionsCorrectCount * 100 / (float)QuestionsTotalCount, MidpointRounding.AwayFromZero);
        QuestionsWrongPercentage = 100 - QuestionsCorrectPercentage;
        StateHasChanged();
    }

    private void SetNiceVerdict()
    {
        Verdict = $"{Localizer[VOZTranslations.Nice]}!!!";
        VerdictClass = CssClasses.TEXT_SUCCESS;
    }

    private void SetBadlyVerdict()
    {
        Verdict = $"{Localizer[VOZTranslations.Badly]}!!!";
        VerdictClass = CssClasses.TEXT_DANGER;
    }

    private void SetUpNewQuestion()
    {
        var newQuestion = QuestionGenerator.GetNextQuestion();
        SetUpQuestion(newQuestion);
        SubmittedAnswer = null;
        Verdict = string.Empty;
    }

    private void SetUpQuestion(Question question)
    {
        Text = question.Text;
        PotentialImage = question.QuestionImage;
        Answers = question.Answers;
    }
}
