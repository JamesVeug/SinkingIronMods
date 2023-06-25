using HarmonyLib;
using UnityEngine;

namespace WaterPumpMod
{
	[HarmonyPatch]
	public class Patches
	{
		[HarmonyPatch(typeof(WeaponSwitcher), "Start"), HarmonyPrefix]
		public static bool CreateButton(WeaponSwitcher __instance)
		{
			GameObject pumpButtonTemplate = MainMod.AssetBundle.LoadAsset<GameObject>("PumpButton");
			if (pumpButtonTemplate == null)
			{
				MainMod.Logger.Error($"Could not get pumpButtonTemplate");
				return true;
			}

			GameObject cloneGameObject = GameObject.Instantiate(pumpButtonTemplate);
			//cloneGameObject.name = "WaterPumpButton";
			//cloneGameObject.transform.position = new Vector3(3.445f - 0.1f, 1.347f, 1.058f);
			//cloneGameObject.transform.rotation = Quaternion.Euler(53.002f, 87.909F + 180, -0.3619995F);
			//cloneGameObject.transform.rotation *= Quaternion.Euler(180, 0, 0);

			cloneGameObject.AddComponent<WaterPumpButton>();

			MainMod.Logger.Msg($"Created WaterPumpButton");
			return true;
		}
		
		[HarmonyPatch(typeof(WaterRisingController), "Start"), HarmonyPrefix]
		public static bool CacheWaterDefaultPosition(WaterRisingController __instance)
		{
			MainMod.WaterStartHeight = __instance.transform.position.y;
			MainMod.WaterRisingController = __instance;

			MainMod.Logger.Msg($"Created WaterPumpButton");
			return true;
		}
	}
}