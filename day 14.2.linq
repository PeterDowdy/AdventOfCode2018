<Query Kind="Program" />

void Main()
{
	var recipeCount = 760221;
	var recipes = new List<int> { 3,7};
	var elfOnePosition = 0;
	var elfTwoPosition = 1;
	
	var lastTen = "";
	
	while (lastTen != "760221") {
		var nextRecipe = recipes[elfOnePosition] + recipes[elfTwoPosition];
		var nextRecipes = nextRecipe > 9 ? new[] { nextRecipe / 10, nextRecipe % 10 } : new[] { nextRecipe % 10 };
		recipes.AddRange(nextRecipes);
		elfOnePosition = (elfOnePosition + recipes[elfOnePosition] + 1) % recipes.Count;
		elfTwoPosition = (elfTwoPosition + recipes[elfTwoPosition] + 1) % recipes.Count;
		lastTen += nextRecipes[0];
		if (lastTen.Length > 6) { lastTen = lastTen.Substring(lastTen.Length - 6, 6); }
		if (lastTen == "760221") {
			(recipes.Count() - 7).Dump();
			break;
		}
		if (nextRecipes.Length > 1) { lastTen += nextRecipes[1]; }
		if (lastTen.Length > 6) { lastTen = lastTen.Substring(lastTen.Length - 6, 6); }
		if (lastTen == "760221")
		{
			(recipes.Count() - 6).Dump();
			break;
		}
	}
}

// Define other methods and classes here
