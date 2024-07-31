using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

using HarmonyLib;

using PrincessRTFM.CowtasticCafeEasyMode.Logging;

using UnityEngine;

namespace PrincessRTFM.CowtasticCafeEasyMode.Patches;

[HarmonyPatch(typeof(OrderManager))]
internal class Orders {
	private const BindingFlags AnyInstance = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

	[HotkeyTrigger("Instafill order", KeyCode.Space)]
	[SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "delegate conformation")]
	internal static void FillCupWithOrder(KeyCode trigger = KeyCode.None) {
		OrderManager manager = OrderManager.instance;
		if (manager is null) {
			Log.Error($"Cannot autofill order, OrderManager instance is null");
			return;
		}
		CupController cup = CupController.instance;
		if (cup is null) {
			Log.Error($"Cannot autofill order, CupController instance is null");
			return;
		}
		Customers order = manager.ActiveCustomer;
		if (order is null) {
			Log.Error($"Cannot autofill order, active customer is null");
			return;
		}
		List<float> fills = manager.ActiveIngreedentPercentages;
		if (fills is null) {
			Log.Error($"Cannot autofill order, active order ingredient percentage list is null");
			return;
		}
		foreach (Toppings want in order.Toppings) {
			switch (want) {
				case Toppings.Ice:
					cup.Ice = true;
					break;
				case Toppings.Boba:
					cup.Boba = true;
					break;
				case Toppings.WhipedCream:
					cup.WhippedCream = true;
					break;
				case Toppings.CaramelSauce:
					cup.CaramelSauce = true;
					break;
				case Toppings.ChocolateSauce:
					cup.ChocolateSauce = true;
					break;
				case Toppings.Sprinkles:
					cup.Sprinkles = true;
					break;
				default:
					try {
						Log.Warn($"Unrecognised topping {want}, attempting automatic reflection");
						FieldInfo field = cup.GetType().GetField(want.ToString(), AnyInstance);
						field?.SetValue(cup, true);
					}
					catch {
						Log.Error($"Automatic reflection failed, {cup.GetType().Name}.{want} is not a valid boolean instance field");
					}
					break;
			}
		}
		// The list of desired filling AMOUNTS is semi-sparse and starts with chocolate, milk, tea, cream, espresso, sugar, and coffee
		// Index 13 (the fourtheenth element) is the amount of breast milk they want
		cup.Chocolate = Mathf.Clamp01(fills[0] / 100);
		cup.Milk = Mathf.Clamp01(fills[1] / 100);
		cup.Tea = Mathf.Clamp01(fills[2] / 100);
		cup.Cream = Mathf.Clamp01(fills[3] / 100);
		cup.Espresso = Mathf.Clamp01(fills[4] / 100);
		cup.Sugar = Mathf.Clamp01(fills[5] / 100);
		cup.Coffee = Mathf.Clamp01(fills[6] / 100);
		cup.BreastMilk = Mathf.Clamp01(fills[13] / 100);
		cup.Fullness = 1f;
		Log.Debug($"Filled cup with {cup.Chocolate:F2}chocolate, {cup.Milk:F2}milk, {cup.Tea:F2}tea, {cup.Cream:F2}cream, {cup.Espresso:F2}coffee, {cup.Sugar:F2}sugar, {cup.BreastMilk:F2}breastmilk");
	}

	[HarmonyPostfix]
	[HarmonyPatch(nameof(OrderManager.StartOrder))]
	public static void AutomaticallyFillCupWithOrder() {
		if (!Config.AutoFillCup)
			return;

		FillCupWithOrder();
	}

	[HarmonyPostfix]
	[HarmonyPatch(nameof(OrderManager.CheckIfOrderIsValid))]
	[SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "parameter names must conform to harmony specification")]
	public static void GuaranteeValidOrder(ref float __result) {
		if (!Config.IgnoreCustomerOrder)
			return;

		Log.Debug($"Overriding oringinal order rating of {__result:F4}");
		__result = 1f;
	}
}
