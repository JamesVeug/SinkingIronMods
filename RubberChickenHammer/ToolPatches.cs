using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using Sonity;
using UnityEngine;
using Patches = SinkingIronMod2.Patches;

[HarmonyPatch]
public class Tool_Patches
{
	public static bool Hit;
		
	public static IEnumerable<MethodBase> TargetMethods()
	{
		yield return AccessTools.Method(typeof(Tool), "Fix", new Type[]{});
	}

	public static bool Prefix(Tool __instance)
	{
		Hit = false;
		return true;
	}
	
	public static void Postfix(Tool __instance)
	{
		if (__instance.IsSingleHit())
		{
			if (Hit)
			{
				SoundManager.Instance.Play2D(Patches.HitSqueekSFX);
			}
			else
			{
				SoundManager.Instance.Play2D(Patches.WhooshSqueekSFX);
			}
		}

		Hit = false;
	}

	public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
	{
		// we want to change this
			
		// SFX_Hammer_Hit_SE.Play2D();
			
		// to
			
		// SFX_Hammer_Hit_SE.Play2D();
		// Callback();

		MethodInfo methodInfo = typeof(SoundEvent).GetMethod("Play2D",  new Type[]{});
		MethodInfo callbackInfo = SymbolExtensions.GetMethodInfo(() => Callback());
			
			
		List<CodeInstruction> codeInstructions = new List<CodeInstruction>(instructions);
		for (int i = 0; i < codeInstructions.Count; i++)
		{
			if (codeInstructions[i].operand == methodInfo)
			{
				codeInstructions.Insert(++i, new CodeInstruction(OpCodes.Call, callbackInfo));
				break;
			}
		}

		return codeInstructions;
	}

	private static void Callback()
	{
		Hit = true;
	}
}