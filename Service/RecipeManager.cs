using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecipeAPI.Controllers;
using RecipeAPI.Models;

namespace RecipeAPI.Service
{
    public class RecipeManager : IRecipe
    {
        private readonly RecipedbContext _context = new RecipedbContext();

      
        public dynamic SearchforRecipe(string ingredient)
        {
            var q = (from i in _context.Recipes
                     join j in _context.IngredientsIndices on i.Rid equals j.Rid
                     join k in _context.Ingredients on j.Iid equals k.Iid
                     where k.Iname == ingredient
                     select new
                     {
                         i.Rname,
                         i.Instructions,
                         i.Rcuisine,
                         i.Rimage
                     });

            return q;
        }

        public dynamic SearchforRecipebycuisine(string cuisine)
        {
            var q = (from i in _context.Recipes
                     where i.Rcuisine == cuisine
                     select new
                     {
                         i.Rname,
                         i.Instructions,
                         i.Rcuisine,
                         i.Rimage
                     });

            return q;
        }

        public dynamic DisplayUserRecipe()
        {
            var q = (from i in _context.Recipes
                     join j in _context.MyRecipes on i.Rid equals j.Rid
                     select i);

            return q;
        }

        public void AddRecipeToUser(int id)
        {
            MyRecipe obj = new MyRecipe();
            obj.Rid = id;
            _context.MyRecipes.Add(obj);
            _context.SaveChanges();
        }

        public bool RecipeExists(int id)
        {
            return _context.MyRecipes.Any(e => e.Rid == id);
        }

        public dynamic GetUsrRecipeByID(int id)
        {

            var q = (from r in _context.MyRecipes
                     where r.Rid == id
                     select r).FirstOrDefault();

            return q;
        }
    }
}
