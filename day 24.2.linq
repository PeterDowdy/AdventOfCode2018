<Query Kind="Program" />

public class Combatant {
	public string Name {get; set;}
	public int Count {get; set;}
	public int HitPoints {get; set;}
	public string[] Immunities {get; set;} =new string[0];
	public string[] Weaknesses {get; set;} =new string[0];
	public int Attack {get; set;}
	public string AttackType {get; set;} = "";
	public int Initiative {get; set;}
	public int EffectivePower => Count * Attack;
	public string Type { get; set; } = "";
	public string Describe => $"{Type} {Name}: {Count} units, HP: {HitPoints}, Attack: {Attack} {AttackType}, Initiative: {Initiative}, Immunities: {string.Join(",", (Immunities.Any() ? Immunities : new[] { "None" }))}, Weaknesses: {string.Join(",", (Weaknesses.Any() ? Weaknesses : new[] { "None" }))}";
}

void Main()
{
	var lastBattleVictorious = false;
	var boost = 1000;
	for (; ; )
	{
		NameCtr = 0;
		var combatants = new[] {
	new Combatant { Name = GetName().First(), Count = 3115, HitPoints =1585,Weaknesses = new[] { "slashing", "bludgeoning" }, Attack =4+boost, AttackType = "slashing", Initiative =7, Type = "defender" },
	new Combatant { Name = GetName().First(), Count = 3866, HitPoints = 6411,Weaknesses = new[] { "cold", "radiation"}, Immunities = new[] { "fire" }, Attack = 14+boost, AttackType = "slashing", Initiative = 11, Type = "defender" },
	new Combatant { Name = GetName().First(), Count = 40, HitPoints = 10471, Weaknesses = new[] { "bludgeoning", "slashing"}, Immunities = new[] { "cold" }, Attack = 2223+boost, AttackType = "cold", Initiative = 3, Type = "defender" },
	new Combatant { Name = GetName().First(), Count = 1923, HitPoints = 2231, Weaknesses = new[] { "slashing", "fire" }, Attack = 10+boost, AttackType = "bludgeoning", Initiative = 13, Type = "defender" },
	new Combatant { Name = GetName().First(), Count = 4033, HitPoints = 10164, Immunities = new[] { "slashing" }, Attack = 22+boost, AttackType = "radiation", Initiative = 5, Type = "defender" },
	new Combatant { Name = GetName().First(), Count = 36, HitPoints = 5938, Weaknesses = new[] { "bludgeoning", "cold"}, Immunities = new[] { "fire" }, Attack = 1589+boost, AttackType = "slashing", Initiative = 4, Type = "defender" },
	new Combatant { Name = GetName().First(), Count = 2814, HitPoints = 7671, Weaknesses = new[] { "cold" }, Attack = 21+boost, AttackType = "radiation", Initiative = 15, Type = "defender" },
	new Combatant { Name = GetName().First(), Count = 217, HitPoints = 9312, Immunities = new[] { "slashing" }, Attack = 345+boost, AttackType = "radiation", Initiative = 8, Type = "defender" },
	new Combatant { Name = GetName().First(), Count = 38, HitPoints = 7686, Weaknesses = new[] { "bludgeoning" }, Attack = 1464+boost, AttackType = "radiation", Initiative = 14, Type = "defender" },
	new Combatant { Name = GetName().First(), Count = 5552, HitPoints = 3756, Weaknesses = new[] { "slashing" }, Attack = 6+boost, AttackType = "fire", Initiative = 10, Type = "defender" },
	new Combatant { Name = GetName().First(), Count = 263, HitPoints = 28458, Weaknesses = new[] { "fire", "radiation" }, Attack = 186, AttackType = "cold", Initiative = 9, Type = "attacker" },
	new Combatant { Name = GetName().First(), Count = 137, HitPoints = 29425, Immunities = new[] { "fire" }, Weaknesses = new[] { "cold" }, Attack = 367, AttackType = "radiation", Initiative = 1, Type = "attacker" },
	new Combatant { Name = GetName().First(), Count = 2374, HitPoints = 41150, Immunities = new[] { "bludgeoning", "slashing", "radiation" }, Weaknesses = new[] { "cold" }, Attack = 34, AttackType = "bludgeoning", Initiative = 6, Type = "attacker" },
	new Combatant { Name = GetName().First(), Count = 1287, HitPoints = 24213, Immunities = new[] { "fire" }, Attack = 36, AttackType = "cold", Initiative = 17, Type = "attacker" },
	new Combatant { Name = GetName().First(), Count = 43, HitPoints = 32463, Weaknesses = new[] { "radiation"}, Immunities = new[] { "slashing", "bludgeoning" }, Attack = 1347, AttackType = "fire", Initiative = 16, Type = "attacker" },
	new Combatant { Name = GetName().First(), Count = 140, HitPoints = 51919, Weaknesses = new[] { "slashing", "bludgeoning" }, Attack = 633, AttackType = "fire", Initiative = 12, Type = "attacker" },
	new Combatant { Name = GetName().First(), Count = 3814, HitPoints = 33403,Attack = 15, AttackType = "fire", Initiative = 19, Type = "attacker" },
	new Combatant { Name = GetName().First(), Count = 3470, HitPoints = 44599, Weaknesses = new[] { "slashing", "radiation" }, Attack = 23, AttackType = "radiation", Initiative = 18, Type = "attacker" },
	new Combatant { Name = GetName().First(), Count = 394, HitPoints = 36279,Attack = 164, AttackType = "fire", Initiative = 20, Type = "attacker" },
	new Combatant { Name = GetName().First(), Count = 4288, HitPoints = 20026,Attack = 7, AttackType = "radiation", Initiative = 2, Type = "attacker" },
	};
		/*foreach (var combatant in combatants)
		{
			Console.WriteLine(combatant.Describe);
		}*/
		Console.WriteLine("Fight");
		while (combatants.Any(d => d.Type == "defender" && d.Count > 0) && combatants.Any(a => a.Type == "attacker" && a.Count > 0))
		{
			//Console.WriteLine("Fight!");
			var targets = new Dictionary<Combatant, Combatant>();
			foreach (var combatant in combatants.Where(c => c.Count > 0).OrderByDescending(c => c.EffectivePower).ThenByDescending(c => c.Initiative))
			{
				var target = combatants.Where(c => c.Count > 0).Where(c => c != combatant && !targets.Values.Any(v => v == c) && c.Type != combatant.Type)
				.OrderByDescending(c => EstimateDamage(combatant, c)).ThenByDescending(c => c.EffectivePower).ThenByDescending(c => c.Initiative).FirstOrDefault();
				if (target == null) continue;
				if (EstimateDamage(combatant, target) == 0) continue;
				targets[combatant] = target;
			}
			var anyDamageDone = false;
			foreach (var key in targets.Keys.OrderByDescending(k => k.Initiative))
			{
				if (key.HitPoints <= 0) continue;
				var damage = EstimateDamage(key, targets[key]);
				//Console.WriteLine($"At initiative {key.Initiative}, {key.Type} {key.Name} attacks {targets[key].Name} for {damage}, killing {damage / targets[key].HitPoints}. {targets[key].Count - damage / targets[key].HitPoints} remain.");
				if (damage / targets[key].HitPoints > 0)
				{
					targets[key].Count -= damage / targets[key].HitPoints;
					anyDamageDone = true;
				}
			}
			if (!anyDamageDone) break;
		}
		Console.WriteLine($"The battle ends with boost {boost}");
		if (combatants.Where(c => c.Count > 0).Any(d => d.Type == "defender") && combatants.Where(c => c.Count > 0).All(d => d.Type != "attacker")) {
			var winners = combatants.Where(c => c.Count > 0).Sum(c => c.Count).Dump();
			lastBattleVictorious = true;
		} else if (lastBattleVictorious == true) {
			Console.WriteLine("First loss");
			break;
		}
		if (lastBattleVictorious) boost--;
		else boost +=1000;
	}
}

public int EstimateDamage(Combatant attacker, Combatant defender)
{
	if (defender.Immunities.Contains(attacker.AttackType)) return 0;
	if (defender.Weaknesses.Contains(attacker.AttackType)) return attacker.EffectivePower * 2;
	return attacker.EffectivePower;
}

private string[] Names = new[] { "Laptop_Soda","Puppy_Trees","Settings_Cat","Monster_Bird","Dog_Ice_cream_cone","Website_Clock","Android_Running","Body_System","Printer_Boat","Printer_Male","Android_Allergies","System_Laptop","Solar_Ring","Cone_Trees","Toilet_Mail","Ring_Ring","Leash_Crab","Crab_Flowers","Shoes_Breakfast","BBQ_Floppy_Disk","Male_Comics","Ice_cream_Whale","Ice_cream_Toilet","Leash_Fence","Towel_Trees","Flowers_Ring","Floppy_Disk_Solar","Cone_System","Poop_Crab","Rollers_Prints","Kitty_Ice_cream","Allergies_Poop","Floppy_Disk_Soap","Settings_Breakfast","Crab_Shoe","Solar_Plants","Trees_Nuclear","Comics_Running","Running_Dog","Towel_Dislike","Poop_Laptop","Printer_Male","Rollers_Comics","Laptop_Body","Whale_Rollers","Rollers_Website","Monster_Shoe","BBQ_Toilet","Clock_Robot","Flowers_Urine","BBQ_Flowers","Soap_Male","Shoe_Android","Nuclear_Book","Hnads_Trees","Hnads_Dislike","Male_Male","Sink_Fence","Floppy_Disk_Plants","Settings_Solar","Nuclear_Shower","Allergies_Plus","Puppy_Ice_cream_cone","Flowers_Android","Monster_Light_saber","Floppy_Disk_System","Water_Prints","Dislike_Ice_cream_cone","Sink_Horse","Poop_Monster","Video_games_Printer","Printer_Book","Shower_Ice_cream_cone","Android_Book","Website_Fusion","Body_Printer","YouTube_Ice_cream_cone","Sink_Cat","Floppy_Disk_Towel","Shelf_Shelf","Leash_Body","Soap_Websites","Ring_Hnads","Kitty_Hnads","Nuclear_Clock","Plants_YouTube","Leash_Shelf","Leash_Elevator","Cat_Nuclear","Android_Plants","Crab_Prints","Ice_cream_cone_Horse","Book_Ring","Puppy_Whale","Soda_Rollers","Running_Nuclear","Crab_Website","Whale_Android","Ice_cream_Shoes","Whale_Flowers" };
int NameCtr = 0;
public IEnumerable<string> GetName() {
	yield return Names[NameCtr++];
}
// Define other methods and classes here