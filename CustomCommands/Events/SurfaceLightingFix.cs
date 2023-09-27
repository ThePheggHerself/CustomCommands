using AdminToys;
using MapGeneration;
using Mirror;
using PluginAPI.Core.Attributes;
using PluginAPI.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CustomCommands.Events
{
	public class SurfaceLightingFix
	{
		[PluginEvent(ServerEventType.WaitingForPlayers)] //Should work for waiting for players? If not then use round start
		public void SpawnLights()
		{
			var lightGO = GameObject.Instantiate(NetworkClient.prefabs.First(r => r.Value.name == "LightSourceToy").Value);
			lightGO.transform.position = new Vector3(135, 1024, -43);
			NetworkServer.Spawn(lightGO);
			lightGO.AddComponent<SurfaceLightComponent>();
			var LSToy = lightGO.GetComponent<LightSourceToy>();
			LSToy.NetworkLightIntensity = 50;
			LSToy.NetworkLightRange = 250;
			LSToy.NetworkLightColor = Color.white;
			LSToy.NetworkLightShadows = false;
		}
		public class SurfaceLightComponent : MonoBehaviour
		{
			private RoomLightController controller = RoomLightController.Instances.First(x => x.Room.Name == RoomName.Outside);
			private void Update()
			{
				if (NetworkServer.active)
				{
					gameObject.GetComponentInParent<LightSourceToy>().NetworkLightIntensity = controller.LightsEnabled ? 100 : 0;
					//gameObject.GetComponentInParent<LightSourceToy>().NetworkLightColor = controller.OverrideColor;
				}
			}
		}
	}
}
