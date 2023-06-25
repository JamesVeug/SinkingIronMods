using System.Collections.Generic;
using System.Linq;
using EndlessModeExtensions;
using Fire;
using WaterPumpMod;
using HarmonyLib;
using Utils;
using Sonity;
using UnityEngine;

namespace Patches
{
	[HarmonyPatch]
	public static class Extinguisher_Patches
	{
		//
		// Add the Extinguisher to the WeaponSwitcher
		//
		[HarmonyPatch(typeof(WeaponSwitcher), "Start"), HarmonyPostfix]
		public static void CreateExtinguisher(WeaponSwitcher __instance)
		{
			GameObject[] weapons = __instance.GetWeapons();
			GameObject wieldingTool = weapons[1];

			// Clone the extinguisher and parent it to an object named Hammer
			GameObject clone = GameObject.Instantiate(wieldingTool, wieldingTool.transform.parent);
			clone.GetComponentInChildren<ParticleSystem>().Stop();
			clone.SetActive(false);
			clone.name = "ExtinguisherWeapon";

			Tool tool = clone.GetComponent<Tool>();
			SFX_Patches.InitializeSFX(tool);
			SoundEvent extinguisherFX = SFX_Patches.ExtinguisherFX;
			tool.SetSFX_Welder_Blank(extinguisherFX);
			
			List<GameObject> allWeapons = weapons.ToList();
			allWeapons.Add(clone);
			if (!__instance.SetWeapons(allWeapons.ToArray()))
			{
				MainMod.Logger.Error($"Could not add Extinguisher to WeaponSwitcher");
				GameObject.Destroy(clone);
				return;
			}
			else
			{
				Transform WeldingGunFinal = Helpers.FindChild(clone.transform, "WeldingGunFinal", true);
				if (WeldingGunFinal == null)
				{
					MainMod.Logger.Error($"Could not find child from path WeldingGunFinal");
					Helpers.LogGameObject(clone.gameObject, false, true);
					GameObject.Destroy(clone);
				}
				else
				{
					foreach (Transform child in WeldingGunFinal)
					{
						child.gameObject.SetActive(false);
					}

					GameObject model = MainMod.AssetBundle.LoadAsset<GameObject>("extinguisher");
					GameObject modelClone = GameObject.Instantiate(model, WeldingGunFinal);
					modelClone.name = "extinguisher";
					modelClone.SetActive(true);
				}
			}
		}

		[HarmonyPatch(typeof(WeaponSwitcher), "Update"), HarmonyPrefix]
		public static bool Hotkeys(WeaponSwitcher __instance)
		{
			int weaponEquipped = __instance.WeaponEquipped();
			GameObject[] weapons = __instance.GetWeapons();
			GameObject equippedWeapon = weapons[weaponEquipped];
			
			if (Input.GetAxis("Mouse ScrollWheel") > 0f && weapons[weaponEquipped].GetComponent<Tool>().HasFinishedSelectedAnimation())
			{
				__instance.SetWeaponEquipped(weaponEquipped+1);
				__instance.ChangeWeaponPublic();
			}
			if (Input.GetAxis("Mouse ScrollWheel") < 0f && weapons[weaponEquipped].GetComponent<Tool>().HasFinishedSelectedAnimation())
			{
				__instance.SetWeaponEquipped(weaponEquipped-1);
				__instance.ChangeWeaponPublic();
			}
			if (Input.GetKeyDown(KeyCode.Alpha1) && equippedWeapon.GetComponent<Tool>().HasFinishedSelectedAnimation())
			{
				__instance.SetWeaponEquipped(0);
				__instance.ChangeWeaponPublic();
			}
			if (Input.GetKeyDown(KeyCode.Alpha2) && equippedWeapon.GetComponent<Tool>().HasFinishedSelectedAnimation())
			{
				__instance.SetWeaponEquipped(1);
				__instance.ChangeWeaponPublic();
			}
			if (Input.GetKeyDown(KeyCode.Alpha3) && equippedWeapon.GetComponent<Tool>().HasFinishedSelectedAnimation())
			{
				__instance.SetWeaponEquipped(2);
				__instance.ChangeWeaponPublic();
			}

			return false;
		}
		
		[HarmonyPatch(typeof(Tool), "Fix"), HarmonyPrefix]
		public static bool ExtinguishFire(Tool __instance)
		{
			if (__instance.gameObject.name != "ExtinguisherWeapon")
			{
				return true;
			}
			
			RaycastHit raycastHit;
			Camera camera = __instance.Cam();
			if (Physics.Raycast(camera.transform.position, camera.transform.forward, out raycastHit, __instance.InteractionRange()))
			{
				EyeLightButton component = raycastHit.transform.GetComponent<EyeLightButton>();
				if (component != null)
				{
					Fire.Fire fire = component.gameObject.GetComponentInChildren<Fire.Fire>();
					if (fire != null)
					{
						fire.gameObject.SetActive(false);
					}
				}
			}
			
			return false;
		}
		
		[HarmonyPatch(typeof(Tool), "Update"), HarmonyPrefix]
		public static bool Update(Tool __instance)
		{
			if (__instance.gameObject.name != "ExtinguisherWeapon")
			{
				return true;
			}

			SoundEvent soundEvent = __instance.SFX_Welder_Blank();
			if (Input.GetMouseButtonDown(0))
			{
				soundEvent.Play2D();
			}
			if (Input.GetMouseButtonUp(0))
			{
				soundEvent.Stop2D();
			}
			
			return false;
		}
	}
}