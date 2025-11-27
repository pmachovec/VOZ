using VOZ.QuestionGenerator.Entities;

namespace VOZ.GUI.Components.Entities;

internal sealed class AnsweredQuestion(
    Question _question,
    Answer _selectedAnswer
)
{
    public Question TheQuestion => _question;

    public Answer SelectedAnswer => _selectedAnswer;
}
