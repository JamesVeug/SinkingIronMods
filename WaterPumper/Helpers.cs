using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;
using WaterPumpMod;

namespace Utils
{
	public static class Helpers
	{
		public static AssetBundle LoadAssetBundleFromEmbeddedResource(string name)
		{
			byte[] resources = GetResourceBytes(name, typeof(Helpers).Assembly);
			AssetBundle bundle = AssetBundle.LoadFromMemory(resources);
			if (bundle == null)
			{
				Debug.LogError($"Tried getting asset bundle from bytes but failed! Is the path wrong?");
			}

			return bundle;
		}
		public static Texture2D LoadTextureFromEmbeddedResource(string filename)
		{
			Texture2D texture = new Texture2D(2, 2, TextureFormat.RGBA32, false);
			byte[] imgBytes = GetResourceBytes(filename, typeof(Helpers).Assembly);
			bool isLoaded = texture.LoadImage(imgBytes);
			if (!isLoaded)
			{
				Debug.LogError($"Tried loading image {filename} from bytes but failed!");
			}
			return texture;
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
			if (obj == null)
			{
				MainMod.Logger.Error($"Could not get field {name} from null!");
				return default(T);
			}
			
			// Set the flags so that private and public fields from instances will be found
			BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
			FieldInfo field = obj.GetType().GetField(name, bindingFlags);
			if (field == null)
			{
				MainMod.Logger.Error($"Could not get field {name} for type {obj.GetType()}");
				return default(T);
			}
			
			object? value = field?.GetValue(obj);
			return (T)value;
		}
		
		public static bool SetField<T>(this object obj, string name, T value) {
			// Set the flags so that private and public fields from instances will be found
			BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
			FieldInfo field = obj.GetType().GetField(name, bindingFlags);
			if (field == null)
			{
				MainMod.Logger.Error($"Could not get field {name} for type {obj.GetType()}");
				return false;
			}

			try
			{
				field.SetValue(obj, value);
				return true;
			}
			catch (Exception e)
			{
				MainMod.Logger.Error(e);
				return false;
			}
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
						MainMod.Logger.Error("Could not find GameObject using initial name: " + name);
						return null;
					}
				}
				else
				{
					Transform child = FindChild(gameObject.transform, name, true);
					if (child == null)
					{
						MainMod.Logger.Error($"Could not find child from path {string.Join("->", names.SubArray(0, i))})");
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

		public static void LogGameObject(GameObject gameObject, bool logFields, bool logChildren=false, string prefix = "")
		{
			string log = $"{prefix}GameObject: {gameObject.name}\n";

			try
			{
				foreach (Behaviour behaviour in gameObject.GetComponents<Behaviour>())
				{
					log += $"{prefix}-Behaviour: {behaviour.GetType().Name}\n";
					if (logFields)
					{
						FieldInfo[] fields = behaviour.GetType()
							.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
						foreach (FieldInfo field in fields)
						{
							log += $"{prefix}--Field: {field.Name} = {field.GetValue(behaviour)}\n";
						}
					}
				}
			}
			catch (Exception e)
			{
				MainMod.Logger.Error(e);
			}
			
			MainMod.Logger.Msg(log);

			if (logChildren)
			{
				foreach (Transform child in gameObject.transform)
				{
					LogGameObject(child.gameObject, logFields, logChildren, prefix + "\t");
				}
			}
		}
	}
}