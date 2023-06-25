using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace SinkingIronMod2
{
	public static class Utils
	{
		public static AssetBundle LoadAssetBundleFromEmbeddedResource(string name)
		{
			byte[] resources = GetResourceBytes(name, typeof(Utils).Assembly);
			AssetBundle bundle = AssetBundle.LoadFromMemory(resources);
			if (bundle == null)
			{
				Debug.LogError($"Tried getting asset bundle from bytes but failed! Is the path wrong?");
			}

			return bundle;
		}
		
		/// <summary>
		/// Reads the contents of an image file in an assembly and returns it as a byte array.
		/// </summary>
		/// <param name="pathCardArt">The name of the art file stored as a resource in the assembly.</param>
		/// <param name="target">The assembly to pull the art from.</param>
		/// <returns>The contents of the file in pathCardArt as a byte array.</returns>
		public static byte[] GetResourceBytes(string filename, Assembly target)
		{
			string lowerKey = $".{filename.ToLowerInvariant()}";
			string resourceName = target.GetManifestResourceNames().FirstOrDefault(r => r.ToLowerInvariant().EndsWith(lowerKey));

			if (string.IsNullOrEmpty(resourceName))
				throw new KeyNotFoundException($"Could not find resource matching {filename} in assembly {target}.");

			using (Stream resourceStream = target.GetManifestResourceStream(resourceName))
			{
				using (MemoryStream memStream = new MemoryStream())
				{
					resourceStream.CopyTo(memStream);
					return memStream.ToArray();
				}
			}
		}
		
		public static T GetField<T>(this object obj, string name) {
			// Set the flags so that private and public fields from instances will be found
			BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
			FieldInfo? field = obj.GetType().GetField(name, bindingFlags);
			object? value = field?.GetValue(obj);
			return (T)value;
		}

		public static Transform FindChild(Transform o, string name, bool allowDisabled)
		{
			foreach (Transform child in o.transform)
			{
				if (!allowDisabled && !child.gameObject.activeSelf)
				{
					continue;
				}
				
				if (child.name == name)
				{
					return child;
				}

				Transform c = FindChild(child, name, allowDisabled);
				if (c != null)
				{
					return c;
				}
			}

			return null;
		}
		
		public static GameObject FindGameObject(params string[] names)
		{
			GameObject gameObject = null;
			for (int i = 0; i < names.Length; i++)
			{
				string name = names[i];
				if (gameObject == null)
				{
					gameObject = GameObject.Find(name);
					if (gameObject == null)
					{
						EndlessMode.Logger.Error("Could not find GameObject using initial name: " + name);
						return null;
					}
				}
				else
				{
					Transform child = FindChild(gameObject.transform, name, true);
					if (child == null)
					{
						EndlessMode.Logger.Error($"Could not find child from path {string.Join("->", names.SubArray(0, i))})");
						return null;
					}

					gameObject = child.gameObject;
				}
			}

			return gameObject;
		}

		public static T[] SubArray<T>(this T[] array, int startIndex, int endIndex)
		{
			T[] subArray = new T[endIndex - startIndex];
			Array.Copy(array, startIndex, subArray, 0, endIndex - startIndex);
			return subArray;
		}

		public static void LogGameObject(GameObject gameObject)
		{
			string log = $"GameObject: {gameObject.name}\n";

			try
			{
				foreach (Behaviour behaviour in gameObject.GetComponents<Behaviour>())
				{
					log += $"\tBehaviour: {behaviour.GetType().Name}\n";
					FieldInfo[] fields = behaviour.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
					foreach (FieldInfo field in fields)
					{
						log += $"\t\tField: {field.Name} = {field.GetValue(behaviour)}\n";
					}
				}
			}
			catch (Exception e)
			{
				EndlessMode.Logger.Error(e);
			}
			
			EndlessMode.Logger.Msg(log);
		}
	}
}