using System.Collections.Generic;
using MelonLoader;
using SinkingIronMod2;
using UnityEngine;

[assembly: MelonInfo(typeof(Plugin), "Sinking Iron Mod", "1.0.0", "Sneak")]
[assembly: MelonGame("Lixian Games", "Sinking Iron")]

namespace SinkingIronMod2
{
	public class Plugin : MelonMod
	{
		public static MelonLogger.Instance Logger;
		public static Plugin Instance;

		public static AssetBundle chickenBundle = null;
		public static List<AudioClip> hitSFX = null;
		public static List<AudioClip> whooshSFX = null;
		
		public override void OnInitializeMelon()
		{
			Instance = this;
			Logger = this.LoggerInstance;

			chickenBundle = Utils.LoadAssetBundleFromEmbeddedResource("chicken");
			if (chickenBundle != null)
			{
				hitSFX = new List<AudioClip>();
				hitSFX.Add(chickenBundle.LoadAsset<AudioClip>("c1"));
				hitSFX.Add(chickenBundle.LoadAsset<AudioClip>("c2"));
				hitSFX.Add(chickenBundle.LoadAsset<AudioClip>("c3"));
				hitSFX.Add(chickenBundle.LoadAsset<AudioClip>("c5"));
				
				whooshSFX = new List<AudioClip>();
				whooshSFX.Add(chickenBundle.LoadAsset<AudioClip>("c4"));
				whooshSFX.Add(chickenBundle.LoadAsset<AudioClip>("c6"));
			}

			Logger.Msg("SinkingIronMod loaded!");
		}
	}
}