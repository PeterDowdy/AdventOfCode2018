<Query Kind="Program" />

void Main()
{
	var recipeCount = 760221;
	var recipes = new List<int> { 3,7};
	var elfOnePosition = 0;
	var elfTwoPosition = 1;
	
	while (recipes.Count < recipeCount) {
		var nextRecipe = recipes[elfOnePosition] + recipes[elfTwoPosition];
		var nextRecipes = nextRecipe > 9 ? new[] { nextRecipe / 10, nextRecipe % 10 } : new[] { nextRecipe % 10 };
		recipes.AddRange(nextRecipes);
		elfOnePosition = (elfOnePosition + recipes[elfOnePosition] + 1) % recipes.Count;
		elfTwoPosition = (elfTwoPosition + recipes[elfTwoPosition] + 1) % recipes.Count;
	}
	var lastTenRecipes = string.Join("",recipes.Skip(recipeCount).Take(10)).Dump();
}

// Define other methods and classes here
