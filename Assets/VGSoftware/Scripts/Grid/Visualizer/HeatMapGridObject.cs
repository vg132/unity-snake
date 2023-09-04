using UnityEngine;

namespace Assets.VGSoftware.Scripts.Grid.Visualizer
{
	public class HeatMapGridObject : BaseGridObject<HeatMapGridObject>
	{
		private const int MIN = 0;
		private const int MAX = 100;

		private int _value;

		public HeatMapGridObject(Grid<HeatMapGridObject> grid, int x, int y)
			: base(grid, x, y)
		{
		}

		public void AddValue(int value)
		{
			_value += value;
			_value = Mathf.Clamp(_value, MIN, MAX);
			_grid.TriggerGridObjectChanged(X, Y);
		}

		public float GetValueNormalized() => (float)_value / MAX;
		public override string ToString() => _value.ToString();
	}
}