namespace PrincessRTFM.CowtasticCafeEasyMode.EventWatchers;

using System;
using System.Linq;

using PrincessRTFM.CowtasticCafeEasyMode;
using PrincessRTFM.CowtasticCafeEasyMode.Logging;

using UnityEngine;

internal class GuiManager: MonoBehaviour {
	public const int
		OverlayWidth = 300,
		OverlayBottomEdge = 125,
		HeaderFontSize = 20,
		BodyFontSize = 14;

	public static Vector2 ScreenSize => new(Screen.width, Screen.height);

	private static GUIStyle headerStyle = null!;
	private static GUIStyle bodyStyle = null!;
	private static GUIContent headerContent = null!;

	private static string[] actionKeybinds = new string[0];
	private static GUIContent bodyContent => new(string.Join("\n", Config.settings
		.Where(pair => pair.Value.ToggleKeys.Length > 0)
		.OrderBy(pair => pair.Value.Key)
		.Select(pair => {
			string key = "[" + pair.Value.ToggleKeys[0] + "]";
			string name = pair.Key.Name;
			string state = ((bool)pair.Key.GetValue(null, null)).Label();
			return $"{key} {name}: {state}";
		})
		.Concat(HotkeyManager.triggerInstructionLines)
	));
	private static Vector2 headerSize = Vector2.zero;
	private static Vector2 bodySize = Vector2.zero;

	public static GuiManager Instance { get; private set; } = null!;
	public void Awake() {
		headerStyle = new() {
			alignment = TextAnchor.MiddleCenter,
			fontSize = HeaderFontSize,
		};

		bodyStyle = new() {
			fontSize = BodyFontSize,
		};

		headerStyle.border.top = 1;
		headerStyle.border.left = 1;
		headerStyle.padding.bottom = 4;
		headerStyle.normal.textColor = Color.white;

		bodyStyle.border.left = 1;
		bodyStyle.normal.textColor = Color.white;

		headerContent = new($"{Core.Module} Status");

		Instance = this;
	}
	public void OnGUI() {
		if (!Config.ShowOverlay || !StateGuard.Ready)
			return;

		GUI.backgroundColor = Color.gray;
		GUI.contentColor = Color.white;

		GUIContent body = bodyContent;

		headerSize = new(OverlayWidth, headerStyle.CalcHeight(headerContent, OverlayWidth));
		bodySize = new(OverlayWidth, bodyStyle.CalcHeight(body, OverlayWidth));

		Vector2 headerPos = new(ScreenSize.x - OverlayWidth, ScreenSize.y - OverlayBottomEdge - bodySize.y - headerSize.y);
		Vector2 bodyPos = new(headerPos.x, headerPos.y + headerSize.y);

		GUI.Label(new Rect(headerPos, headerSize), headerContent, headerStyle);
		GUI.Label(new Rect(bodyPos, bodySize), body, bodyStyle);
	}
}
