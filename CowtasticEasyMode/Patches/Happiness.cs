using System.Diagnostics.CodeAnalysis;

using HarmonyLib;

using UnityEngine;

namespace PrincessRTFM.CowtasticCafeEasyMode.Patches;

[HarmonyPatch]
internal static class Happiness {
	public const float MinimumThreshold = 5;

	private static void setHappinessProperty(object thing, float? happiness) {
		if (thing is StatsManager sm)
			sm.Happiness = happiness ?? Mathf.Max(sm.Happiness, MinimumThreshold);
		else if (thing is BaseGameMode bgm)
			bgm.Happiness = happiness ?? Mathf.Max(bgm.Happiness, MinimumThreshold);
	}
	[HarmonyPrefix]
	[HarmonyPatch(typeof(StatsManager), nameof(StatsManager.UpdateGui))]
	[HarmonyPatch(typeof(BaseGameMode), "Update")]
	[HarmonyPatch(typeof(BaseGameMode), "UpdateOtherComponents")]
	[SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "parameter names must conform to harmony specification")]
	public static void ForceHappiness(object __instance) {
		if (Config.LockHappinessAtCap)
			setHappinessProperty(__instance, 100);
		else if (Config.PreventGameLoss)
			setHappinessProperty(__instance, null);
	}

	[HarmonyPrefix]
	[HarmonyPatch(typeof(BaseGameMode), nameof(BaseGameMode.ChangeHappyness))]
	[SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "parameter names must conform to harmony specification")]
	public static void OverrideHappinessLoss(BaseGameMode __instance, ref float amount) {
		if (Config.LockHappinessAtCap) {
			amount = 100;
		}
		else if (Config.PreventGameLoss) {
			amount = __instance.Happiness + amount < MinimumThreshold
				? MinimumThreshold - __instance.Happiness
				: amount;
		}
	}
}
