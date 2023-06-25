using MelonLoader;
using UnityEngine;
using Utils;

[assembly: MelonInfo(typeof(WaterPumpMod.MainMod), "Water Pump", "1.0.0", "Sneak")]
[assembly: MelonGame("Lixian Games", "Sinking Iron")]

namespace WaterPumpMod
{
	public class MainMod : MelonMod
	{
		public static MelonLogger.Instance Logger;
		public static MainMod Instance;
		public static AssetBundle AssetBundle;
		public static float WaterStartHeight;
		public static WaterRisingController WaterRisingController;
		public static AudioClip SteamBurstSFX;
		public static AudioClip PumpEngineSFX;


		public override void OnInitializeMelon()
		{
			Instance = this;
			Logger = this.LoggerInstance;
			
			AssetBundle = Helpers.LoadAssetBundleFromEmbeddedResource("waterpump");
			if (AssetBundle != null)
			{
				SteamBurstSFX = AssetBundle.LoadAsset<AudioClip>("steam_burst");
				if (SteamBurstSFX == null)
				{
					Logger.Error("Could not load SteamBurstSFX!");
				}
				
				PumpEngineSFX = AssetBundle.LoadAsset<AudioClip>("pump_engine");
				if (PumpEngineSFX == null)
				{
					Logger.Error("Could not load PumpEngineSFX!");
				}
			}
			else
			{
				Logger.Error("Could not load waterpump AssetBundle!");
			}

			Logger.Msg("Water Pump Mod loaded!");
		}
	}
}