using CommandSystem;
using Mirror;
using NWAPIPermissionSystem;
using PlayerRoles.PlayableScps.Scp079.Cameras;
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
	public enum VoteType
	{
		NONE = 0,
		AdminVote = 1,
		AutoEventVote = 2,
	}


	public class Plugin
	{
		public static bool EventInProgress => CurrentEvent != EventType.NONE;
		public static bool VoteInProgress => CurrentVote != VoteType.NONE;

		public static EventType CurrentEvent = EventType.NONE;
		public static VoteType CurrentVote = VoteType.NONE;
		public static string CurrentVoteString = string.Empty;

		public static void SetVote(VoteType type, string vStr)
		{
			CurrentVote = type;
			CurrentVoteString = vStr;
		}

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

		public static void EndVote()
		{
			if (!Plugin.VoteInProgress)
				return;

			int yes = 0;
			int no = 0;
			int nil = 0;

			foreach (var a in Player.GetPlayers())
			{
				if (a.TemporaryData.Contains("vote_yes"))
				{
					yes++;
					a.TemporaryData.Remove("vote_yes");
				}
				else if (a.TemporaryData.Contains("vote_no"))
				{
					no++;
					a.TemporaryData.Remove("vote_no");
				}
				else
					nil++;
			}

			Server.SendBroadcast($"The vote is over!\n<color=green>{yes} voted yes</color>, <color=red>{no} voted no</color>, and {nil} did not vote", 10);
			Plugin.SetVote(VoteType.NONE, string.Empty);
		}
	}
}
