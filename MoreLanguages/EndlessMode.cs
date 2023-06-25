using MelonLoader;
using SinkingIronMod2;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

[assembly: MelonInfo(typeof(MoreLanguages), "More Languages", "1.0.0", "Sneak")]
[assembly: MelonGame("Lixian Games", "Sinking Iron")]

namespace SinkingIronMod2
{
	public class MoreLanguages : MelonMod
	{
		public static MelonLogger.Instance Logger;

		public override void OnInitializeMelon()
		{
			Logger = this.LoggerInstance;

			Locale locale = LocalizationSettings.AvailableLocales.Locales[0];
			locale.

			LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[locale];
			
			Logger.Msg("Endless Mode loaded!");
		}
	}
}