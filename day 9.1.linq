<Query Kind="Program" />

public class MarbleNode
{
	public MarbleNode Prev { get; set; }
	public int Number { get; set; }
	public MarbleNode Next { get; set; }
}
public void Main()
{
	var scores = new Dictionary<int, long>();
	var players = 455;
	var curPlayer = 0;
	var marbleCount = 71223;
	var lastPlacedMarble = 0;
	var currentMarble = new MarbleNode { Number = 0 };
	currentMarble.Next = currentMarble;
	currentMarble.Prev = currentMarble;
	while (lastPlacedMarble < marbleCount)
	{
		curPlayer = ((curPlayer + 1) % players);
		lastPlacedMarble++;
		if (lastPlacedMarble % 23 == 0)
		{
			if (!scores.ContainsKey(curPlayer)) scores[curPlayer] = 0;
			scores[curPlayer] += lastPlacedMarble;
			var sevenCounterClockwise = currentMarble.Prev.Prev.Prev.Prev.Prev.Prev.Prev;
			scores[curPlayer] += sevenCounterClockwise.Number;
			currentMarble = sevenCounterClockwise.Next;
			sevenCounterClockwise.Prev.Next = sevenCounterClockwise.Next;
			sevenCounterClockwise.Next.Prev = sevenCounterClockwise.Prev;
		}
		else
		{
			var (oneClockwise, twoClockwise) = (currentMarble.Next, currentMarble.Next.Next);
			var newMarble = new MarbleNode { Number = lastPlacedMarble };
			oneClockwise.Next = newMarble;
			newMarble.Prev = oneClockwise;
			newMarble.Next = twoClockwise;
			twoClockwise.Prev = newMarble;
			currentMarble = newMarble;
		}
	}
	scores.Values.Max().Dump();
}