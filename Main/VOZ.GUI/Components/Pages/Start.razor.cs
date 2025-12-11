using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using VOZ.GUI.Resources.Translations;
using VOZ.QuestionGenerator;
using VOZ.QuestionGenerator.Entities;
using System.Linq;

namespace VOZ.GUI.Components.Pages;

public class StartBase : ComponentBase
{
    private readonly HashSet<int> _selectedCategoriesIds = [];
    private readonly HashSet<int> _selectedSubcategoriesIds = [];

    [Inject]
    protected IStringLocalizer<VOZTranslations> Localizer { get; set; } = default!;

    [Inject]
    private IQuestionGenerator QuestionGenerator { get; set; } = default!;

    protected IEnumerable<Category> Categories = default!;

    protected override async Task OnInitializedAsync()
    {
        Categories = await QuestionGenerator.GetCategoriesAsync(CancellationToken.None);
    }

    protected bool IsCategorySelected(int categoryId) => _selectedCategoriesIds.Contains(categoryId);

    protected bool IsSubcategorySelected(int subcategoryId) => _selectedSubcategoriesIds.Contains(subcategoryId);

    protected void OnCategoryChange(ChangeEventArgs e, Category category)
    {
        var isChecked = e.Value is true;

        if (isChecked)
        {
            _selectedCategoriesIds.Add(category.Id);

            foreach (var subcategory in category.Subcategories)
            {
                _selectedSubcategoriesIds.Add(subcategory.Id);
            }
        }
        else
        {
            _selectedCategoriesIds.Remove(category.Id);

            foreach (var subcategory in category.Subcategories)
            {
                _selectedSubcategoriesIds.Remove(subcategory.Id);
            }
        }
    }

    protected void OnSubcategoryChange(ChangeEventArgs e, Subcategory subcategory)
    {
        var isChecked = e.Value is true;

        if (isChecked)
        {
            _selectedSubcategoriesIds.Add(subcategory.Id);
        }
        else
        {
            _selectedSubcategoriesIds.Remove(subcategory.Id);
        }

        // Keep category state in sync: checked only when all its subcategories are checked
        var category = Categories.FirstOrDefault(c => c.Id == subcategory.CategoryId);

        if (category is null)
        {
            return;
        }

        var hasSubcategories = category.Subcategories.Count != 0;
        var isAllChecked = hasSubcategories && category.Subcategories.All(sc => _selectedSubcategoriesIds.Contains(sc.Id));

        if (isAllChecked)
        {
            _selectedCategoriesIds.Add(category.Id);
        }
        else
        {
            _selectedCategoriesIds.Remove(category.Id);
        }
    }
}
