using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using VOZ.Components.Pages.QuestionnaireComponents;
using VOZ.Constants;
using VOZ.Database.Entities;
using VOZ.Generator;
using VOZ.Resources.Translations;

namespace VOZ.Components.Pages;

public class QuestionnaireBase : ComponentBase
{
    private readonly List<Answer> _submittedAnswers = [];

    // To be invoked when color styling should be cleared from answer buttons.
    // Answer buttons contain a method registered to this event, setting buttons' CSS styling properly.
    private event EventHandler? _setAnswerButtonsLightEvent;

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
    private QuestionGenerator QuestionGenerator { get; set; } = default!;

    [Inject]
    private QuestionnaireParams QuestionnaireParams { get; set; } = default!;

    protected int AnswerPointer { get; private set; }

    protected IEnumerable<Answer> Answers { get; private set; } = [];

    protected string Done { get; private set; } = string.Empty;

    protected bool IsLoading { get; private set; } = true;

    protected string NextQuestionButtonDisabled { get; private set; } = CssClasses.DISABLED;

    protected QuestionImage? PotentialImage { get; private set; }

    protected string PreviousQuestionButtonDisabled { get; private set; } = CssClasses.DISABLED;

    protected int QuestionsCorrectCount { get; private set; }

    protected int QuestionsCorrectPercentage { get; private set; }

    protected int QuestionsTotalCount { get; private set; }

    protected int QuestionsWrongCount { get; private set; }

    protected int QuestionsWrongPercentage { get; private set; }

    protected string Text { get; private set; } = string.Empty;

    protected string Verdict { get; private set; } = string.Empty;

    protected string VerdictClass { get; private set; } = string.Empty;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await QuestionnaireParams.SetUpQuestionsTask;
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

    protected void RegisterAnswerButton(AnswerButtonBase answerButtonBase)
    {
        _setAnswerButtonsLightEvent += answerButtonBase.SetLight;
        _newQuestionEvent += answerButtonBase.ReactToNewQuestion;
        _submittedAnswerEvent += answerButtonBase.ReactToSubmittedAnswer;
    }

    protected async Task ClickPreviousQuestionButtonAsync()
    {
        await SetAnswerButtonsLightAsync();
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

        if (AnswerPointer >= QuestionGenerator.QuestionsCount - 1 && !string.IsNullOrEmpty(Done))
        {
            // There's no next question available, keep the next question button disabled.
            // This is reachable when answering all questions and listing through history.
            NextQuestionButtonDisabled = CssClasses.DISABLED;
            SetUpAnswer(_submittedAnswers[AnswerPointer]);
        }
        else if (AnswerPointer >= _submittedAnswers.Count)
        {
            // Display not answered question, which can, but doesn't have to, be already generated.
            await SetAnswerButtonsLightAsync();

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
            _newQuestionEvent?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            // Display answered question from history.
            await SetAnswerButtonsLightAsync();
            var submittedAnswer = _submittedAnswers[AnswerPointer];
            SetUpAnswer(submittedAnswer);
            await InvokeAsync(StateHasChanged);
            _submittedAnswerEvent?.Invoke(this, submittedAnswer);
        }
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
        QuestionsTotalCount++;
        QuestionsCorrectPercentage = (int)Math.Round(QuestionsCorrectCount * 100 / (float)QuestionsTotalCount, MidpointRounding.AwayFromZero);
        QuestionsWrongPercentage = 100 - QuestionsCorrectPercentage;
        _actualQuestion = null;

        if (QuestionsTotalCount < QuestionGenerator.QuestionsCount)
        {
            NextQuestionButtonDisabled = string.Empty;
        }
        else
        {
            NextQuestionButtonDisabled = CssClasses.DISABLED;
            Done = $"{Localizer[VOZTranslations.Done]}!!!";
        }

        StateHasChanged();
    }

    private async Task SetAnswerButtonsLightAsync()
    {
        // Remove coloring from answer buttons by invoking the corresponding event.
        // The slight delay prevents button coloring to briefly appear on new buttons rendered for the new question.
        // Button styling being briefly "transferred" to new buttons is an imperfection of Blazor.
        // It can't be eliminated by any awaiting and rerendering without the explicit delay.
        _setAnswerButtonsLightEvent?.Invoke(this, EventArgs.Empty);
        await InvokeAsync(StateHasChanged);
        await Task.Delay(50);
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
