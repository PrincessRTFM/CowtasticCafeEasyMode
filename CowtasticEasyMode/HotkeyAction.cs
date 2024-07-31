using System;
using System.Reflection;

using MonoMod.Utils;

using UnityEngine;

namespace PrincessRTFM.CowtasticCafeEasyMode;

internal delegate void KeybindHandler(KeyCode trigger);

internal class HotkeyAction {
	public string Label { get; }
	public KeyCode[] Keybinds { get; }
	public KeybindHandler Method { get; }

	public string Descriptor { get; }

	// cached at static init for runtime performance
	private static readonly MethodInfo handler = typeof(KeybindHandler).GetMethod("Invoke");
	private static readonly Type handlerReturn = handler.ReturnType;
	private static readonly ParameterInfo[] handlerArgs = handler.GetParameters();
	public static bool IsValid(MethodInfo method) {
		if (method.ReturnType != handlerReturn)
			return false;
		if (method.GetCustomAttribute<HotkeyTriggerAttribute>() is null)
			return false;
		ParameterInfo[] args = method.GetParameters();
		if (args.Length != handlerArgs.Length)
			return false;
		for (int i = 0; i < handlerArgs.Length; ++i) {
			if (args[i].ParameterType != handlerArgs[i].ParameterType)
				return false;
		}

		return true;
	}

	public HotkeyAction(MethodInfo method) {
		HotkeyTriggerAttribute attr = method.GetCustomAttribute<HotkeyTriggerAttribute>() ?? throw new ArgumentException("provided method does not have a HotkeyTrigger attribute", nameof(method));
		this.Label = attr.Label;
		this.Keybinds = attr.Keybinds;
		this.Method = method.CreateDelegate<KeybindHandler>();
		this.Descriptor = Core.DescribeMethod(method, false);
	}
	public HotkeyAction(KeybindHandler function) {
		MethodInfo method = function.Method
			?? throw new ArgumentException("cannot identify method represented by provided Action", nameof(function), new NullReferenceException("$Action.Method returned null"));
		HotkeyTriggerAttribute attr = method.GetCustomAttribute<HotkeyTriggerAttribute>() ?? throw new ArgumentException("provided method does not have a HotkeyTrigger attribute", nameof(method));
		this.Label = attr.Label;
		this.Keybinds = attr.Keybinds;
		this.Method = function;
		this.Descriptor = Core.DescribeMethod(method, false);
	}

	public void Invoke(KeyCode trigger) => this.Method(trigger);

	public override string ToString() => $"[{this.Keybinds[0]}] {this.Label}";
	public static implicit operator string(HotkeyAction action) => action.ToString();

	public static implicit operator KeybindHandler(HotkeyAction action) => action.Method;
	public static implicit operator HotkeyAction(KeybindHandler action) => new(action);
}
