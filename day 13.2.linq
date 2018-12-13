<Query Kind="Program" />

public enum Directions {
	Left,
	Straight,
	Right
}
public class Cart {
	public Cart(char v, (int X, int Y) coordinates)
	{
		Direction = v;
		NextDirection = Directions.Left;
		Coordinates = coordinates;
	}
	public (int X, int Y) Coordinates { get; set; }
	public char Direction {get; set;}
	public Directions NextDirection {get; set;}
}
public class Track {
	public Track(char v, (int X, int Y) coordinates )
	{
		Coordinates = coordinates;
		if (v == '<' || v == '>') {
			Type = '-';
			Cart = new Cart(v,coordinates);
		}
		else if (v == '^' || v == 'v') {
			Type = '|';
			Cart = new Cart(v,coordinates);
		}
		else {
			Type = v;
		}
	}
	public (int X, int Y) Coordinates { get; set; }
	public char Type {get; set;}
	public Cart Cart {get; set;}
}
void Main()
{
	var input = File.ReadAllText($"{Path.GetDirectoryName(Util.CurrentQueryPath)}/13.txt");
  var lines = input.Replace("\r","").Split('\n');
  var trackGrid = new Track[lines.Count(),lines.First().Count()];
  var carts = new List<Cart>();
  for (var y = 0; y < trackGrid.GetLength(0); y++) {
  	for (var x = 0; x < trackGrid.GetLength(1); x++) {
		trackGrid[y, x] = new Track(lines[y][x], (x, y));
		if (trackGrid[y, x]?.Cart != null)
		{
			carts.Add(trackGrid[y, x].Cart);
		}
	}
  }
  bool oneCartRemains = false;
	do
	{

		if (carts.Count == 1)
		{
			Console.WriteLine($"Last cart at ({carts.First().Coordinates.X},{carts.First().Coordinates.Y})");
			oneCartRemains = true;
		}
		//Print(trackGrid);
		var orderedCarts = carts.OrderBy(c => c.Coordinates.Y).ThenBy(c => c.Coordinates.X).ToList();
		foreach (var cart in orderedCarts) {
		if (!carts.Contains(cart)) continue;
			var cartCell = trackGrid[cart.Coordinates.Y, cart.Coordinates.X];
			Track targetCell = null;
			switch (cart.Direction)
			{
				case '>':
				targetCell = trackGrid[cart.Coordinates.Y, cart.Coordinates.X+1];
					break;
				case 'v':
					targetCell = trackGrid[cart.Coordinates.Y+1, cart.Coordinates.X];
					break;
				case '<':
					targetCell = trackGrid[cart.Coordinates.Y, cart.Coordinates.X - 1];
					break;
				case '^':
					targetCell = trackGrid[cart.Coordinates.Y-1, cart.Coordinates.X];
					break;
			}
			if (targetCell.Cart != null) {
				carts.Remove(cartCell.Cart);
				cartCell.Cart = null;
				carts.Remove(targetCell.Cart);
				targetCell.Cart = null;
			}
			else
			{
				cartCell.Cart = null;
				targetCell.Cart = cart;
				cart.Coordinates = targetCell.Coordinates;
				switch (targetCell.Type)
				{
					case '\\':
						if (cart.Direction == '>') cart.Direction = 'v';
						else if (cart.Direction == '^') cart.Direction = '<';
						else if (cart.Direction == '<') cart.Direction = '^';
						else if (cart.Direction == 'v') cart.Direction = '>';
						break;
					case '/':
						if (cart.Direction == '>') cart.Direction = '^';
						else if (cart.Direction == '^') cart.Direction = '>';
						else if (cart.Direction == '<') cart.Direction = 'v';
						else if (cart.Direction == 'v') cart.Direction = '<';
						break;
					case '+':
						switch (cart.NextDirection)
						{
							case Directions.Left:
								switch (cart.Direction)
								{
									case '>':
										cart.Direction = '^';
										break;
									case 'v':
										cart.Direction = '>';
										break;
									case '<':
										cart.Direction = 'v';
										break;
									case '^':
										cart.Direction = '<';
										break;
								}
								cart.NextDirection = Directions.Straight;
								break;
							case Directions.Right:
								switch (cart.Direction)
								{
									case '>':
										cart.Direction = 'v';
										break;
									case 'v':
										cart.Direction = '<';
										break;
									case '<':
										cart.Direction = '^';
										break;
									case '^':
										cart.Direction = '>';
										break;
								}
								cart.NextDirection = Directions.Left;
								break;
							case Directions.Straight:
								cart.NextDirection = Directions.Right;
								break;
						}
						break;
				}
			}
		}
	} while (!oneCartRemains);
}
public void Print(Track[,] grid) {
	for (var y = 0; y < grid.GetLength(0); y++) {
		for (var x = 0; x < grid.GetLength(1); x++) {
			Console.Write(grid[y,x]?.Cart?.Direction ?? grid[y,x]?.Type ?? ' ');
		}
		Console.WriteLine();
	}
	Console.WriteLine();
}