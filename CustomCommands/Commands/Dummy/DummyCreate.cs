using CommandSystem;
using CustomCommands.Misc;
using MEC;
using PluginAPI.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CustomCommands.Commands.Dummy
{
	[CommandHandler(typeof(RemoteAdminCommandHandler))]
	public class DummyCreate : ICustomCommand
	{
		public string Command => "dummycreate";

		public string[] Aliases => null;

		public string Description => "Creates a dummy player";

		public string[] Usage { get; } = { "name" };

		public PlayerPermissions? Permission => null;
		public string PermissionString => "cuscom.dummyc";

		public bool RequirePlayerSender => false;

		public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
		{
			if (!sender.CanRun(this, arguments, out response, out var players, out var pSender))
				return false;

			var dummy = DummyManager.CreateDummy(arguments.ElementAt(0));

			response = $"Dummy '{dummy.authManager.UserId}' created";

			return true;
		}
	}
}
