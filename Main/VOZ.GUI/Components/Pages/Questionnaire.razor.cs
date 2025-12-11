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

    // To be invoked when a new question is displayed.
    // Answer buttons contain a method registered to this event, setting buttons' CSS styling properly.
    private event EventHandler? _newQuestionEvent;

    // To be invoked when an already answered question is displayed.
    // Answer buttons contain a method registered to this event, setting buttons' CSS styling properly.
    private event EventHandler<Answer>? _submittedAnswerEvent;

    private Question? _actualQuestion;

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

    protected async Task ClickPreviousQuestionButtonAsync()
    {
        var submittedAnswer = _submittedAnswers[--AnswerPointer];
        SetUpAnswer(submittedAnswer);

        if (AnswerPointer <= 0)
        {
            PreviousQuestionButtonDisabled = CssClasses.DISABLED;
        }

        NextQuestionButtonDisabled = string.Empty;

        // The questionnaire component must be refreshed before firing the submitted answer event so that answer buttons contain answers corresponding to the actual question.
        // StateHasChanged maliciously runs the component refresh asynchronously, causing the event to be actually fired before the questionnaire is refreshed.
        // This causes that button answers correspond to the previous question when the event is fired and correct and wrong answers are not detected correctly.
        // As a solution, the StateHasChanged must be wrapped to InvokeAsync and awaited.
        await InvokeAsync(StateHasChanged);

        // Only at this point, answer buttons contain answers corresponding to the actual question.
        // Now it's safe to fire the submitted answer event and set buttons' CSS classes properly.
        _submittedAnswerEvent?.Invoke(this, submittedAnswer);
    }

    protected async Task ClickNextQuestionButtonAsync()
    {
        AnswerPointer++;
        PreviousQuestionButtonDisabled = string.Empty;

        if (AnswerPointer >= _submittedAnswers.Count)
        {
            // Display not answered question, which can, but doesn't have to, be already generated.
            if (_actualQuestion is not null)
            {
                // Not answered question has been already generated.
                SetUpQuestion(_actualQuestion);
            }
            else
            {
                // Not answered question has not been generated yet.
                SetUpNewQuestion();
            }

            SetNoVerdict();
            NextQuestionButtonDisabled = CssClasses.DISABLED;
            await InvokeAsync(StateHasChanged);
            _newQuestionEvent?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            // Display answered question from history.
            var submittedAnswer = _submittedAnswers[AnswerPointer];
            SetUpAnswer(submittedAnswer);
            await InvokeAsync(StateHasChanged);
            _submittedAnswerEvent?.Invoke(this, submittedAnswer);
        }
    }

    protected void RegisterAnswerButton(AnswerButtonBase answerButtonBase)
    {
        _newQuestionEvent += answerButtonBase.ReactToNewQuestion;
        _submittedAnswerEvent += answerButtonBase.ReactToSubmittedAnswer;
    }


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
        _submittedAnswerEvent?.Invoke(this, submittedAnswer);
        NextQuestionButtonDisabled = string.Empty;
        QuestionsTotalCount++;
        QuestionsCorrectPercentage = (int)Math.Round(QuestionsCorrectCount * 100 / (float)QuestionsTotalCount, MidpointRounding.AwayFromZero);
        QuestionsWrongPercentage = 100 - QuestionsCorrectPercentage;
        _actualQuestion = null;
        StateHasChanged();
    }

    private void SetUpNewQuestion()
    {
        _actualQuestion = QuestionGenerator.GetNextQuestion();
        SetUpQuestion(_actualQuestion);
    }

    private void SetUpAnswer(Answer answer)
    {
        SetUpQuestion(answer.Question);

        if (answer.IsCorrect)
        {
            SetNiceVerdict();
        }
        else
        {
            SetBadlyVerdict();
        }
    }

    private void SetUpQuestion(Question question)
    {
        Text = question.Text;
        PotentialImage = question.QuestionImage;
        Answers = question.Answers;
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

    private void SetNoVerdict()
    {
        Verdict = string.Empty;
        VerdictClass = string.Empty;
    }
}
