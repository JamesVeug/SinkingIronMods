using System.Collections.Generic;
using EndlessModeExtensions;
using Fire;
using WaterPumpMod;
using HarmonyLib;
using UnityEngine;

namespace Patches
{
	[HarmonyPatch]
	public static class Fire_Patches
	{
		[HarmonyPatch(typeof(MonsterEyeManager), "Start"), HarmonyPostfix]
		public static void AddFireObjects(MonsterEyeManager __instance)
		{
			// Init FireManager
			//__instance.gameObject.AddComponent<FireManager>();
			FireManager.Fires = new List<Fire.Fire>();
			
			// Try and initialize the managers
			GameObject fireTemplate = MainMod.AssetBundle.LoadAsset<GameObject>("Fire");
			if (fireTemplate == null)
			{
				MainMod.Logger.Error($"Could not find Fire from asset bundle");
				return;
			}


			GameObject o = GameObject.Find("---Interactables---");
			if (o == null)
			{
				MainMod.Logger.Error($"Could not find ---Interactables---");
				return;
			}

			EyeLightButton[] lightButtons = o.GetComponentsInChildren<EyeLightButton>(true);
			foreach (EyeLightButton button in lightButtons)
			{
				GameObject fireClone = GameObject.Instantiate(fireTemplate, button.transform);
				fireClone.transform.localPosition += new Vector3(0, 0.1f, 0);
				if (button.gameObject.name.ToLower().Contains("left"))
				{
					fireClone.transform.localPosition += new Vector3(0, 0.1f, 0.1f);
				}
				else if (button.gameObject.name.ToLower().Contains("right"))
				{
					fireClone.transform.localPosition += new Vector3(0, 0.1f, -0.1f);
				}
				
				
				Fire.Fire fire = fireClone.AddComponent<Fire.Fire>();
				FireManager.Fires.Add(fire);
				
				fire.gameObject.SetActive(false);
			}
		}
		
		[HarmonyPatch(typeof(EyeLightButton), "Interact"), HarmonyPrefix]
		public static bool StopButtonInteraction(EyeLightButton __instance)
		{
			Fire.Fire fire = __instance.gameObject.GetComponentInChildren<Fire.Fire>(true);
			if (fire == null)
			{
				MainMod.Logger.Error($"Could not find Fire from {__instance.gameObject.name}");
				return true;
			}

			if (fire.gameObject.activeSelf)
			{
				MainMod.Logger.Msg($"Stopped interaction with {__instance.gameObject.name}");
				return false;
			}

			MainMod.Logger.Msg($"Fire not active {__instance.gameObject.name}");
			return true;
		}
		
		[HarmonyPatch(typeof(EyeLightButton), "Interact"), HarmonyPrefix]
		public static void SetButtonOnFire(EyeLightButton __instance)
		{
			MainMod.Logger.Msg($"Interacted with {__instance.gameObject.name}");
			if (__instance.MonsterEye().gameObject.activeInHierarchy && __instance.CanFlashMonster())
			{
				Fire.Fire fire = __instance.gameObject.GetComponentInChildren<Fire.Fire>(true);
				if (fire == null)
				{
					MainMod.Logger.Error($"Could not find Fire from {__instance.gameObject.name}");
					return;
				}
			
				fire.gameObject.SetActive(true);
				MainMod.Logger.Msg($"Set fire {fire.gameObject.name} to active");
			}
		}
	}
}