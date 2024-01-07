namespace PrincessRTFM.CowtasticCafeEasyMode.Patches;

using System.Diagnostics.CodeAnalysis;

using HarmonyLib;

using PrincessRTFM.CowtasticCafeEasyMode.Logging;

using UnityEngine;

[HarmonyPatch]
[SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "delegate conformation")]
internal class Money {
	[HotkeyTrigger("Remove $10", KeyCode.PageDown)]
	public static void RemoveMoney10(KeyCode trigger) {
		float amount = Mathf.Min(10, BaseGameMode.instance.Money);
		BaseGameMode.instance.Money -= amount;
		Log.Info($"Removed ${amount} (now at ${BaseGameMode.instance.Money})");
	}
	[HotkeyTrigger("Remove $100", KeyCode.End)]
	public static void RemoveMoney100(KeyCode trigger) {
		float amount = Mathf.Min(100, BaseGameMode.instance.Money);
		BaseGameMode.instance.Money -= amount;
		Log.Info($"Removed ${amount} (now at ${BaseGameMode.instance.Money})");
	}
	[HotkeyTrigger("Remove $1000", KeyCode.Delete)]
	public static void RemoveMoney1000(KeyCode trigger) {
		float amount = Mathf.Min(1000, BaseGameMode.instance.Money);
		BaseGameMode.instance.Money -= amount;
		Log.Info($"Removed ${amount} (now at ${BaseGameMode.instance.Money})");
	}

	[HotkeyTrigger("Add $10", KeyCode.PageUp)]
	public static void AddMoney10(KeyCode trigger) {
		BaseGameMode.instance.Money += 10;
		Log.Info($"Added $10 (now at ${BaseGameMode.instance.Money})");
	}
	[HotkeyTrigger("Add $100", KeyCode.Home)]
	public static void AddMoney100(KeyCode trigger) {
		BaseGameMode.instance.Money += 100;
		Log.Info($"Added $100 (now at ${BaseGameMode.instance.Money})");
	}
	[HotkeyTrigger("Add $1000", KeyCode.Insert)]
	public static void AddMoney1000(KeyCode trigger) {
		BaseGameMode.instance.Money += 1000;
		Log.Info($"Added $1000 (now at ${BaseGameMode.instance.Money})");
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
