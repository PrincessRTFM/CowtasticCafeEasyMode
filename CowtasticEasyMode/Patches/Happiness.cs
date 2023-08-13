namespace PrincessRTFM.CowtasticCafeEasyMode.Patches;

using HarmonyLib;

using PrincessRTFM.CowtasticCafeEasyMode;

using UnityEngine;

[HarmonyPatch]
internal static class Happiness {
	public const float MinimumThreshold = 5;

	// XXX implement Config.LockHappinessAtCap and Config.KeepHappinessAboveThreshold separately
	[HarmonyPrefix]
	[HarmonyPatch(typeof(StatsManager), nameof(StatsManager.UpdateGui))]
	[HarmonyPatch(typeof(BaseGameMode), "Update")]
	[HarmonyPatch(typeof(BaseGameMode), "UpdateOtherComponents")]
	public static void ForceHappiness(object __instance) {
		if (!Config.LockHappinessAtCap && !Config.PreventGameLoss)
			return;

		switch (__instance) {
			case StatsManager sm:
				sm.Happiness = Config.LockHappinessAtCap
					? 100
					: Mathf.Max(sm.Happiness, MinimumThreshold);
				break;
			case BaseGameMode bgm:
				bgm.Happiness = Config.LockHappinessAtCap
					? 100
					: Mathf.Max(bgm.Happiness, MinimumThreshold);
				break;
		}
	}

	[HarmonyPrefix]
	[HarmonyPatch(typeof(BaseGameMode), nameof(BaseGameMode.ChangeHappyness))]
	public static void OverrideHappinessLoss(BaseGameMode __instance, ref float amount) {
		if (Config.LockHappinessAtCap)
			amount = Mathf.Abs(amount);
		else if (Config.PreventGameLoss)
			amount = __instance.Happiness + amount < MinimumThreshold ? MinimumThreshold - __instance.Happiness : amount;
	}
}
