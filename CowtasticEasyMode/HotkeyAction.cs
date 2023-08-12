namespace PrincessRTFM.CowtasticCafeEasyMode;

using System;
using System.Reflection;

using MonoMod.Utils;

using UnityEngine;

internal class HotkeyAction {
	public string Label { get; }
	public KeyCode[] Keybinds { get; }
	public Action Method { get; }

	public string Descriptor { get; }

	public HotkeyAction(string label, Action method, params KeyCode[] keybinds) {
		this.Label = label;
		this.Keybinds = keybinds;
		this.Method = method;
		this.Descriptor = method.Method.DeclaringType.Name + "." + method.Method.Name;
	}
	public HotkeyAction(MethodBase method) {
		HotkeyTriggerAttribute attr = method.GetCustomAttribute<HotkeyTriggerAttribute>() ?? throw new ArgumentException("provided method does not have a HotkeyTrigger attribute", nameof(method));
		this.Label = attr.Label;
		this.Keybinds = attr.Keybinds;
		this.Method = method.CreateDelegate<Action>();
		this.Descriptor = method.DeclaringType.Name + "." + method.Name;
	}
	public HotkeyAction(Action function) {
		MethodInfo method = function.Method
			?? throw new ArgumentException("cannot identify method represented by provided Action", nameof(function), new NullReferenceException("$Action.Method returned null"));
		HotkeyTriggerAttribute attr = method.GetCustomAttribute<HotkeyTriggerAttribute>() ?? throw new ArgumentException("provided method does not have a HotkeyTrigger attribute", nameof(method));
		this.Label = attr.Label;
		this.Keybinds = attr.Keybinds;
		this.Method = method.CreateDelegate<Action>();
		this.Descriptor = method.DeclaringType.Name + "." + method.Name;
	}

	public void Invoke() => this.Method();

	public override string ToString() => $"[{this.Keybinds[0]}] {this.Label}";
	public static implicit operator string(HotkeyAction action) => action.ToString();

	public static implicit operator Action(HotkeyAction action) => action.Method;
	public static implicit operator HotkeyAction(Action action) => new(action);
}
