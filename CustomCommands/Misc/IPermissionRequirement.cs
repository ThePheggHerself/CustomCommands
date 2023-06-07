using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomCommands
{
	public interface IPermissionRequirement
	{
		string Permission { get; }
	}
}
