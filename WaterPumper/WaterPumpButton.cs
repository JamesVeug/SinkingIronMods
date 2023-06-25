using Fire;
using UnityEngine;
using Utils.Extensions;

namespace WaterPumpMod
{

	public class WaterPumpButton : MonoBehaviour, IInteractable
	{
		float drainSpeed = 0.01f;
		
		private InteractionSystem interactionSystem;

		private bool pumping = false;

		private void Start()
		{
			interactionSystem = GameObject.FindObjectOfType<InteractionSystem>();
			if (interactionSystem == null)
			{
				MainMod.Logger.Error($"No interaction system found");
			}
			else
			{
				MainMod.Logger.Msg($"Found interaction system");

			}
		}

		public void Interact()
		{
			WaterRisingController waterRisingController = MainMod.WaterRisingController;
			Vector3 position = waterRisingController.transform.position;
			if (position.y > MainMod.WaterStartHeight)
			{
				StartPump();
				MainMod.Logger.Msg($"Pumping");
			}
			else
			{
				MainMod.Logger.Msg($"No water to pump!");
			}
		}

		private void Update()
		{
			if (pumping)
			{
				if (Input.GetKeyUp(KeyCode.E))
				{
					StopPump();
					MainMod.Logger.Msg($"Stopped pumping");
					return;
				}

				RaycastHit raycastHit = interactionSystem.RaycastHit();
				if (raycastHit.collider == null || raycastHit.collider.gameObject != gameObject)
				{
					StopPump();
					MainMod.Logger.Msg($"No longer looking at pump");
					return;
				}
				
				// Pumping
				WaterRisingController waterRisingController = MainMod.WaterRisingController;
				Vector3 position = waterRisingController.transform.position;
				float newHeight = Mathf.Max(MainMod.WaterStartHeight, position.y - drainSpeed * Time.deltaTime);
				position.y = newHeight;
				waterRisingController.transform.position = position;

				if (newHeight <= MainMod.WaterStartHeight)
				{
					StopPump();
					MainMod.Logger.Msg($"All water pumped!");
					return;
				}
			}
		}

		private void StartPump()
		{
			pumping = true;
			
			SFX_Patches.EngineSFX.Play2D();
		}

		private void StopPump()
		{
			pumping = false;
			SFX_Patches.EngineSFX?.Stop2D();
			SFX_Patches.SteamBurstSFX?.Play2D();
		}

		public string Description()
		{
			return "Drain Water";
		}
	}
}