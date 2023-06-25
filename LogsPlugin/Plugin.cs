using MelonLoader;
using UnityEngine;

[assembly: MelonInfo(typeof(Plugin), "Logs Plugin", "1.0.0", "Sneak")]

public class Plugin : MelonPlugin
{
	public override void OnPreModsLoaded()
	{
		Application.logMessageReceived += Logs;
		LoggerInstance.Msg("Logs Plugin loaded!");
	}

	private void Logs(string condition, string stacktrace, LogType type)
	{
		if (type == LogType.Error || type == LogType.Exception)
		{
			LoggerInstance.Error($"[{type}] {condition}\n{stacktrace}");
		}
		else if (type == LogType.Warning)
		{
			LoggerInstance.Warning($"[{type}] {condition}");
		}
		else
		{
			LoggerInstance.Msg($"[{type}] {condition}");
		}
	}
}