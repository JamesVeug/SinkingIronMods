using System;
using System.Collections.Generic;
using DebugMenu.Scripts.Popups;
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
		public static List<BaseWindow> AllWindows = new List<BaseWindow>();
		
		public override void OnInitializeMelon()
		{
			Instance = this;
			Logger = this.LoggerInstance;
			
			GenerateDebugWindows();

			Logger.Msg("SinkingIronMod loaded!");
		}

		private static void GenerateDebugWindows()
		{
			Type[] types = System.Reflection.Assembly.GetExecutingAssembly().GetTypes();
			for (int i = 0; i < types.Length; i++)
			{
				Type type = types[i];
				if (type.IsSubclassOf(typeof(BaseWindow)))
				{
					Plugin.Logger.Msg($"Made {type}!");
					AllWindows.Add((BaseWindow)Activator.CreateInstance(type));
				}
			}
		}

		public override void OnGUI()
		{
			for (int i = 0; i < AllWindows.Count; i++)
			{
				if (AllWindows[i].IsActive)
					AllWindows[i].OnWindowGUI();
			}
		}

		public T ToggleWindow<T>() where T : BaseWindow, new()
		{
			return (T)ToggleWindow(typeof(T));
		}

		public BaseWindow ToggleWindow(Type t)
		{
			for (int i = 0; i < AllWindows.Count; i++)
			{
				BaseWindow window = AllWindows[i];
				if (window.GetType() == t)
				{
					window.IsActive = !window.IsActive;
					return window;
				}
			}

			return null;
		}

		public T GetWindow<T>() where T : BaseWindow
		{
			for (int i = 0; i < AllWindows.Count; i++)
			{
				T window = (T)AllWindows[i];
				if (window.GetType() == typeof(T))
				{
					return window;
				}
			}

			return null;
		}
	}
}