using HarmonyLib;

namespace Patches
{
	[HarmonyPatch]
	public static class EyeLightButton_Patches
	{
		[HarmonyPatch(typeof(EyeLightButton), "Interact"), HarmonyPrefix]
		public static bool StopIfOnFire(EyeLightButton __instance)
		{
			if (!__instance.gameObject.GetComponentInChildren<Fire.Fire>(false))
			{
				return true;
			}

			return false;
		}
	}
}