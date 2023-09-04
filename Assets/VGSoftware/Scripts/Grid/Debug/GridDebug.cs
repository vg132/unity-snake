using Assets.Scripts.Utils;
using TMPro;
using UnityEngine;

namespace Assets.VGSoftware.Scripts.Grid
{
	public class GridDebug<TGridObject>
	{
		private Grid<TGridObject> _grid;
		private TextMeshPro[,] _debugText;

		public GridDebug(Grid<TGridObject> grid)
		{
			_grid = grid;
			_debugText = new TextMeshPro[_grid.Width, _grid.Height];

			for (int x = 0; x < _grid.Width; x++)
			{
				for (int y = 0; y < _grid.Height; y++)
				{
					_debugText[x, y] = Utilities.CreateWorldTextPro(_grid.GetGridItem(x,y)?.ToString(), null, _grid.GetWorldPosition(x, y) + new Vector3(_grid.CellSize, _grid.CellSize) * 0.5f, 20, Color.white);
					Debug.DrawLine(_grid.GetWorldPosition(x, y), _grid.GetWorldPosition(x, y + 1), Color.white, 100f);
					Debug.DrawLine(_grid.GetWorldPosition(x, y), _grid.GetWorldPosition(x + 1, y), Color.white, 100f);
				}
			}
			Debug.DrawLine(_grid.GetWorldPosition(0, _grid.Height), _grid.GetWorldPosition(_grid.Width, _grid.Height), Color.white, 100f);
			Debug.DrawLine(_grid.GetWorldPosition(_grid.Width, _grid.Height), _grid.GetWorldPosition(_grid.Width, 0), Color.white, 100f);

			_grid.OnGridValueChanged += Grid_OnGridValueChanged;
		}

		private void Grid_OnGridValueChanged(object sender, OnGridValueChangedEventArgs<TGridObject> e)
		{
			_debugText[e.X, e.Y].text = e.Value?.ToString();
		}
	}
}