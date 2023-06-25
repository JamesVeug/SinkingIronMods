using WaterPumpMod;
using Utils;
using Sonity;
using UnityEngine;
using HarmonyLib;

namespace Fire
{
	[HarmonyPatch]
	public class SFX_Patches
	{
		public static SoundEvent SteamBurstSFX = null;
		public static SoundEvent EngineSFX = null;

		[HarmonyPatch(typeof(Tool), "Start"), HarmonyPrefix]
		public static bool CreateSFX(Tool __instance)
		{
			InitializeSFX(__instance);

			return true;
		}

		public static void InitializeSFX(Tool tool)
		{
			if (SteamBurstSFX != null) 
				return;
			
			SoundEvent hitSoundEvent = tool.GetField<SoundEvent>("SFX_Welder_Hit");
			hitSoundEvent = hitSoundEvent == null ? tool.GetField<SoundEvent>("SFX_Hammer_Hit_SE") : hitSoundEvent;
			
			SoundContainer container = hitSoundEvent.internals.soundContainers[0];

			SteamBurstSFX = ScriptableObject.Instantiate(hitSoundEvent);
			SoundContainer squeekContainer = ScriptableObject.Instantiate(container);
			SteamBurstSFX.internals.soundContainers[0] = squeekContainer;
			squeekContainer.internals.audioClips = new AudioClip[] { MainMod.SteamBurstSFX };
			squeekContainer.internals.data.volumeRatio *= 6f;
			SteamBurstSFX.name = "SteamBurstSFX";

			EngineSFX = ScriptableObject.Instantiate(hitSoundEvent);
			SoundContainer whooshContainer = ScriptableObject.Instantiate(container);
			EngineSFX.internals.soundContainers[0] = whooshContainer;
			whooshContainer.internals.audioClips = new AudioClip[] { MainMod.PumpEngineSFX };
			whooshContainer.internals.data.volumeRatio *= 6f;
			whooshContainer.internals.data.loopEnabled = true;
			EngineSFX.name = "PumpEngineSFX";
		}
	}
}