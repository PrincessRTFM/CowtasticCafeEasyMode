namespace PrincessRTFM.CowtasticCafeEasyMode;

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

using Kajabity.Tools.Java;

using PrincessRTFM.CowtasticCafeEasyMode.Logging;

using UnityEngine;

internal static class Config {
	[Setting("happiness.lockAtCap", false, KeyCode.L, KeyCode.H)]
	public static bool LockHappinessAtCap { get; set; }

	[Setting("happiness.preventGameLoss", true, KeyCode.P, KeyCode.H)]
	public static bool PreventGameLoss { get; set; }

	[Setting("money.earnDouble", true, KeyCode.E, KeyCode.I, KeyCode.M)]
	public static bool IncomeTimes2 { get; set; }

	[Setting("money.earnQuintuple", false, KeyCode.D, KeyCode.I, KeyCode.M)]
	public static bool IncomeTimes5 { get; set; }

	[Setting("money.spendingHalf", true, KeyCode.R, KeyCode.O, KeyCode.M)]
	public static bool HalveAllCosts { get; set; }

	[Setting("money.spendingOff", false, KeyCode.N, KeyCode.O, KeyCode.M)]
	public static bool NoSpending { get; set; }

	[Setting("overlay.visible", true, KeyCode.Slash, KeyCode.Question)]
	public static bool ShowOverlay { get; set; }

	[Setting("autopass.fillCup", false, KeyCode.F)]
	public static bool AutoFillCup { get; set; }

	[Setting("autopass.skipCheck", false, KeyCode.G)]
	public static bool IgnoreCustomerOrder { get; set; }

	#region Implementation details
	private static readonly JavaProperties conf = new();
	internal static readonly Dictionary<PropertyInfo, SettingAttribute> settings = new();
	static Config() {
		PropertyInfo[] properties = typeof(Config)
			.GetProperties()
			.Where(p => p.PropertyType == typeof(bool) && p.GetCustomAttribute<SettingAttribute>() is not null)
			.ToArray();
		foreach (PropertyInfo prop in properties) {
			SettingAttribute meta = prop.GetCustomAttribute<SettingAttribute>()!;
			settings[prop] = meta;
			prop.SetValue(null, meta.Default, null);
		}
	}
	public static void Load() {
		Log.Info($"Reloading configuration from {Core.ConfigPath}");
		using FileStream configStream = new(Core.ConfigPath, FileMode.OpenOrCreate, FileAccess.Read, FileShare.Read);
		conf.Load(configStream);
		Update();
	}
	public static void Update() {
		Log.Debug($"Updating {settings.Count} setting{(settings.Count == 1 ? "" : "s")}");
		foreach (KeyValuePair<PropertyInfo, SettingAttribute> pair in settings) {
			PropertyInfo prop = pair.Key;
			SettingAttribute meta = pair.Value;
			string stored = conf.GetProperty(meta.Key, meta.Default.ToString().ToLower());
			if (!bool.TryParse(stored, out bool value))
				value = meta.Default;
			prop.SetValue(null, value, null);
			Log.Debug($"- {meta.Key} = {value.ToString().ToLower()}");
		}
	}
	public static void Save() {
		Log.Info($"Writing configuration to {Core.ConfigPath}");
		foreach (KeyValuePair<PropertyInfo, SettingAttribute> pair in settings) {
			PropertyInfo prop = pair.Key;
			SettingAttribute meta = pair.Value;
			conf.SetProperty(meta.Key, prop.GetValue(null, null).ToString().ToLower());
		}
		using FileStream configStream = new(Core.ConfigPath, FileMode.Create, FileAccess.Write, FileShare.None);
		conf.Store(configStream, false);
	}
	public static void Sync() {
		Load();
		Save();
	}
	#endregion
}
