using System.Collections.Generic;
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
	}
}