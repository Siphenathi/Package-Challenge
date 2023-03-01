using System.Collections.Generic;

namespace Mobiquity.Packer.Model
{
	public class Package
	{
		public int PackageWeightLimit { get; set; }
		public List<Item> Items { get; set; }
	}
}