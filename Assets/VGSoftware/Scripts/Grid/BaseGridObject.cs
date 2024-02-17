namespace Assets.VGSoftware.Scripts.Grid
{
	public abstract class BaseGridObject<TGridObject>
	{
		protected Grid<TGridObject> _grid;
		public int X { get; protected set; }
		public int Y { get; protected set; }

		public BaseGridObject(Grid<TGridObject> grid, int x, int y)
		{
			_grid = grid;
			X = x;
			Y = y;
		}

		public abstract bool IsDefaultState();

		protected virtual void TriggerGridObjectChanged()
		{
			_grid.TriggerGridObjectChanged(X, Y);
		}
	}
}