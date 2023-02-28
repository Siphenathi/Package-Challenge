using System.Collections.Generic;

namespace Mobiquity.Packer.Model
{
	public class Package
	{
		public int PackageLimit { get; set; }
		public List<Item> Items { get; set; }
	}
}