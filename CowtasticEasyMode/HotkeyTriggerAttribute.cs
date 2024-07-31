using System;

using UnityEngine;

namespace PrincessRTFM.CowtasticCafeEasyMode;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
internal class HotkeyTriggerAttribute(string label, params KeyCode[] keys): Attribute {
	public string Label { get; } = label;
	public KeyCode[] Keybinds { get; } = keys;

	public override string ToString() => $"[{this.Keybinds[0]}] {this.Label}";
}
