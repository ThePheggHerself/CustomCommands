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
	public class LateJoin
	{
		[PluginEvent(ServerEventType.PlayerJoined), PluginPriority(LoadPriority.Low)]
		public void PlayerJoin(PlayerJoinedEvent args)
		{
			if (!Plugin.EventInProgress && Round.IsRoundStarted && Round.Duration.TotalSeconds < 30)
				args.Player.SetRole(RoleTypeId.ClassD, RoleChangeReason.LateJoin);
		}
	}
}
