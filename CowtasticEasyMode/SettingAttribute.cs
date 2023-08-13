namespace PrincessRTFM.CowtasticCafeEasyMode;

using System;

using UnityEngine;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
internal class SettingAttribute: Attribute {
	public string Key { get; }
	public bool Default { get; }
	public KeyCode[] ToggleKeys { get; }
	public SettingAttribute(string name, bool initial, params KeyCode[] hotkeys) {
		this.Key = name;
		this.Default = initial;
		this.ToggleKeys = hotkeys;
	}
}
