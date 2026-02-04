using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using VOZ.Shared.Resources.Translations;

namespace VOZ.Shared.Components.Layout;

public class MainLayoutBase : LayoutComponentBase
{
    [Inject]
    protected IStringLocalizer<VOZTranslations> Localizer { get; set; } = default!;
}
