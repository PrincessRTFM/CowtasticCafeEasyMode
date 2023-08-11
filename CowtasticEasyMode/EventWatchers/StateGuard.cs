namespace PrincessRTFM.CowtasticCafeEasyMode.EventWatchers;

using PrincessRTFM.CowtasticCafeEasyMode.Logging;

using UnityEngine;
using UnityEngine.SceneManagement;

internal class StateGuard: MonoBehaviour {
	private static bool seenMainMenu = false;
	public static bool Ready => Core.Initialised && seenMainMenu;

	public void Awake() => SceneManager.activeSceneChanged += onSceneChanged;
	private static void onSceneChanged(Scene current, Scene next) {
		if (next.name == "MainMenu") {
			SceneManager.activeSceneChanged -= onSceneChanged;
			seenMainMenu = true;
			Log.Debug("Main menu scene triggered");
		}
	}
}
