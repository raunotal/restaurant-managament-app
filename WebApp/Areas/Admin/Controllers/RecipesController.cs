using App.Contracts.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using App.DAL.EF;
using App.Domain;
using App.Domain.Identity;
using Microsoft.AspNetCore.Authorization;
using WebApp.Areas.Admin.ViewModels;

namespace WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RecipesController : Controller
    {
        private readonly IAppUnitOfWork _unitOfWork;

        public RecipesController(IAppUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: Admin/Recipes
        public async Task<IActionResult> Index()
        {
            var res = await _unitOfWork.Recipes.GetAllAsync();
            return View(res);
        }

        // GET: Admin/Recipes/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // var recipe = await _repository.FirstOrDefaultAsync(id.Value);

            var recipe = await _unitOfWork.Recipes
                .FirstOrDefaultAsync(id.Value);
            if (recipe == null)
            {
                return NotFound();
            }

            return View(recipe);
        }

        // GET: Admin/Recipes/Create
        public async Task<IActionResult> Create()
        {
            var viewModel = new RecipeCreateViewModel()
            {
                AppUserSelectList = new SelectList(await _unitOfWork.AppUsers.GetAllAsync(), nameof(AppUser.Id),
                    nameof(AppUser.Email))
            };
            return View(viewModel);
        }

        // POST: Admin/Recipes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RecipeCreateViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Recipes.Add(viewModel.Recipe);
                await _unitOfWork.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            viewModel.AppUserSelectList =
                new SelectList(await _unitOfWork.AppUsers.GetAllAsync(), nameof(AppUser.Id),
                    nameof(AppUser.Email), viewModel.Recipe.AppUserId);
            return View(viewModel);
        }

        // GET: Admin/Recipes/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipe = await _unitOfWork.Recipes.FirstOrDefaultAsync(id.Value);
            if (recipe == null)
            {
                return NotFound();
            }

            ViewData["AppUserId"] =
                new SelectList(await _unitOfWork.AppUsers.GetAllAsync(), "Id", "Id", recipe.AppUserId);
            return View(recipe);
        }

        // POST: Admin/Recipes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Recipe recipe)
        {
            if (id != recipe.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _unitOfWork.Recipes.Update(recipe);
                    await _unitOfWork.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _unitOfWork.Recipes.ExistsAsync(recipe.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction(nameof(Index));
            }

            ViewData["AppUserId"] =
                new SelectList(await _unitOfWork.AppUsers.GetAllAsync(), "Id", "Id", recipe.AppUserId);
            return View(recipe);
        }

        // GET: Admin/Recipes/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipe = await _unitOfWork.Recipes
                .FirstOrDefaultAsync(id.Value);
            if (recipe == null)
            {
                return NotFound();
            }

            return View(recipe);
        }

        // POST: Admin/Recipes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var recipe = await _unitOfWork.Recipes.FirstOrDefaultAsync(id);
            if (recipe != null)
            {
                await _unitOfWork.Recipes.RemoveAsync(id);
            }

            await _unitOfWork.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}