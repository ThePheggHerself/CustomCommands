using CommandSystem;
using Mirror;
using NWAPIPermissionSystem;
using PlayerRoles.Ragdolls;
using PluginAPI.Core;
using PluginAPI.Core.Attributes;
using PluginAPI.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Android;
using Utils;

namespace CustomCommands
{
	public enum EventType
	{
		NONE = 0,

		Infection = 1,
		Battle = 2,
		Hush = 3
	}


	public class Plugin
	{
		public static bool EventInProgress => CurrentEvent != EventType.NONE;
		public static EventType CurrentEvent = EventType.NONE;

		[PluginEntryPoint("Custom Commands", "1.0.0", "Simple plugin for custom commands", "ThePheggHerself")]
		public void OnPluginStart()
		{
			Log.Info($"Plugin is loading...");

			EventManager.RegisterEvents<Events>(this);
			EventManager.RegisterEvents<Fixes>(this);

			//RagdollManager.OnRagdollSpawned += RagdollManager_OnRagdollSpawned;
		}

		private void RagdollManager_OnRagdollSpawned(BasicRagdoll obj)
		{
			MEC.Timing.CallDelayed(5, () =>
			{
				//ExplosionUtils.ServerExplode(obj.transform.position, new Footprinting.Footprint(obj.Info.OwnerHub));

				ExplosionUtils.ServerSpawnEffect(obj.CenterPoint.position, ItemType.GrenadeHE);
				NetworkServer.Destroy(obj.gameObject);
			});
		}
	}
}
