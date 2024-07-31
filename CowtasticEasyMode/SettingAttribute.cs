using System;

using UnityEngine;

namespace PrincessRTFM.CowtasticCafeEasyMode;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
internal class SettingAttribute(string name, bool initial, params KeyCode[] hotkeys): Attribute {
	public string Key { get; } = name;
	public bool Default { get; } = initial;
	public KeyCode[] ToggleKeys { get; } = hotkeys;
}
