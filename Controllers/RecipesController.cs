using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecipeAPI.Models;
using RecipeAPI.Service;

namespace RecipeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecipesController : ControllerBase
    {
        //db context
        private readonly RecipedbContext _context;
        //logger
        static readonly log4net.ILog _log4net = log4net.LogManager.GetLogger(typeof(RecipesController));
        //Service Class
        RecipeManager rmanager = new RecipeManager();

        public RecipesController(RecipedbContext context)
        {
            _context = context;
        }


        // GET: api/Recipes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Recipe>>> GetRecipes()
        {
            _log4net.Info("Get Recipes invoked.");
            return await _context.Recipes.ToListAsync();

        }

        //DELETE: api/Recipes/5 (for admin)

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {

            var r = await _context.Recipes.FindAsync(id);
            if (r == null)
            {
                return NotFound();
            }

            _context.Recipes.Remove(r);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/Recipes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Recipe>> GetRecipe(int id)
        {
            var recipe = await _context.Recipes.FindAsync(id);
            _log4net.Info("Get Recipes invoked.");

            if (recipe == null)
            {
                return NotFound();
            }

            return recipe;
        }

        //Admin adding recipe
        // POST: api/Recipes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("add/")]
        public async Task<ActionResult<Recipe>> PostRecipe(Recipe recipe)
        {
            _context.Recipes.Add(recipe);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRecipe", new { id = recipe.Rid }, recipe);
        }

        //Delete recipe from user table
        // DELETE: api/Recipes/delete/6
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteRecipe(int id)
        {

            var q = rmanager.GetUsrRecipeByID(id);
            _context.MyRecipes.Remove(q);
            _context.SaveChanges();

            return Ok();
        }

        
        //Find recipe by ingredient
        // GET: api/Recipes/5
        [HttpGet("item/{ingre}")]
        public dynamic FindRecipe(string ingre)
        {

            _log4net.Info("Searching Recipe with ingredient " + ingre);
            return rmanager.SearchforRecipe(ingre);
        }

        //Find recipe by cuisine
        [HttpGet("items/{cuisine}")]
        public dynamic FindRecipebycuisine(string cuisine)
        {

            _log4net.Info("Searching Recipe with ingredient " + cuisine);
            return rmanager.SearchforRecipebycuisine(cuisine);
        }

        //Display User Recipes
        [HttpGet("/myrecipe")]
        public dynamic MyRecipes()
        {
            _log4net.Info("Getting user's recipes.");
            return rmanager.DisplayUserRecipe();
        }

        //Add recipe to user
        [HttpPost("/myrecipe/{id}")]
        public void MyRecipe(int id, Recipe r)
        {
            if (!rmanager.RecipeExists(id))
            {
                rmanager.AddRecipeToUser(id);
            }
            _log4net.Info("Saved a recipe to user's table.");
        }

    }
}
