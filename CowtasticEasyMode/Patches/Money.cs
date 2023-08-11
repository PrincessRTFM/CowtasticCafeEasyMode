namespace PrincessRTFM.CowtasticCafeEasyMode.Patches;

using HarmonyLib;

[HarmonyPatch]
internal class Money {
	[HarmonyPrefix]
	[HarmonyPatch(typeof(BaseGameMode), nameof(BaseGameMode.AddMoney))]
	public static void IncreasedEarnings(ref float amount) {
		if (Config.IncomeTimes2)
			amount *= 2;
		if (Config.IncomeTimes5)
			amount *= 5;
	}

	[HarmonyPrefix]
	[HarmonyPatch(typeof(BaseGameMode), nameof(BaseGameMode.SubMoney))]
	public static void ReducedSpendings(ref float amount) {
		if (Config.HalveAllCosts)
			amount /= 2;
		if (Config.NoSpending)
			amount = 0;
	}
}
