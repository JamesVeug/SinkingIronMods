using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using EndlessModeExtensions;
using WaterPumpMod;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Fire
{
	public class FireManager : MonoBehaviour
	{
		public static List<Fire> Fires;

		public static float MinTimeBetweenFires;
		public static float MaxTimeBetweenFires;
		
		private GameFlowController gameFlowController;

		private void Start()
		{
			MainMod.Logger.Msg("FireManager started!");
			GameManager.instance.GetComponent<GameEvents>().onChangeDifficulty += ChangeDifficulty;
			gameFlowController = GameObject.FindObjectOfType<GameFlowController>();
			
			StartCoroutine(SpawnFire());
		}

		private void OnDestroy()
		{
			GameManager.instance.GetComponent<GameEvents>().onChangeDifficulty -= ChangeDifficulty;
		}

		private IEnumerator SpawnFire()
		{
			MainMod.Logger.Msg("Spawning fire...");
			if (MaxTimeBetweenFires == 0)
			{
				MainMod.Logger.Msg("Waiting for difficulty to be set...");
				while (MaxTimeBetweenFires == 0)
				{
					yield return new WaitForSeconds(1);
				}
				
				MainMod.Logger.Msg("Difficulty set, continuing...");
			}
			
			float seconds = Random.Range(MinTimeBetweenFires, MaxTimeBetweenFires);
			List<Fire> allFire = new List<Fire>(Fires.Where((a)=>!a.gameObject.activeSelf));
			if (allFire.Count > 0)
			{
				int num = Random.Range(0, allFire.Count);
				allFire[num].gameObject.SetActive(true);
				MainMod.Logger.Msg($"Spawned fire {allFire[num].gameObject.name}");
			}
			else
			{
				MainMod.Logger.Msg("No inactive fires found, waiting...");
			}
			
			yield return new WaitForSeconds(seconds);
			StartCoroutine(SpawnFire());
		}

		private void ChangeDifficulty(float _1, float _2, float _3)
		{
			MainMod.Logger.Msg("Difficulty changed, updating fire spawn times...");
			switch (gameFlowController.DifficultyID())
			{
				case 0:
				case 1:
				case 2:
				case 3:
				case 4:
				case 5:
				case 6:
					MinTimeBetweenFires = 5f;
					MaxTimeBetweenFires = 7f;
					break;
				case 7:
				case 8:
					MinTimeBetweenFires = 7f;
					MaxTimeBetweenFires = 9f;
					break;
				case 9:
				case 10:
					MinTimeBetweenFires = 0;
					MaxTimeBetweenFires = 0;
					break;
			}
		}
	}
}