namespace PrincessRTFM.CowtasticCafeEasyMode;

using System;

using UnityEngine;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
internal class HotkeyTriggerAttribute: Attribute {
	public string Label { get; }
	public KeyCode[] Keybinds { get; }
	public HotkeyTriggerAttribute(string label, params KeyCode[] keys) {
		this.Label = label;
		this.Keybinds = keys;
	}
	public override string ToString() => $"[{this.Keybinds[0]}] {this.Label}";
}
