<Query Kind="Program" />

public bool Empty((string,int) worker) {
	return worker.Item1 == null;
}
void Main()
{
	var input = File.ReadAllText($"{Path.GetDirectoryName(Util.CurrentQueryPath)}/7.txt");
	var steps = input.Replace("\r", "").Split('\n').Select(i => i.Substring(5, 1)).ToList();
	steps.AddRange(input.Replace("\r", "").Split('\n').Select(i => i.Substring(36, 1)));
	steps = steps.Distinct().ToList();
	var stepChains = input.Replace("\r", "").Split('\n').Select(i => (Step: i.Substring(36, 1), Prereq: i.Substring(5, 1)));
	var stepRequirements = steps.ToDictionary(s => s, s => stepChains.Where(sc => sc.Step == s).Select(sc => sc.Prereq).ToList());
	var chain = new List<string>();
	var workers = new (string, int)[5];
	var elapsedSeconds = 0;
	do
	{
		elapsedSeconds++;
		while (workers.Any(Empty))
		{
			var emptyWorker = Array.IndexOf(workers, workers.FirstOrDefault(Empty));
			var orphans = stepRequirements.Where(s => !s.Value.Any() || s.Value.All(s2 => chain.Any(c => c == s2))).ToList();
			var batmen = orphans.OrderBy(o => o.Key).Where(k => !workers.Any(w => w.Item1 == k.Key)).ToList();
			var batman = batmen.FirstOrDefault();
			if (batman.Key == null) break;
			if (workers.All(w => w.Item1 != batman.Key))
			{
				workers[emptyWorker] = (batman.Key, (int)batman.Key[0] - 4);
			}
		}
		for (var i = 0; i < 5; i++)
		{
			workers[i].Item2--;
			if (workers[i].Item2 == 0)
			{
				chain.Add(workers[i].Item1);
				stepRequirements.Remove(workers[i].Item1);
				stepRequirements = stepRequirements.ToDictionary(s => s.Key, s => s.Value.Where(v => v != workers[i].Item1).ToList());
				workers[i].Item1 = null;
			}
		}
	} while (stepRequirements.Any());
	elapsedSeconds.Dump();
}