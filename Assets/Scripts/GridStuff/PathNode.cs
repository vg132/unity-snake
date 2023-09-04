
namespace Assets.Scripts.GridStuff
{
	public class PathNode : BaseGridObject<PathNode>
	{
		public int gCost { get; set; }
		public int hCost { get; set; }
		public int fCost => gCost + hCost;
		public PathNode CameFromNode { get; set; }
		public bool IsObsticle { get; set; }

		public PathNode(Grid<PathNode> grid, int x, int y)
			: base(grid, x, y)
		{
		}

		public override string ToString() => $"{X}, {Y}\n{IsObsticle}";
	}
}