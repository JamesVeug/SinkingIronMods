using MelonLoader;
using SinkingIronMod2;
using UnityEngine;
using UnityEngine.UI;

[assembly: MelonInfo(typeof(EndlessMode), "Endless Mode", "1.0.0", "Sneak")]
[assembly: MelonGame("Lixian Games", "Sinking Iron")]

namespace SinkingIronMod2
{
	public class EndlessMode : MelonMod
	{
		public static bool IsEndlessMode = true;
		
		public static MelonLogger.Instance Logger;
		public static EndlessMode Instance;
		public static AssetBundle AssetBundle;
		public static int Depth;
		public static int Repairs;
		public static int LeaksFixed;

		public override void OnInitializeMelon()
		{
			Instance = this;
			Logger = this.LoggerInstance;
			
			AssetBundle = Utils.LoadAssetBundleFromEmbeddedResource("endless");
			if(AssetBundle == null)
			{
				Logger.Error("Could not load AssetBundle!");
			}

			Logger.Msg("Endless Mode loaded!");
		}
	}
}