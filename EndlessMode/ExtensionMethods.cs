using SinkingIronMod2;
using UnityEngine;

namespace EndlessModeExtensions
{
	public static class ExtensionMethods
	{
		public static GameObject DifficultyPanel(this MainMenuDisplay display)
		{
			return display.GetField<GameObject>("difficultyPanel");
		}
		
		public static GameObject SkipProloguePanel(this MainMenuDisplay display)
		{
			return display.GetField<GameObject>("skipProloguePanel");
		}
		
		public static GameObject Canvas(this MainMenuDisplay display)
		{
			return display?.GetField<GameObject>("canvas");
		}
		
		public static float WallHealth(this FixableWall wall)
		{
			return wall.GetField<float>("wallHealth");
		}
	}
}