using System;
using System.Reflection;
using WaterPumpMod;
using Utils;
using Sonity;
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
		
		public static float DifficultyID(this GameFlowController wall)
		{
			return wall.GetField<int>("difficultyID");
		}
		
		public static void ChangeWeaponPublic(this WeaponSwitcher switcher)
		{
			MethodInfo methodInfo = typeof(WeaponSwitcher).GetMethod("ChangeWeapon", 
				BindingFlags.Instance | BindingFlags.NonPublic, 
				null, 
				new Type[] { }, 
				null);
			if (methodInfo != null)
			{
				methodInfo.Invoke(switcher, new object[] { });
			}
			else
			{
				MainMod.Logger.Error("Could not find WeaponPublic method.");
			}
		}
		
		public static GameObject[] GetWeapons(this WeaponSwitcher wall)
		{
			return wall.GetField<GameObject[]>("weapons");
		}
		
		public static bool SetWeapons(this WeaponSwitcher wall, GameObject[] weapons)
		{
			return wall.SetField<GameObject[]>("weapons", weapons);
		}
		
		public static int WeaponEquipped(this WeaponSwitcher wall)
		{
			return wall.GetField<int>("weaponEquipped");
		}
		
		public static bool SetWeaponEquipped(this WeaponSwitcher wall, int index)
		{
			return wall.SetField<int>("weaponEquipped", index);
		}
		
		public static MonsterEye MonsterEye(this EyeLightButton wall)
		{
			return wall.GetField<MonsterEye>("monsterEye");
		}
		
		public static bool CanFlashMonster(this EyeLightButton wall)
		{
			return wall.GetField<bool>("canFlashMonster");
		}

		public static Camera Cam(this Tool wall)
		{
			return wall.GetField<Camera>("cam");
		}

		public static float InteractionRange(this Tool wall)
		{
			return wall.GetField<float>("interactionRange");
		}

		public static SoundEvent SFX_Welder_Blank(this Tool wall)
		{
			return wall.GetField<SoundEvent>("SFX_Welder_Blank");
		}

		public static void SetSFX_Welder_Blank(this Tool wall, SoundEvent sfx)
		{
			wall.SetField<SoundEvent>("SFX_Welder_Blank", sfx);
		}
	}
}