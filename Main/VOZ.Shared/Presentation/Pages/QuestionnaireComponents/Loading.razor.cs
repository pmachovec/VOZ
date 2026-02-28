using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using VOZ.Shared.Resources.Translations;

namespace VOZ.Shared.Presentation.Pages.QuestionnaireComponents;

public class LoadingBase : ComponentBase
{
    [Inject]
    protected IStringLocalizer<VOZTranslations> Localizer { get; set; } = default!;
}
