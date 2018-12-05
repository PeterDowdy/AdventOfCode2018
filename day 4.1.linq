<Query Kind="Program">
  <Reference>&lt;RuntimeDirectory&gt;\System.Net.Http.dll</Reference>
  <Namespace>System.Net.Http</Namespace>
</Query>

public enum ActionType {
	BeginShift,
	WakeUp,
	FallAsleep
}
public class ScheduleFragment {
	public ScheduleFragment(string input) {
		Date = DateTime.Parse(input.Split(']')[0].Replace("[", ""));
		if (input.Contains("Guard")) {
			GuardId = int.Parse(input.Split('#')[1].Split(' ')[0]);
			Action = ActionType.BeginShift;
		} else if (input.Contains("wakes up")) {
			Action = ActionType.WakeUp;
		} else if (input.Contains("falls asleep")) {
			Action = ActionType.FallAsleep;
		}
		else throw new Exception($"Invalid input detected: {input}");
	}
	public DateTime Date;
	public int? GuardId;
	public ActionType Action;
}
async void Main()
{
	var input = File.ReadAllText($"{Path.GetDirectoryName (Util.CurrentQueryPath)}/4.txt");

	var schedules = input.Split('\n').Where(x => !string.IsNullOrEmpty(x)).Select(x => new ScheduleFragment(x)).OrderBy(x => x.Date);
	var guardTimes = new Dictionary<int, List<(int sleepStart, int sleepEnd, int totalSleep)>>();
	int? currentGuard = null;
	int? start = null;
	ActionType? lastAction = null;
	foreach (var fragment in schedules)
	{
		if (fragment.GuardId.HasValue)
		{
			currentGuard = fragment.GuardId;
			if (!guardTimes.ContainsKey(currentGuard.Value)) guardTimes[currentGuard.Value] = new List<(int, int, int)>();
		}
		else if (fragment.Action == ActionType.WakeUp)
		{
			if (!lastAction.HasValue)
			{
				start = fragment.Date.Minute;
				lastAction = ActionType.WakeUp;
			}
			else
			{
				guardTimes[currentGuard.Value].Add((start.Value, fragment.Date.Minute, fragment.Date.Minute - start.Value));
				lastAction = null;
			}
		}
		else if (fragment.Action == ActionType.FallAsleep)
		{
			if (!lastAction.HasValue)
			{
				start = fragment.Date.Minute;
				lastAction = ActionType.FallAsleep;
			}
			else
			{
				guardTimes[currentGuard.Value].Add((start.Value, fragment.Date.Minute, fragment.Date.Minute - start.Value));
				lastAction = null;
			}
		}
	}
	var sleepiestGuard = guardTimes.OrderByDescending(kvp => kvp.Value.Sum(x => x.totalSleep)).First();
	sleepiestGuard.Dump();
	var sleepMinutes = new int[60];
	for (var i = 0; i < 60; i++) sleepMinutes[i] = 0;
	foreach (var sleepSegment in sleepiestGuard.Value)
	{
		for (var i = sleepSegment.sleepStart; i < sleepSegment.sleepEnd; i++) {
			sleepMinutes[i]++;
		}
	}
	sleepMinutes.Max().Dump();
	Array.IndexOf(sleepMinutes, sleepMinutes.Max()).Dump();
}

// Define other methods and classes here