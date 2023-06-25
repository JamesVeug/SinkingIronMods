using System.Collections.Generic;
using MelonLoader;
using Utils;
using UnityEngine;

[assembly: MelonInfo(typeof(WaterPumpMod.MainMod), "Fire Mod", "1.0.0", "Sneak")]
[assembly: MelonGame("Lixian Games", "Sinking Iron")]

namespace WaterPumpMod
{
	public class MainMod : MelonMod
	{
		public static MelonLogger.Instance Logger;
		public static MainMod Instance;
		
		public static AssetBundle AssetBundle;
		public static List<Sprite> FireSprites;
		public static AudioClip FireSFX;
		public static AudioClip ExtinguisherSFX;

		public override void OnInitializeMelon()
		{
			Instance = this;
			Logger = this.LoggerInstance;
			
			// Load asset bundle
			AssetBundle = Helpers.LoadAssetBundleFromEmbeddedResource("fire");
			if (AssetBundle != null)
			{
				FireSFX = AssetBundle.LoadAsset<AudioClip>("fireSFX");
				if (FireSFX == null)
				{
					Logger.Error("Could not load fireSFX!");
				}
				
				ExtinguisherSFX = AssetBundle.LoadAsset<AudioClip>("extinguisherSFX");
				if (ExtinguisherSFX == null)
				{
					Logger.Error("Could not load extinguisherSFX!");
				}
			}
			else
			{
				Logger.Error("Could not load fire AssetBundle!");
			}

			FireSprites = new List<Sprite>(32);
			for (int i = 1; i <= 32; i++)
			{
				Texture2D texture2D = Helpers.LoadTextureFromEmbeddedResource($"firesprites_{i}.png");
				if (texture2D != null)
				{
					Sprite sprite = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), new Vector2(0.5f, 0.5f), 100f);
					FireSprites.Add(sprite);
				}
			}

			Logger.Msg("Fire Mod loaded!");
		}
	}
}