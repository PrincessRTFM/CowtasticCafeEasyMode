using System.Diagnostics.CodeAnalysis;

using HarmonyLib;

namespace PrincessRTFM.CowtasticCafeEasyMode.Patches;

[HarmonyPatch]
internal static class AlwaysActivePatches {

	[HarmonyPrefix]
	[HarmonyPatch(typeof(BaseGameMode), nameof(BaseGameMode.BuyMilkFullness))]
	[SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "parameter names must conform to harmony specification")]
	public static void AlwaysAnimateSurprised(ref bool DoSUpriseGrowthAnimation) => DoSUpriseGrowthAnimation = true;

}
