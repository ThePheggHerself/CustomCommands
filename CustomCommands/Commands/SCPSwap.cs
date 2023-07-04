using CommandSystem;
using PlayerRoles;
using PluginAPI.Core;
using RemoteAdmin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomCommands.Commands
{
	[CommandHandler(typeof(ClientCommandHandler))]
	public class SCPSwap : ICommand, IUsageProvider
	{
		public string Command => "scpswap";

		public string[] Aliases { get; } = { "sswap" };
		public string Description => "Changes your current SCP";

		public string[] Usage { get; } = { "scp" };

		public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
		{
			if (!Extensions.CanRun(arguments, Usage, out response))
			{
				return false;
			}

			if (sender is PlayerCommandSender pSender && pSender.ReferenceHub.IsSCP() && pSender.ReferenceHub.roleManager.CurrentRole.RoleTypeId != RoleTypeId.Scp0492)
			{
				var player = Player.Get(pSender.ReferenceHub);

				if (player.Health != player.MaxHealth)
				{
					response = "You cannot swap as you have taken damage";
					return false;
				}
				if (Round.Duration > TimeSpan.FromMinutes(1))
				{
					response = "You can only swap your SCP within the first minute of a round";
					return false;
				}

				var role = Extensions.GetRoleFromString(arguments.Array[1]);

				if (!Player.GetPlayers().Where(r => r.Role == role).Any())
				{
					player.SetRole(role);
					response = "You have now swapped SCP";
					return true;
				}
				else
				{
					response = "There is already a player playing as that SCP";
					return false;
				}
			}

			response = "You must be an SCP to run this command";
			return false;
		}
	}

	[CommandHandler(typeof(ClientCommandHandler))]
	public class SCPList : ICommand
	{
		public string Command => "scplist";

		public string[] Aliases { get; } = { "slist" };
		public string Description => "Lists all current SCPs";

		public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
		{
			if (sender is PlayerCommandSender pSender && pSender.ReferenceHub.IsSCP())
			{
				var scps = Player.GetPlayers().Where(r => r.IsSCP);

				if (!scps.Any())
				{
					response = "There are no other SCPs";
					return false;
				}

				List<string> scpString = new List<string>();
				foreach(var a in scps)
				{
					scpString.Add(a.Role.ToString().ToLower().Replace("scp", string.Empty));
				}

				response = $"Current SCPs: {string.Join(", ", scpString)}";
				return true;
			}

			response = "You must be an SCP to run this command";
			return false;
		}
	}
}
