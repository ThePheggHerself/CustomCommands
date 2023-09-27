using CommandSystem;
using InventorySystem;
using InventorySystem.Items.Firearms;
using InventorySystem.Items.Firearms.Attachments;
using MapGeneration;
using MapGeneration.Distributors;
using Mirror;
using PluginAPI.Core;
using RemoteAdmin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CustomCommands.Commands
{
	[CommandHandler(typeof(RemoteAdminCommandHandler))]
	[CommandHandler(typeof(ClientCommandHandler))]
	[CommandHandler(typeof(GameConsoleCommandHandler))]
	public class TestCommand : ICommand, IUsageProvider
	{
		public string Command => "nevergonna";

		public string[] Aliases => null;

		public string Description => "Test command. Try it :)";

		public string[] Usage => new[] { "give", "you", "up" };

		public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
		{
			//var room = RoomIdentifier.AllRoomIdentifiers.Where(r => r.Name == RoomName.Hcz939).First();
			//var lightPrefab = NetworkClient.prefabs.Where(r => r.Value.name == "Scp1853PedestalStructure Variant").First();

			//var lightGO = GameObject.Instantiate(lightPrefab.Value);
			//lightGO.transform.parent = room.transform;

			////lightGO.transform.localPosition =  new UnityEngine.Vector3(3.5f, 0, 5.5f);

			//lightGO.transform.localPosition = Vector3.zero;
			//lightGO.transform.localPosition += Vector3.forward * float.Parse(arguments.ElementAt(1));
			//lightGO.transform.localPosition += Vector3.right * float.Parse(arguments.ElementAt(2));

			//lightGO.transform.rotation = room.transform.rotation;
			//lightGO.transform.Rotate(0, -90, 0);
			//NetworkServer.Spawn(lightGO);

			//foreach(Component a in lightGO.GetComponents(typeof(Component))){
			//	Log.Info($"{a.name} | {a.GetType()}");
			//}

			response = $"Never gonna give you up,\nNever gonna let you down.\nNever gonna run around,\nAnd desert you.\nNever gonna make you cry,\nNever gonna say goodbye.\nNever gonna tell a lie,\nAnd hurt you.";
			return true;
		}
	}
}
