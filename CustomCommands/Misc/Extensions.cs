using CommandSystem;
using NWAPIPermissionSystem;
using PlayerRoles;
using PluginAPI.Core;
using RemoteAdmin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace CustomCommands
{
	public static class Extensions
	{
		public static bool CanRun(this ICommandSender sender, ICustomCommand cmd, ArraySegment<string> args, out string Response, out List<Player> Players, out PlayerCommandSender pSender)
		{
			Players = new List<Player>();
			pSender = null;
			if (cmd.RequirePlayerSender && !(sender is PlayerCommandSender pS))
			{
				Response = "You must be a player to run this command";
				return false;
			}

			if (cmd.Permission != null && !sender.CheckPermission((PlayerPermissions)cmd.Permission))
			{
				Response = $"You do not have the required permission to execute this command: {cmd.Permission}";
				return false;
			}
			else if (!string.IsNullOrEmpty(cmd.PermissionString) && !sender.CheckPermission(cmd.PermissionString))
			{
				Response = $"You do not have the required permission to execute this command: {cmd.PermissionString}";
				return false;
			}

			if (args.Count < cmd.Usage.Length)
			{
				Response = $"Missing argument: {cmd.Usage[args.Count]}";
				return false;
			}

			if (cmd.Usage.Contains("%player%"))
			{
				var index = cmd.Usage.IndexOf("%player%");

				var hubs = RAUtils.ProcessPlayerIdOrNamesList(args, index, out string[] newArgs, false);

				if (hubs.Count < 1)
				{
					Response = $"No player(s) found for: {args.ElementAt(index)}";
					return false;
				}
				else
				{
					foreach (var plr in hubs)
					{
						Players.Add(Player.Get(plr));
					}
				}
			}

			Response = string.Empty;
			return true;
		}

        public static RoleTypeId[] ValidSwapSCP = new RoleTypeId[]
        {
            RoleTypeId.Scp173, RoleTypeId.Scp049, RoleTypeId.Scp079,RoleTypeId.Scp096, RoleTypeId.Scp106,RoleTypeId.Scp939, RoleTypeId.Scp3114
        };

		public static RoleTypeId GetRoleFromString(string role)
		{
			var roles = Enum.GetNames(typeof(RoleTypeId));

			if (Enum.TryParse(role, true, out RoleTypeId roleType))
			{
                if(!IsValidSCP(roleType))
                    return RoleTypeId.None;

				return roleType;
			}
			else return RoleTypeId.None;
		}

		public static bool IsValidSCP(this RoleTypeId role)
		{
			return ValidSwapSCP.Contains(role);
		}
	}
}
