using CommandSystem;
using CustomCommands.Events;
using Hints;
using InventorySystem.Items;
using InventorySystem.Items.Armor;
using InventorySystem.Items.Firearms;
using MapGeneration;
using Mirror;
using PlayerRoles;
using PlayerRoles.PlayableScps.Scp079;
using PlayerRoles.RoleAssign;
using PluginAPI.Core;
using PluginAPI.Core.Attributes;
using PluginAPI.Core.Items;
using PluginAPI.Enums;
using PluginAPI.Events;
using RemoteAdmin;
using RoundRestarting;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CustomCommands.Features
{
	public class _079Removal
	{
		[PluginEvent]
		public void SpawnEvent(PlayerSpawnEvent args)
		{
			var scps = SCPSwap.AvailableSCPs;

			if (args.Role == RoleTypeId.Scp079 && scps.Any())
			{
				args.Player.SetRole(scps[new System.Random().Next(0, scps.Length)], RoleChangeReason.LateJoin);
			}
				
		}
	}
}
