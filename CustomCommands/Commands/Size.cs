using CommandSystem;
using Mirror;
using PluginAPI.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CustomCommands.Commands
{
	//[CommandHandler(typeof(RemoteAdminCommandHandler))]
	public class Size : CommandSystem.ICommand, IUsageProvider
	{
		public string Command => "size";

		public string[] Aliases { get; } = { };
		public string Description => "Modify the size of a specified player";
		public string[] Usage { get; } = { "%player%", "x", "y", "z" };

		public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
		{
			if (!Extensions.CanRun(sender, PlayerPermissions.Noclip, arguments, Usage, out response, out List<Player> Players))
				return false;

			if (!float.TryParse(arguments.Array[2], out float x) || !float.TryParse(arguments.Array[2], out float y) || !float.TryParse(arguments.Array[2], out float z))
			{
				response = "Valid scale not provided";
				return false;
			}

			var svrPlrs = Server.GetPlayers();

			foreach(var p in Players)
			{
				var nId = p.ReferenceHub.networkIdentity;
				p.ReferenceHub.gameObject.transform.localScale = new UnityEngine.Vector3(1 * x, 1 * y, 1 * z);
				ObjectDestroyMessage dMsg = new ObjectDestroyMessage { netId = nId.netId };

				foreach(var player in svrPlrs)
				{
					NetworkConnection nConn = player.ReferenceHub.connectionToClient;

					if (player.UserId != p.UserId)
						nConn.Send(dMsg, 0);

					typeof(NetworkServer).GetMethod("SendSpawnMessage", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { nId, nConn });
				}
			}

			response = $"Scale of {Players.Count} {(Players.Count != 1 ? "players" : "player")} has been set to {x}, {y}, {z}";
			return true;
		}
	}
}
