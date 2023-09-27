using PlayerRoles;
using PluginAPI.Core;
using PluginAPI.Core.Attributes;
using PluginAPI.Enums;
using PluginAPI.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomCommands.Events
{
	public class SCPSwap
	{
		[PluginEvent(ServerEventType.PlayerSpawn)]
		public void PlayerSpawn(PlayerSpawnEvent args)
		{
			if (args.Player.Role.IsValidSCP() && Round.Duration < TimeSpan.FromMinutes(1) && !Plugin.EventInProgress)
			{
				args.Player.SendBroadcast("You can change your SCP by using the \".scpswap\" command in your console", 6);
			}
		}
	}
}
