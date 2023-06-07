using CommandSystem;
using NWAPIPermissionSystem;
using PlayerRoles;
using PluginAPI.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomCommands
{
	public static class Extensions
	{
		public static bool CanRun(ArraySegment<string> arg, string[] expectedArgs, out string response)
		{
			if (arg.Count < expectedArgs.Length)
			{
				response = $"Missing argument: {expectedArgs[arg.Count]}";
				return false;
			}

			response = string.Empty;
			return true;
		}

		public static bool CanRun(ICommandSender sender, PlayerPermissions? perm, ArraySegment<string> arg, string[] expectedArgs, out string response, out List<Player> players)
		{
			players = new List<Player>();

			if (perm != null && !sender.CheckPermission((PlayerPermissions)perm))
			{
				response = $"You do not have the required permission to execute this command: {perm}";
				return false;
			}

			if (arg.Count < expectedArgs.Length)
			{
				response = $"Missing argument: {expectedArgs[arg.Count]}";
				return false;
			}

			players = GetPlayersFromString(arg.Array[1]);
			if (players.Count < 1)
			{
				response = "No valid players found";
				return false;
			}

			response = string.Empty;
			return true;
		}

		public static bool CanRun(ICommandSender sender, PlayerPermissions? perm, ArraySegment<string> arg, string[] expectedArgs, out string response)
		{

			if (perm != null && !sender.CheckPermission((PlayerPermissions)perm))
			{
				response = $"You do not have the required permission to execute this command: {perm}";
				return false;
			}

			if (arg.Count < expectedArgs.Length)
			{
				response = $"Missing argument: {expectedArgs[arg.Count]}";
				return false;
			}

			response = string.Empty;
			return true;
		}

		public static bool CanRun(ICommandSender sender, string perm, ArraySegment<string> arg, string[] expectedArgs, out string response, out List<Player> players)
		{
			players = new List<Player>();

			if (perm != null && !PermissionHandler.CheckPermission(sender, perm))
			{
				response = $"You do not have the required permission to execute this command: {perm}";
				return false;
			}

			if (arg.Count < expectedArgs.Length)
			{
				response = $"Missing argument: {expectedArgs[arg.Count]}";
				return false;
			}

			players = GetPlayersFromString(arg.Array[1]);
			if (players.Count < 1)
			{
				response = "No valid players found";
				return false;
			}

			response = string.Empty;
			return true;
		}

		public static bool CanRun(ICommandSender sender, ArraySegment<string> arg, string[] expectedArgs, out string response, out List<Player> players)
		{
			players = new List<Player>();

			if (arg.Count < expectedArgs.Length)
			{
				response = $"Missing argument: {expectedArgs[arg.Count]}";
				return false;
			}

			players = GetPlayersFromString(arg.Array[1]);
			if (players.Count < 1)
			{
				response = "No valid players found";
				return false;
			}

			response = string.Empty;
			return true;
		}

		internal static List<Player> GetPlayersFromString(string users)
		{
			var allPlayers = Server.GetPlayers();
			if (users.ToLower() == "*")
				return allPlayers.Where(p => !p.IsServer).ToList();

			string[] playerStrings = users.Split('.');
			List<Player> playerList = new List<Player>();

			foreach (string plrStr in playerStrings)
			{
				Player plr = null;
				if (int.TryParse(plrStr, out int id) && Player.TryGet(id, out plr))
					playerList.Add(plr);
				else if (Player.TryGet(plrStr, out plr))
					playerList.Add(plr);
				else if (Player.TryGetByName(plrStr, out plr))
					playerList.Add(plr);
			}

			return playerList;
		}

		public static RoleTypeId GetRoleFromString(string role)
		{
			switch (role.ToLower().Replace("scp", string.Empty))
			{
				case "049":
					return RoleTypeId.Scp049;
				case "079":
					return RoleTypeId.Scp079;
				case "939":
					return RoleTypeId.Scp939;
				case "173":
					return RoleTypeId.Scp173;
				case "096":
					return RoleTypeId.Scp096;
				case "106":
				default:
					return RoleTypeId.Scp106;
			}
		}
	}
}
