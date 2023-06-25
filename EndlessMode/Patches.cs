using EndlessModeExtensions;
using HarmonyLib;
using SinkingIronMod2;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization.Components;

namespace EndlessModePatches
{
	[HarmonyPatch]
	public static class Patches
	{
		//
		// Add button to enable endless mode
		//
		[HarmonyPatch(typeof(MainMenuDisplay), "Start"), HarmonyPostfix]
		public static void AddEndlessModeButton(MainMenuDisplay __instance)
		{
			Transform hardButton = Utils.FindChild(__instance.transform, "HardButton", true);
			if (hardButton == null)
			{
				EndlessMode.Logger.Error("Could not add Endless Mode button. HardButton not found.");
				return;
			}

			GameObject endlessModeButton = GameObject.Instantiate(hardButton.gameObject, hardButton.parent);
			endlessModeButton.name = "EndlessButton";

			Button button = endlessModeButton.GetComponent<Button>();
			if (button == null)
			{
				GameObject.Destroy(endlessModeButton);
				EndlessMode.Logger.Error("Could not add Endless Mode button. endlessModeButton has no Button component.");
				return;
			}
			
			Transform textGameObject = button.transform.GetChild(0);
			LocalizeStringEvent localize = textGameObject.GetComponent<LocalizeStringEvent>();
			if (localize != null)
			{
				// TODO: Localise
				localize.enabled = false;
			}
			else
			{
				EndlessMode.Logger.Error("Could not change disable endless mode buttons localise. Doesn't exist?");
			}

			TextMeshProUGUI text = textGameObject.GetComponent<TextMeshProUGUI>();
			if (text != null)
			{
				text.SetText("Endless");
			}
			else
			{
				EndlessMode.Logger.Error("Could not change Endless Mode button text. Text component not found.");
			}

			button.onClick.RemoveAllListeners();
			button.onClick.AddListener(delegate
			{
				EndlessMode.Logger.Error("Endless Mode enabled!");
				EndlessMode.IsEndlessMode = true;
				GameManager.instance.SetHardMode(true);
				__instance.DifficultyPanel().SetActive(value: false);
				__instance.SkipProloguePanel().SetActive(value: false);
				GameObject.FindObjectOfType<SceneChanger>().ChangeLevelWithFade("SCENE_10_SubmarineUp");
				AudioFade audioFade = GameObject.Find("MUSIC_MainMenu").GetComponent<AudioFade>();
				audioFade.SetNewFadeSpeed(1);
				audioFade.SetNewFade(0);
			});
		}
	}
}