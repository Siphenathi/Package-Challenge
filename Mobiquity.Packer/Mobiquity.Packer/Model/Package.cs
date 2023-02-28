using System.Collections.Generic;

namespace Mobiquity.Packer.Model
{
	public class Package
	{
		public int PackageLimit { get; set; }
		public List<Item> Items { get; set; }
	}

	public class Item
	{
		public int Index { get; set; }
		public double Weight { get; set; }
		public double Cost { get; set; }
	}
}