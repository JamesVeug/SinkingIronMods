using System;
using System.Collections;
using System.Reflection;
using EndlessModeExtensions;
using HarmonyLib;
using SinkingIronMod2;
using UnityEngine;
using UnityEngine.UI;

namespace EndlessModePatches
{
	[HarmonyPatch]
	public static class GameOver_Patches
	{
		//
		// Update the gameover stats if we are in Endless Mode
		//
		[HarmonyPatch(typeof(GameOverDisplay), "TriggerGameOver"), HarmonyPostfix]
		public static void TriggerGameOver(GameOverDisplay __instance)
		{
			EndlessMode.Logger.Error($"TriggerGameOver {EndlessMode.IsEndlessMode}");
			if (!EndlessMode.IsEndlessMode)
			{
				return;
			}
			
			GameObject gameOverCanvas = __instance.transform.gameObject;
			if (gameOverCanvas == null)
			{
				EndlessMode.Logger.Error("Could not find GameOverCanvas.");
				return;
			}
			
			GameObject gameObject = EndlessMode.AssetBundle.LoadAsset<GameObject>("GameOverStats");
			if (gameObject == null)
			{
				EndlessMode.Logger.Error("Could not load GameOverStats prefab.");
				return;
			}

			GameObject clone = GameObject.Instantiate(gameObject, gameOverCanvas.transform);

			clone.GetComponent<Canvas>().sortingOrder = gameOverCanvas.GetComponentInChildren<Canvas>().sortingOrder + 1; 
				
			var GameOverStatsText = clone.GetComponentInChildren<Text>();
			if (GameOverStatsText == null)
			{
				EndlessMode.Logger.Error("Could not find Text component in GameOverStats prefab.");
				GameObject.Destroy(clone);
				return;
			}
			
			GameOverStatsText.text = $"Depth: {EndlessMode.Depth}m\n" +
			                                     $"Repairs: {EndlessMode.Repairs}\n" +
			                                     $"Leaks Fixed: {EndlessMode.LeaksFixed}\n";
		}
	}
}