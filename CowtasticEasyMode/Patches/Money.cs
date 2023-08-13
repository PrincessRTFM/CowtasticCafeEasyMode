namespace PrincessRTFM.CowtasticCafeEasyMode.Patches;

using System.Diagnostics.CodeAnalysis;

using HarmonyLib;

using PrincessRTFM.CowtasticCafeEasyMode.Logging;

using UnityEngine;

[HarmonyPatch]
internal class Money {
	[HotkeyTrigger("Remove $10", KeyCode.PageDown)]
	[SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "delegate conformation")]
	public static void RemoveMoney(KeyCode trigger) {
		float amount = Mathf.Min(10, BaseGameMode.instance.Money);
		BaseGameMode.instance.Money -= amount;
		Log.Info($"Removed ${amount} (now at ${BaseGameMode.instance.Money})");
	}
	[HotkeyTrigger("Add $10", KeyCode.PageUp)]
	[SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "delegate conformation")]
	public static void AddMoney(KeyCode trigger) {
		BaseGameMode.instance.Money += 10;
		Log.Info($"Added $10 (now at ${BaseGameMode.instance.Money})");
	}

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
