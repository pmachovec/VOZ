using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using VOZ.Shared.Resources.Translations;

namespace VOZ.Shared.Components.Pages;

public class NotFoundBase : ComponentBase
{
    [Inject]
    protected IStringLocalizer<VOZTranslations> Localizer { get; set; } = default!;
}
