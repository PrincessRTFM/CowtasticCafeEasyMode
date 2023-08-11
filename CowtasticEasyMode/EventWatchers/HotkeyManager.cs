namespace PrincessRTFM.CowtasticCafeEasyMode.EventWatchers;

using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using PrincessRTFM.CowtasticCafeEasyMode;

using PrincessRTFM.CowtasticCafeEasyMode.Logging;

using UnityEngine;

public class HotkeyManager: MonoBehaviour {
	public static HotkeyManager Instance { get; private set; } = null!;
	public void Awake() => Instance = this;
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
	}
}
