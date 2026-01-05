using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using VOZ.Constants;
using VOZ.Database.Entities;
using VOZ.Generator;
using VOZ.Resources.Translations;

namespace VOZ.Components.Pages;

public class StartBase : ComponentBase
{
    private HashSet<int> _selectedCategoriesIds = default!;
    private HashSet<int> _selectedSubcategoriesIds = default!;

    [Inject]
    protected IStringLocalizer<VOZTranslations> Localizer { get; set; } = default!;

    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    [Inject]
    private QuestionGenerator QuestionGenerator { get; set; } = default!;

    [Inject]
    private QuestionnaireParams QuestionnaireParams { get; set; } = default!;

    protected Category[] Categories { get; private set; } = default!;

    protected string StartButtonDisabled { get; private set; } = string.Empty;

    protected override async Task OnInitializedAsync()
    {
        Categories = [.. await QuestionGenerator.GetCategoriesWithSubcategoriesAsync(CancellationToken.None)];
        _selectedCategoriesIds = [.. Categories.Select(c => c.Id)];
        _selectedSubcategoriesIds = [.. Categories.SelectMany(c => c.Subcategories).Select(sc => sc.Id)];
        DisableStartButtonWhenNoSubcategoriesSelected();
    }

    protected bool IsCategorySelected(int categoryId) => _selectedCategoriesIds.Contains(categoryId);

    protected bool IsSubcategorySelected(int subcategoryId) => _selectedSubcategoriesIds.Contains(subcategoryId);

    protected void OnCategoryChange(ChangeEventArgs e, Category category)
    {
        if (e.Value is true)
        {
            _ = _selectedCategoriesIds.Add(category.Id);

            foreach (var subcategory in category.Subcategories)
            {
                _ = _selectedSubcategoriesIds.Add(subcategory.Id);
            }
        }
        else
        {
            _ = _selectedCategoriesIds.Remove(category.Id);

            foreach (var subcategory in category.Subcategories)
            {
                _ = _selectedSubcategoriesIds.Remove(subcategory.Id);
            }
        }

        DisableStartButtonWhenNoSubcategoriesSelected();
    }

    protected void OnSubcategoryChange(ChangeEventArgs e, Subcategory subcategory)
    {
        var isChecked = e.Value is true;
        _ = isChecked ? _selectedSubcategoriesIds.Add(subcategory.Id) : _selectedSubcategoriesIds.Remove(subcategory.Id);

        // Keep category state in sync - checked only when all its subcategories are checked
        var category = Categories.FirstOrDefault(c => c.Id == subcategory.CategoryId);

        if (category is null)
        {
            return;
        }

        var hasSubcategories = category.Subcategories.Count != 0;
        var isAllChecked = hasSubcategories && category.Subcategories.All(sc => _selectedSubcategoriesIds.Contains(sc.Id));
        _ = isAllChecked ? _selectedCategoriesIds.Add(category.Id) : _selectedCategoriesIds.Remove(category.Id);
        DisableStartButtonWhenNoSubcategoriesSelected();
    }

    protected void Start()
    {
        // Comparing categories lengths is enough to determine if all categories are selected.
        QuestionnaireParams.SetUpQuestionsTask = _selectedCategoriesIds.Count == Categories.Length
            ? QuestionGenerator.SetUpQuestionsAsync(CancellationToken.None)
            : QuestionGenerator.SetUpQuestionsAsync(_selectedSubcategoriesIds, CancellationToken.None);

        NavigationManager.NavigateTo("/questionnaire");
    }

    private void DisableStartButtonWhenNoSubcategoriesSelected() =>
        StartButtonDisabled = _selectedSubcategoriesIds.Count == 0 ? CssClasses.DISABLED : string.Empty;
}
