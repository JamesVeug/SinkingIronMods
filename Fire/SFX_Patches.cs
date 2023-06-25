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
		public static SoundEvent FireFX = null;
		public static SoundEvent ExtinguisherFX = null;

		[HarmonyPatch(typeof(Tool), "Start"), HarmonyPrefix]
		public static bool CreateSFX(Tool __instance)
		{
			InitializeSFX(__instance);

			return true;
		}

		public static void InitializeSFX(Tool tool)
		{
			if (FireFX != null) 
				return;
			
			SoundEvent hitSoundEvent = tool.GetField<SoundEvent>("SFX_Welder_Hit");
			
			SoundContainer container = hitSoundEvent.internals.soundContainers[0];

			FireFX = ScriptableObject.Instantiate(hitSoundEvent);
			SoundContainer squeekContainer = ScriptableObject.Instantiate(container);
			FireFX.internals.soundContainers[0] = squeekContainer;
			squeekContainer.internals.audioClips = new AudioClip[] { MainMod.FireSFX };
			squeekContainer.internals.data.volumeRatio *= 6f;
			squeekContainer.internals.data.loopEnabled = true;
			FireFX.name = "FireSFX";

			ExtinguisherFX = ScriptableObject.Instantiate(hitSoundEvent);
			SoundContainer whooshContainer = ScriptableObject.Instantiate(container);
			ExtinguisherFX.internals.soundContainers[0] = whooshContainer;
			whooshContainer.internals.audioClips = new AudioClip[] { MainMod.ExtinguisherSFX };
			whooshContainer.internals.data.volumeRatio *= 10f;
			whooshContainer.internals.data.loopEnabled = true;
			ExtinguisherFX.name = "ExtinguisherSFX";
		}
	}
}