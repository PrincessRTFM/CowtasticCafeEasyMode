using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;

using HarmonyLib;
using HarmonyLib.Tools;

using PrincessRTFM.CowtasticCafeEasyMode.Logging;

using UnityEngine;

namespace PrincessRTFM.CowtasticCafeEasyMode;

public static class Core {
	public static string Module { get; } = Assembly.GetExecutingAssembly().GetName().Name;
	public static string Version { get; } = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).ProductVersion;

	public static string LogPath { get; } = Path.Combine(Environment.CurrentDirectory, $"{Module}.log");
	public static string ConfigPath { get; } = Path.Combine(Environment.CurrentDirectory, $"{Module}.properties");

	internal static Harmony? Patcher { get; private set; } = null;
	public static bool Initialised => Patcher is not null;

	internal static GameObject Dispatcher { get; private set; } = null!;

	private static bool initialising = false;

	public static void Launch() => AppDomain.CurrentDomain.AssemblyLoad += onAssemblyLoad;

	public static string DescribeMethod(MethodBase method, bool includeReturn = true) {
		ParameterInfo[] allParams = method.GetParameters();
		string retval = allParams
			.Where(p => (p.Attributes & ParameterAttributes.Retval) == ParameterAttributes.Retval)
			.FirstOrDefault()
			?.ParameterType
			.Name
			?? (method as MethodInfo)?.ReturnType.Name
			?? "<unknown>";
		string[] args = allParams
			.Where(p => (p.Attributes & ParameterAttributes.Retval) == 0)
			.OrderBy(p => p.Position)
			.Select(p => p.ParameterType.Name)
			.ToArray();
		string label = $"{method.DeclaringType.Name}.{method.Name}({string.Join(", ", args)})";
		return includeReturn ? $"{retval} {label}" : label;
	}

	private static void init() {
		if (Initialised || initialising)
			return;
		initialising = true;
		try {
			initialiseLogging();
			Config.Sync();
			hookUnityEvents();
		}
		catch (Exception e) {
			Log.Fatal(e.ToString());
			return;
		}
		new Thread(installHarmonyPatches) {
			Name = $"{Module} - harmony patcher",
			IsBackground = false,
		}.Start();
	}
	private static void initialiseLogging() {
		Log.output = new StreamWriter(new FileStream(LogPath, FileMode.Create, FileAccess.Write, FileShare.Read)) {
			AutoFlush = true,
		};
		HarmonyFileLog.Writer = Log.output;
		HarmonyFileLog.Enabled = true;
		DateTime now = DateTime.Now;
		string date = $"{now.Year:D4}-{now.Month:D2}-{now.Day:D2}, running on .NET {Environment.Version}";
		Log.Info($"==== {Module} log for {date}");
	}
	private static void hookUnityEvents() {
		Log.EnterMethod();
		Log.Debug("Initialising unity GameObject for event handlers");
		Dispatcher = new(Module) {
			hideFlags = HideFlags.HideAndDontSave,
		};
		UnityEngine.Object.DontDestroyOnLoad(Dispatcher);
		Log.Info("Initialising unity event handlers");
		foreach (Type unityHandler in Assembly.GetExecutingAssembly().GetTypes().Where(t => t.IsAssignableTo(typeof(MonoBehaviour)))) {
			Log.Debug($"Registering {unityHandler.Name}");
			Dispatcher.AddComponent(unityHandler);
		}
		Log.ExitMethod();
	}
	private static void installHarmonyPatches() {
		Thread.Sleep(500); // just to be safe
		try {
			Log.EnterMethod();
			Log.Debug("Initialising Harmony instance");
			Harmony harmony = new(typeof(Core).FullName);
			Log.Info("Generating and applying patches");
			harmony.PatchAll(Assembly.GetExecutingAssembly());
			Patcher = harmony;
			MethodBase[] patches = harmony.GetPatchedMethods().ToArray();
			int count = patches.Count();
			Log.Debug($"Applied {count} patch{(count == 1 ? "" : "es")}");
			foreach (MethodBase patched in patches)
				Log.Debug($"- {DescribeMethod(patched)}");
		}
		catch (Exception e) {
			Log.Fatal(e.ToString());
		}
		finally {
			Log.ExitMethod();
		}
	}

	private static void onAssemblyLoad(object sender, AssemblyLoadEventArgs e) {
		if (e.LoadedAssembly.GetName().Name is "Game") {
			AppDomain.CurrentDomain.AssemblyLoad -= onAssemblyLoad;
			new Thread(init) {
				Name = $"{Module} - core",
				IsBackground = false,
			}.Start();
		}
	}
}
