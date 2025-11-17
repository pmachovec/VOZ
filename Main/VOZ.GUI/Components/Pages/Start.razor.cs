using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using VOZ.GUI.Resources.Translations;

namespace VOZ.GUI.Components.Pages;

public class StartBase : ComponentBase
{
    [Inject]
    protected IStringLocalizer<VOZTranslations> Localizer { get; set; } = default!;
}
