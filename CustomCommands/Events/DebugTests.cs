using InventorySystem.Items.Firearms;
using InventorySystem.Items.Firearms.Attachments;
using MapGeneration;
using Mirror;
using PlayerRoles.PlayableScps.Scp079;
using PluginAPI.Core;
using PluginAPI.Core.Attributes;
using PluginAPI.Enums;
using PluginAPI.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CustomCommands.Events
{
	public class DebugTests
	{
		[PluginEvent(ServerEventType.PlayerDamagedShootingTarget)]
		public void TargetDamagedEvent(PlayerDamagedShootingTargetEvent args)
		{
			if (args.Player.CurrentItem is Firearm firearm)
			{
				Log.Info($"{firearm.GetCurrentAttachmentsCode()}");
			}
		}

		//[PluginEvent(ServerEventType.MapGenerated)]
		public void MapGenEvent(MapGeneratedEvent args)
		{
			foreach(var a in NetworkClient.prefabs)
			{
				Log.Info($"{a.Value.name}");
			}
		}
	}
}
