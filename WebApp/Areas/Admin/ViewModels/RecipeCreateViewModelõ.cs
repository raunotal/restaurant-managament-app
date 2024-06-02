using App.Domain;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Areas.Admin.ViewModels;

public class RecipeCreateViewModel
{
    public Recipe Recipe { get; set; } = default!;
    public SelectList? AppUserSelectList { get; set; }
}