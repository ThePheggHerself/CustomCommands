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
	public class NameFix
	{
		[PluginEvent(ServerEventType.PlayerJoined), PluginPriority(LoadPriority.Highest)]
		public void PlayerJoined(PlayerJoinedEvent args)
		{
			if (args.Player.Nickname.Contains("u003c") || args.Player.Nickname.Contains("u003e"))
			{
				Log.Info($"{args.Player.Nickname} contains invalid characters and has been kicked from the server");
				args.Player.Kick("Invalid characters in username");
				return;
			}
		}
	}
}
