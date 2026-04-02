using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using VOZ.Shared.Resources.Translations;

namespace VOZ.Shared.Presentation.Pages;

public class NotFoundBase : ComponentBase
{
    [Inject]
    protected IStringLocalizer<VOZTranslations> Localizer { get; set; } = default!;
}
