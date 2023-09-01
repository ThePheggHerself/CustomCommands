using PlayerRoles;
using PluginAPI.Core.Attributes;
using PluginAPI.Core;
using PluginAPI.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdminToys;
using InventorySystem.Items.Firearms;
using PlayerStatsSystem;
using InventorySystem.Items.Firearms.Attachments;
using Mirror;
using UnityEngine;
using PluginAPI.Events;
using CustomPlayerEffects;
using InventorySystem.Items;
using InventorySystem;
using InventorySystem.Items.Usables;
using Utils;

namespace CustomCommands
{
	public class Fixes
	{
		[PluginEvent(ServerEventType.PlayerJoined)]
		public void PlayerJoin(PlayerJoinedEvent args)
		{
			if (!Plugin.EventInProgress && Round.IsRoundStarted && Round.Duration.TotalSeconds < 30)
				args.Player.SetRole(RoleTypeId.ClassD, RoleChangeReason.LateJoin);

			if (Plugin.VoteInProgress)
			{
				args.Player.SendBroadcast($"<b><color=#fa886b>[VOTE]</color></b> <color=#79b7ed>{Plugin.CurrentVoteString}</color>\nUse your console to vote now!", 15);
				args.Player.SendConsoleMessage("A vote has been started. Run the `.yes` command to vote yes, or `.no` command to vote no");
			}
		}

		[PluginEvent(ServerEventType.PlayerSpawn)]
		public void PlayerSpawn(PlayerSpawnEvent args)
		{
			if (args.Player.IsSCP && Round.Duration < TimeSpan.FromMinutes(1) && !Plugin.EventInProgress)
			{
				args.Player.SendBroadcast("You can change your SCP by using the \".scpswap\" command in your console", 6);
			}
		}

		[PluginEvent(ServerEventType.PlayerDamagedShootingTarget)]
		public void TargetDamagedEvent(PlayerDamagedShootingTargetEvent args)
		{
			if (args.Player.CurrentItem is Firearm firearm)
			{
				Log.Info($"{firearm.GetCurrentAttachmentsCode()}");
			}
		}

		[PluginEvent(ServerEventType.PlayerHandcuff)]
		public bool OnDisarm(PlayerHandcuffEvent args)
		{
			if (args.Target.Role == RoleTypeId.Tutorial)
				return false;
			else
				return true;
		}

		//[PluginEvent(ServerEventType.WaitingForPlayers)]
		public void WaitingForPlayers()
		{
			foreach (var room in RoomLightController.Instances)
			{
				room.NetworkOverrideColor = new UnityEngine.Color(0.4f, 0.4f, 0.4f);
			}

			var lightPrefab = NetworkClient.prefabs.Where(r => r.Value.name == "LightSourceToy").First();

			var lightGO = GameObject.Instantiate(lightPrefab.Value);
			lightGO.transform.position = new UnityEngine.Vector3(135, 995, -50);
			lightGO.GetComponent<Light>().intensity = 100;
			NetworkServer.Spawn(lightGO);

			lightGO = GameObject.Instantiate(lightPrefab.Value);
			lightGO.transform.position = new UnityEngine.Vector3(135, 995, -48);
			lightGO.GetComponent<Light>().intensity = 100;
			NetworkServer.Spawn(lightGO);

			lightGO = GameObject.Instantiate(lightPrefab.Value);
			lightGO.transform.position = new UnityEngine.Vector3(135, 995, -46);
			lightGO.GetComponent<Light>().intensity = 100;
			NetworkServer.Spawn(lightGO);

			lightGO = GameObject.Instantiate(lightPrefab.Value);
			lightGO.transform.position = new UnityEngine.Vector3(135, 995, -44);
			lightGO.GetComponent<Light>().intensity = 100;
			NetworkServer.Spawn(lightGO);

			lightGO = GameObject.Instantiate(lightPrefab.Value);
			lightGO.transform.position = new UnityEngine.Vector3(135, 995, -42);
			lightGO.GetComponent<Light>().intensity = 100;
			NetworkServer.Spawn(lightGO);

			lightGO = GameObject.Instantiate(lightPrefab.Value);
			lightGO.transform.position = new UnityEngine.Vector3(135, 995, -40);
			lightGO.GetComponent<Light>().intensity = 100;
			NetworkServer.Spawn(lightGO);

			lightGO = GameObject.Instantiate(lightPrefab.Value);
			lightGO.transform.position = new UnityEngine.Vector3(135, 995, -38);
			lightGO.GetComponent<Light>().intensity = 100;
			NetworkServer.Spawn(lightGO);
		}
	}
}
