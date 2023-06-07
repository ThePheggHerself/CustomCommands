using CommandSystem;
using NWAPIPermissionSystem;
using PluginAPI.Core;
using PluginAPI.Core.Attributes;
using PluginAPI.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Android;

namespace CustomCommands
{
	public class Plugin
	{
		public static bool EventInProgress = false;

		[PluginEntryPoint("Custom Commands", "1.0.0", "Simple plugin for custom commands", "ThePheggHerself")]
		public void OnPluginStart()
		{
			Log.Info($"Plugin is loading...");

			EventManager.RegisterEvents<Events>(this);
		}
    }
}
