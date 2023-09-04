using System;
using UnityEngine;

namespace Assets.Scripts.GridStuff
{
	public partial class Grid<TGridObject>
	{
		public event EventHandler<OnGridValueChangedEventArgs<TGridObject>> OnGridValueChanged;

		public int Width { get; private set; }
		public int Height { get; private set; }
		public float CellSize { get; private set; }
		private Vector3 _originPosition;
		private TGridObject[,] _grid;

		public Grid(int width, int height, float cellSize, Vector3 originPosition = default, Func<Grid<TGridObject>, int, int, TGridObject> createGridObject = null)
		{
			Width = width;
			Height = height;
			CellSize = cellSize;
			_originPosition = originPosition;
			_grid = new TGridObject[Width, Height];

			for (int x = 0; x < Width; x++)
			{
				for (int y = 0; y < Height; y++)
				{
					_grid[x, y] = createGridObject != null ? createGridObject(this, x, y) : default;
				}
			}
#if UNITY_EDITOR
			new GridDebug<TGridObject>(this);
#endif
		}

		public Vector3 GetWorldPosition(int x, int y)
		{
			return new Vector3(x, y) * CellSize + _originPosition;
		}

		public Vector2Int GetLocalPosition(Vector3 worldPosition)
		{
			var position = worldPosition - _originPosition;
			return new Vector2Int(Mathf.FloorToInt(position.x / CellSize), Mathf.FloorToInt(position.y / CellSize));
		}

		public void SetGridItem(Vector3 worldPosition, TGridObject value)
		{
			var position = GetLocalPosition(worldPosition);
			SetGridItem(position.x, position.y, value);
		}

		public void SetGridItem(int x, int y, TGridObject value)
		{
			if (InRange(x, y))
			{
				_grid[x, y] = value;
				TriggerGridObjectChanged(x, y);
			}
		}

		public void TriggerGridObjectChanged(int x, int y)
		{
			if (OnGridValueChanged != null && InRange(x, y))
			{
				var value = _grid[x, y];
				OnGridValueChanged(this, new OnGridValueChangedEventArgs<TGridObject>(x, y, value));
			}
		}

		public TGridObject GetGridItem(Vector3 worldPosition)
		{
			var position = GetLocalPosition(worldPosition);
			return GetGridItem(position.x, position.y);
		}

		public TGridObject GetGridItem(int x, int y)
		{
			if (InRange(x, y))
			{
				return _grid[x, y];
			}
			return default;
		}

		public bool InRange(Vector2Int position) => InRange(position.x, position.y);

		public bool InRange(Vector3 worldPosition)
		{
			var localPosition = GetLocalPosition(worldPosition);
			return InRange(localPosition.x, localPosition.y);
		}

		public bool InRange(int x, int y) => x >= 0 && y >= 0 && x < Width && y < Height;
	}
}