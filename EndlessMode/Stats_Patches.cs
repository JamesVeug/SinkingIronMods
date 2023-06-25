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
	[HarmonyPatch(typeof(FixableWall), "FixWall", new Type[]{typeof(float), typeof(Tool)})]
	public static class Stats_Patches
	{
		[HarmonyPrefix]
		public static bool FixWall_Prefix(FixableWall __instance, float fixRate, Tool tool, out bool __state)
		{
			__state = __instance.WallHealth() < (tool.IsSingleHit() ? 200f : 100f); // IsDamaged
			return true;
		}
		
		[HarmonyPostfix]
		public static void FixWall_Postfix(FixableWall __instance, float fixRate, Tool tool, bool __state)
		{
			if (!EndlessMode.IsEndlessMode || !__state)
			{
				return;
			}

			if (tool.IsSingleHit())
			{
				if (__instance.WallHealth() >= 200f)
				{
					EndlessMode.Repairs++;
				}
			}
			else
			{
				if (__instance.WallHealth() >= 100f) // Leaks are hard coded to have 100 hp
				{
					EndlessMode.LeaksFixed++;
				}
			}
		}
	}
}