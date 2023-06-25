using System;
using WaterPumpMod;
using UnityEngine;

namespace Fire
{
	public class Fire : MonoBehaviour
	{
		private int index = 0;
		private SpriteRenderer spriteRenderer;
		private AudioSource audioSource;

		private void Awake()
		{
			spriteRenderer = GetComponent<SpriteRenderer>();
			if (spriteRenderer == null)
			{
				MainMod.Logger.Error($"Could not find SpriteRenderer on {gameObject.name}!");
			}

			audioSource = gameObject.AddComponent<AudioSource>();
			audioSource.clip = MainMod.FireSFX;
			audioSource.loop = true;
			audioSource.volume = 0.75f;
			audioSource.spatialize = true;
			audioSource.spatialBlend = 1f;
			audioSource.minDistance = 1f;
			audioSource.maxDistance = 10f;
			audioSource.Play();
		}

		void Update()
		{
			// Rotate this gameobject to face the camera by keep X rotation the same
			transform.LookAt(Camera.main.transform);
			transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);

			index++;
			if (index >= MainMod.FireSprites.Count)
			{
				index = 0;
			}
			spriteRenderer.sprite = MainMod.FireSprites[index];
		}
	}
}