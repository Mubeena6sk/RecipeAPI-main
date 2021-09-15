using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RecipeAPI.Models;

namespace RecipeAPI.Service
{
    public interface IRecipe
    { 
        dynamic SearchforRecipe(string ingredient);
        dynamic DisplayUserRecipe();
        void AddRecipeToUser(int id);
        bool RecipeExists(int id);
        dynamic GetUsrRecipeByID(int id);
    }
}
