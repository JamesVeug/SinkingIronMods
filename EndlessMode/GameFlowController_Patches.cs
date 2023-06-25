using HarmonyLib;
using SinkingIronMod2;
using UnityEngine;
using UnityEngine.UI;

namespace EndlessModePatches
{
	[HarmonyPatch]
	public static class GameFlowController_Patches
	{
		//
		// Don't Finish the game by timer if we are in Endless Mode
		//
		[HarmonyPatch(typeof(GameFlowController), "Down_Phase_End"), HarmonyPrefix]
		public static bool NeverReachBottom()
		{
			EndlessMode.Logger.Error($"TriggerFinishedLevelUp {EndlessMode.IsEndlessMode}");
			if (!EndlessMode.IsEndlessMode)
			{
				return true;
			}

			// Never finish by a timer
			return false;
		}
		
		//
		// Don't change the music if we are in Endless Mode
		//
		[HarmonyPatch(typeof(GameFlowController), "ReachedHalfway"), HarmonyPrefix]
		public static bool NeverReachHalfWay()
		{
			EndlessMode.Logger.Error($"ReachedHalfway {EndlessMode.IsEndlessMode}");
			if (!EndlessMode.IsEndlessMode)
			{
				return true;
			}

			// Never make the music intense
			return false;
		}
		
		//
		// Stop the Gauge from moving if we are in Endless Mode
		//
		[HarmonyPatch(typeof(GameFlowController), "Start"), HarmonyPrefix]
		public static bool DisableGauge(GameFlowController __instance)
		{
			if (EndlessMode.IsEndlessMode)
			{
				Animator animator = __instance.GetField<Animator>("animator");
				if (animator != null)
				{
					animator.enabled = false;
				}
				else
				{
					EndlessMode.Logger.Error($"Could not disable gauge. animator is null.");
				}
			}

			return true;
		}
	}
}