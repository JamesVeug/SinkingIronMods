using HarmonyLib;
using Sonity;
using UnityEngine;

namespace SinkingIronMod2
{
	[HarmonyPatch]
	public static class Patches
	{
		public static SoundEvent HitSqueekSFX = null;
		public static SoundEvent WhooshSqueekSFX = null;
			
		[HarmonyPatch(typeof(MaterialSwitcherHammer), "Start"), HarmonyPrefix]
		public static bool MaterialSwitcherHammer(MaterialSwitcherHammer __instance)
		{
			AssetBundle loadFromFile = Plugin.chickenBundle;
			if (loadFromFile == null)
			{
				Plugin.Logger.Msg($"bundle is null");
				return true;
			}

			GameObject chickenPrefab = loadFromFile.LoadAsset<GameObject>("chickenModel");
			if (chickenPrefab == null)
			{
				Plugin.Logger.Msg($"chickenPrefab is null");
				return true;
			}

			GameObject clone = GameObject.Instantiate(chickenPrefab, __instance.transform);
			clone.transform.localPosition = new Vector3(-0.00114f, 0.00356f, -0.00076f) + new Vector3(0.0005f,-0.005f,0.000f); // Z Up
			clone.transform.localScale = new Vector3(0.005f, 0.01f, 0.005f);
			clone.transform.localEulerAngles = new Vector3(273.107f, -0.001f, 0.001f);
			clone.layer = __instance.gameObject.layer;
			clone.SetActive(true);
			
			__instance.GetComponent<MeshRenderer>().enabled = false;
			return false;
		}
		
		/*[HarmonyPatch(typeof(SceneChanger), "ChangeLevelWithFade"), HarmonyPrefix]
		public static bool ChangeLevelWithFade(SceneChanger __instance, ref string nextLevel)
		{
			Debug.Log($"ChangeLevelWithFade " + nextLevel);
			if (SceneManager.GetActiveScene().name == "SCENE_00_Init")
			{
				nextLevel = "SCENE_10_SubmarineUp";
			}
			Debug.Log($"ChangeLevelWithFade " + nextLevel);
			return true;
		}
		
		[HarmonyPatch(typeof(SceneChanger), "ChangeLevelWithoutFade"), HarmonyPrefix]
		public static bool ChangeLevelWithoutFade(SceneChanger __instance, ref string nextLevel)
		{
			Debug.Log($"ChangeLevelWithoutFade " + nextLevel);
			if (SceneManager.GetActiveScene().name == "SCENE_00_Init")
			{
				nextLevel = "SCENE_10_SubmarineUp";
			}
			Debug.Log($"ChangeLevelWithoutFade " + nextLevel);
			return true;
		}*/


		[HarmonyPatch(typeof(Tool), "Start"), HarmonyPrefix]
		public static bool Tool_Start(Tool __instance)
		{
			if (HitSqueekSFX != null)
			{
				return true;
			}
			
			SoundEvent hitSoundEvent = __instance.GetField<SoundEvent>("SFX_Hammer_Hit_SE");
			if (hitSoundEvent != null)
			{
				SoundContainer container = hitSoundEvent.internals.soundContainers[0];

				HitSqueekSFX = ScriptableObject.Instantiate(hitSoundEvent);
				SoundContainer squeekContainer = ScriptableObject.Instantiate(container);
				HitSqueekSFX.internals.soundContainers[0] = squeekContainer;
				squeekContainer.internals.audioClips = Plugin.hitSFX.ToArray();
				squeekContainer.internals.data.volumeRatio *= 4f;
				HitSqueekSFX.name = "SqueekHit";
				
				WhooshSqueekSFX = ScriptableObject.Instantiate(hitSoundEvent);
				SoundContainer whooshContainer = ScriptableObject.Instantiate(container);
				WhooshSqueekSFX.internals.soundContainers[0] = whooshContainer;
				whooshContainer.internals.audioClips = Plugin.whooshSFX.ToArray();
				whooshContainer.internals.data.volumeRatio *= 3f;
				WhooshSqueekSFX.name = "SqueekWoosh";
			}

			return true;
		}
	}
}