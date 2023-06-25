using System;
using System.Collections;
using System.Reflection;
using HarmonyLib;
using SinkingIronMod2;
using UnityEngine;
using UnityEngine.UI;

namespace EndlessModePatches
{
	[HarmonyPatch]
	public static class EndlessFlowGameLogic_Patches
	{
		//
		// Don't Finish the game by timer if we are in Endless Mode
		//
		[HarmonyPatch(typeof(GameFlowController), "Start"), HarmonyPostfix]
		public static void CreateEndlessModeCounter(GameFlowController __instance)
		{
			EndlessMode.Logger.Error($"CreateEndlessModeCounter {EndlessMode.IsEndlessMode}");
			if (!EndlessMode.IsEndlessMode)
			{
				return;
			}

			GameObject gameObject = EndlessMode.AssetBundle.LoadAsset<GameObject>("UI");
			if (gameObject == null)
			{
				EndlessMode.Logger.Error("Could not load UI prefab.");
				return;
			}
			
			GameObject ui = GameObject.Instantiate(gameObject);
			Text text = gameObject.GetComponentInChildren<Text>();
			if (text == null)
			{
				EndlessMode.Logger.Error("Could not find Text component in UI prefab.");
				GameObject.Destroy(ui);
				return;
			}
			
			__instance.StartCoroutine(UpdateSubmergeText(ui, __instance));
		}

		public static IEnumerator UpdateSubmergeText(GameObject gameObject, GameFlowController flowController)
		{
			Text text = gameObject.GetComponentInChildren<Text>();
			EndlessMode.Depth = 0;
			// Show the depth
			text.text = $"{EndlessMode.Depth}m";
			
			yield return new WaitForSeconds(1);
			GameManager gameManager = GameManager.instance;
			MonsterEyeManager monsterEyeManager = GameObject.FindObjectOfType<MonsterEyeManager>();
			monsterEyeManager.enabled = true;

			// TODO: Display m in gameover screen
			// TODO: Record total repairs and put that in the gameover screen
			
			// Difficulty 0 -> 10 inclusive
			// Higher difficulty reduces time between attacks and higher chance of double damages
			// TODO: Adjust chance of eye with difficulty
			int difficulty = 0;
			bool hardMode = false;
			flowController.GameDifficultyController(difficulty);
			
			
			while (true)
			{
				// Show the depth
				text.text = $"{EndlessMode.Depth}m";
				
				yield return new WaitForSeconds(1);
				EndlessMode.Depth++;
				if(EndlessMode.Depth % 100 == 0)
				{
					difficulty++;
					if (difficulty > 10)
					{
						difficulty = 1; // difficulty 0's delays are too short so this speeds it up
						hardMode = !hardMode;
						gameManager.SetHardMode(hardMode);
						EndlessMode.Logger.Error("Hard Mode: " + hardMode);
					}

					flowController.GameDifficultyController(difficulty);
				}
			}
			
			EndlessMode.Logger.Error("UpdateSubmergeText Done");
		}

		private static void GameDifficultyController(this GameFlowController flowController, int amount)
		{
			MethodInfo methodInfo = typeof(GameFlowController).GetMethod("Down_ChangePhase", 
				BindingFlags.Instance | BindingFlags.NonPublic, 
				null, 
				new Type[] { typeof(int) }, 
				null);
			if (methodInfo != null)
			{
				EndlessMode.Logger.Error("Difficulty: " + amount);
				methodInfo.Invoke(flowController, new object[] { amount });
			}
			else
			{
				EndlessMode.Logger.Error("Could not find GameDifficultyController method.");
			}
		}
	}
}