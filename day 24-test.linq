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
	
	var combatants = new[] {
	new Combatant { Name = GetName().First(), Count = 17, HitPoints = 5390, Weaknesses = new[] { "radiation", "bludgeoning" }, Attack = 4507, AttackType = "fire",Initiative = 2, Type = "defender" },
	new Combatant { Name = GetName().First(),Count = 989, HitPoints = 1274, Immunities = new[] { "fire" }, Weaknesses = new[] { "slashing", "bludgeoning" }, Attack = 25, AttackType = "slashing", Initiative = 3, Type = "defender" },
	new Combatant { Name = GetName().First(),Count = 801, HitPoints = 4706, Weaknesses = new[] { "radiation" }, Attack = 116, AttackType = "bludgeoning", Initiative = 1, Type = "attacker" },
	new Combatant { Name = GetName().First(),Count = 4485, HitPoints = 2961, Immunities = new[] { "radiation" }, Weaknesses = new[] { "fire", "cold" }, Attack = 12, AttackType = "slashing", Initiative = 4, Type = "attacker" }
	};
	foreach (var combatant in combatants)
	{
		Console.WriteLine(combatant.Describe);
	}
	while (combatants.Any(d => d.Type == "defender" && d.Count > 0) && combatants.Any(a => a.Type == "attacker" && a.Count > 0))
	{
		var targets = new Dictionary<Combatant,Combatant>();
		foreach (var combatant in combatants.Where(c => c.Count > 0).OrderByDescending(c => c.EffectivePower).ThenByDescending(c=>c.Initiative)) {
			var target = combatants.Where(c => c.Count > 0).Where(c => c != combatant && !targets.Values.Any(v => v == c) && c.Type != combatant.Type)
			.OrderByDescending(c => EstimateDamage(combatant,c)).ThenByDescending(c => c.EffectivePower).ThenByDescending(c => c.Initiative).FirstOrDefault();
			if (target == null) continue;
			if (EstimateDamage(combatant, target) == 0) continue;
			targets[combatant] = target;
		}
		foreach (var key in targets.Keys.OrderByDescending(k => k.Initiative)) {
			if (key.HitPoints <= 0) continue;
			var damage = EstimateDamage(key, targets[key]);
			Console.WriteLine($"At initiative {key.Initiative}, {key.Type} {key.Name} attacks {targets[key].Name} for {damage}, killing {damage / targets[key].HitPoints}. {targets[key].Count - damage / targets[key].HitPoints} remain.");
			targets[key].Count -= damage/targets[key].HitPoints;
		}
	}
	Console.WriteLine("The battle ends");
	var winners = combatants.Where(c => c.Count > 0).Sum(c => c.Count).Dump();
}

public int EstimateDamage(Combatant attacker, Combatant defender ) {
	if (defender.Immunities.Contains(attacker.AttackType)) return 0;
	if (defender.Weaknesses.Contains(attacker.AttackType)) return attacker.EffectivePower*2;
	return attacker.EffectivePower;
}

private string[] Names = new[] { "Laptop_Soda","Puppy_Trees","Settings_Cat","Monster_Bird","Dog_Ice_cream_cone","Website_Clock","Android_Running","Body_System","Printer_Boat","Printer_Male","Android_Allergies","System_Laptop","Solar_Ring","Cone_Trees","Toilet_Mail","Ring_Ring","Leash_Crab","Crab_Flowers","Shoes_Breakfast","BBQ_Floppy_Disk","Male_Comics","Ice_cream_Whale","Ice_cream_Toilet","Leash_Fence","Towel_Trees","Flowers_Ring","Floppy_Disk_Solar","Cone_System","Poop_Crab","Rollers_Prints","Kitty_Ice_cream","Allergies_Poop","Floppy_Disk_Soap","Settings_Breakfast","Crab_Shoe","Solar_Plants","Trees_Nuclear","Comics_Running","Running_Dog","Towel_Dislike","Poop_Laptop","Printer_Male","Rollers_Comics","Laptop_Body","Whale_Rollers","Rollers_Website","Monster_Shoe","BBQ_Toilet","Clock_Robot","Flowers_Urine","BBQ_Flowers","Soap_Male","Shoe_Android","Nuclear_Book","Hnads_Trees","Hnads_Dislike","Male_Male","Sink_Fence","Floppy_Disk_Plants","Settings_Solar","Nuclear_Shower","Allergies_Plus","Puppy_Ice_cream_cone","Flowers_Android","Monster_Light_saber","Floppy_Disk_System","Water_Prints","Dislike_Ice_cream_cone","Sink_Horse","Poop_Monster","Video_games_Printer","Printer_Book","Shower_Ice_cream_cone","Android_Book","Website_Fusion","Body_Printer","YouTube_Ice_cream_cone","Sink_Cat","Floppy_Disk_Towel","Shelf_Shelf","Leash_Body","Soap_Websites","Ring_Hnads","Kitty_Hnads","Nuclear_Clock","Plants_YouTube","Leash_Shelf","Leash_Elevator","Cat_Nuclear","Android_Plants","Crab_Prints","Ice_cream_cone_Horse","Book_Ring","Puppy_Whale","Soda_Rollers","Running_Nuclear","Crab_Website","Whale_Android","Ice_cream_Shoes","Whale_Flowers" };
int NameCtr = 0;
public IEnumerable<string> GetName() {
	yield return Names[NameCtr++];
}
// Define other methods and classes here