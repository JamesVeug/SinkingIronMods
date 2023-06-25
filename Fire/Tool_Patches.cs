using System.Collections.Generic;
using System.Linq;
using EndlessModeExtensions;
using WaterPumpMod;
using HarmonyLib;
using Utils;
using UnityEngine;

namespace Patches
{
	[HarmonyPatch]
	public static class Tool_Patches
	{
		//
		// Show particles
		//
		[HarmonyPatch(typeof(Tool), "PlayElectricSparks"), HarmonyPrefix]
		public static bool PlayParticles(Tool __instance)
		{
			if (__instance.gameObject.name != "ExtinguisherWeapon")
			{
				return true;
			}

			Transform extinguisher = Helpers.FindChild(__instance.transform, "extinguisher", true);
			if (extinguisher == null)
			{
				MainMod.Logger.Error($"Could not find child from path extinguisher");
				Helpers.LogGameObject(__instance.gameObject, false, true);
				return true;
			}

			ParticleSystem particleSystems = extinguisher.GetComponentInChildren<ParticleSystem>();
			if (particleSystems == null)
			{
				MainMod.Logger.Error($"Could not find child from path extinguisher");
				Helpers.LogGameObject(__instance.gameObject, false, true);
				return true;
			}

			if (!particleSystems.isPlaying)
			{
				particleSystems.Play();
			}
			return false;
		}
		
		//
		// Disable particles
		//
		[HarmonyPatch(typeof(Tool), "StopElectricSparks"), HarmonyPrefix]
		public static bool StopParticles(Tool __instance)
		{
			if (__instance.gameObject.name != "ExtinguisherWeapon")
			{
				return true;
			}

			Transform extinguisher = Helpers.FindChild(__instance.transform, "extinguisher", true);
			if (extinguisher == null)
			{
				MainMod.Logger.Error($"Could not find child from path extinguisher");
				Helpers.LogGameObject(__instance.gameObject, false, true);
				return true;
			}

			ParticleSystem particleSystems = extinguisher.GetComponentInChildren<ParticleSystem>();
			if (particleSystems == null)
			{
				MainMod.Logger.Error($"Could not find child from path extinguisher");
				Helpers.LogGameObject(__instance.gameObject, false, true);
				return true;
			}

			if (particleSystems.isPlaying)
			{
				particleSystems.Stop();
			}
			return false;
		}
	}
}