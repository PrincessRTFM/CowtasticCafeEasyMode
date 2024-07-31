using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using PrincessRTFM.CowtasticCafeEasyMode.Logging;

using UnityEngine;

namespace PrincessRTFM.CowtasticCafeEasyMode.EventWatchers;

public class HotkeyManager: MonoBehaviour {
	internal static readonly Dictionary<KeyCode, HashSet<HotkeyAction>> triggerKeys = [];
	internal static string[] TriggerInstructionLines { get; private set; } = [];

	public static HotkeyManager Instance { get; private set; } = null!;
	public void Awake() {
		MethodInfo[] triggers = this
			.GetType()
			.Assembly
			.GetTypes()
			.SelectMany(t => t.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
			.Where(HotkeyAction.IsValid)
			.ToArray();
		Log.Debug($"Registering {triggers.Length} hotkey-bound action trigger{(triggers.Length == 1 ? "" : "s")}");
		foreach (MethodInfo method in triggers) {
			HotkeyTriggerAttribute attr = method.GetCustomAttribute<HotkeyTriggerAttribute>()!;
			KeyCode[] keys = attr.Keybinds;
			foreach (KeyCode key in keys) {
				if (!triggerKeys.ContainsKey(key))
					triggerKeys.Add(key, []);
				HotkeyAction action = new(method);
				triggerKeys[key].Add(action);
				Log.Debug($"- {action}: {action.Descriptor}()");
			}
		}
		TriggerInstructionLines = triggerKeys
			.SelectMany(pair => pair.Value
				.OrderBy(action => action.Label)
				.Select(action => action.ToString())
			)
			.ToArray();

		Instance = this;
	}

	public void Update() {
		if (!StateGuard.Ready)
			return;

		bool changed = false;
		foreach (KeyValuePair<PropertyInfo, SettingAttribute> setting in Config.settings) {
			if (setting.Value.ToggleKeys.Any(Input.GetKeyDown)) {
				PropertyInfo prop = setting.Key;
				bool value = !(bool)prop.GetValue(null, null);
				prop.SetValue(null, value, null);
				Log.Debug($"{setting.Value.Key} = {value.ToString().ToLower()}");
				changed = true;
			}
		}
		if (changed)
			Config.Save();

		foreach (KeyValuePair<KeyCode, HashSet<HotkeyAction>> bindings in triggerKeys) {
			if (Input.GetKeyDown(bindings.Key)) {
				foreach (HotkeyAction func in bindings.Value) {
					Log.Debug($"Invoking [{func.Label}] via {func.Descriptor}()");
					func.Invoke(bindings.Key);
				}
			}
		}
	}
}
