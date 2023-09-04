using Assets.Scripts.Utils;
using System;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.GridStuff
{
	public class GridInt
	{
		public event EventHandler<OnGridValueChangedEventArgs> OnGridValueChanged;
		public class OnGridValueChangedEventArgs : EventArgs
		{
			public int X { get; private set; }
			public int Y { get; private set; }
			public int Value { get; private set; }

			public OnGridValueChangedEventArgs(int x, int y, int value)
			{
				X = x;
				Y = y;
				Value = value;
			}
		}

		public int Width { get; private set; }
		public int Height { get; private set; }
		public float CellSize { get; private set; }
		private Vector3 _originPosition;
		private int[,] _grid;
		private TextMeshPro[,] _debugText;

#if UNITY_EDITOR
		private bool _debug = true;
#endif

		public GridInt(int width, int height, float cellSize, Vector3 originPosition = default)
		{
			Width = width;
			Height = height;
			CellSize = cellSize;
			_originPosition = originPosition;
			_grid = new int[Width, Height];

#if UNITY_EDITOR
			if (_debug)
			{
				SetupDebug();
			}
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

		public void SetValue(Vector3 worldPosition, int value)
		{
			var position = GetLocalPosition(worldPosition);
			SetValue(position.x, position.y, value);
		}

		public void SetValue(int x, int y, int value)
		{
			if (InRange(x, y))
			{
				_grid[x, y] = value;
				if (OnGridValueChanged != null)
				{
					OnGridValueChanged(this, new OnGridValueChangedEventArgs(x, y, value));
				}
			}
		}

		public enum AddValueType
		{
			CenterSquare = 0,
			CenterStar = 1
		}

		public void AddValue(Vector3 worldPosition, int value, int range, int? fullValueRange = null, AddValueType addValueType = AddValueType.CenterSquare)
		{
			var position = GetLocalPosition(worldPosition);
			AddValue(position.x, position.y, value, range, fullValueRange, addValueType);
		}

		public void AddValue(int originX, int originY, int value, int range, int? fullValueRange = null, AddValueType addValueType = AddValueType.CenterSquare)
		{
			fullValueRange = fullValueRange ?? range;
			var lowerValueAmount = Mathf.RoundToInt((float)value / (range - fullValueRange.Value));
			for (var x = 0; x < range; x++)
			{
				for (var y = 0; y < range; y++)
				{
					var radius = x + y;
					var addValueAmount = value;
					if (radius > fullValueRange)
					{
						addValueAmount -= lowerValueAmount * (radius - fullValueRange.Value);
						Debug.Log($"{addValueAmount} - {radius} - {fullValueRange} - {lowerValueAmount}");
					}
					if (addValueAmount <= 0)
					{
						continue;
					}
					if (addValueType == AddValueType.CenterSquare)
					{
						AddValueSquare(originX, originY, x, y, range, addValueAmount);
					}
					else if (addValueType == AddValueType.CenterStar)
					{
						AddValueInStar(originX, originY, x, y, addValueAmount);
					}
				}
			}
		}

		private void AddValueSquare(int originX, int originY, int x, int y, int range, int value)
		{
			AddValue(originX - (range / 2) + x, (originY - (range / 2)) + y, value);
		}

		public void AddValueInStar(int originX, int originY, int x, int y, int value)
		{
			AddValue(originX + x, originY + y, value);
			if (x != 0)
			{
				AddValue(originX - x, originY + y, value);
			}
			if (y != 0)
			{
				AddValue(originX + x, originY - y, value);
				if (x != 0)
				{
					AddValue(originX - x, originY - y, value);
				}
			}
		}

		public void AddValue(Vector3 worldPosition, int value)
		{
			var position = GetLocalPosition(worldPosition);
			AddValue(position.x, position.y, value);
		}

		private void AddValue(int x, int y, int value)
		{
			SetValue(x, y, GetValue(x, y) + value);
		}

		public int GetValue(Vector3 worldPosition)
		{
			var position = GetLocalPosition(worldPosition);
			return GetValue(position.x, position.y);
		}

		public int GetValue(int x, int y)
		{
			if (InRange(x, y))
			{
				return _grid[x, y];
			}
			return -1;
		}

		#region Help functions

		private bool InRange(int x, int y) => x >= 0 && y >= 0 && x < Width && y < Height;

#if UNITY_EDITOR
		private void SetupDebug()
		{
			_debugText = new TextMeshPro[Width, Height];
			for (int x = 0; x < Width; x++)
			{
				for (int y = 0; y < Height; y++)
				{
					_debugText[x, y] = Utilities.CreateWorldTextPro(_grid[x, y].ToString(), null, GetWorldPosition(x, y) + new Vector3(CellSize, CellSize) * 0.5f, 20, Color.white);
					Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 100f);
					Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 100f);
				}
			}
			Debug.DrawLine(GetWorldPosition(0, Height), GetWorldPosition(Width, Height), Color.white, 100f);
			Debug.DrawLine(GetWorldPosition(Width, Height), GetWorldPosition(Width, 0), Color.white, 100f);

			OnGridValueChanged += Grid_OnGridValueChanged;
		}

		private void Grid_OnGridValueChanged(object sender, OnGridValueChangedEventArgs e)
		{
			_debugText[e.X, e.Y].text = e.Value.ToString();
		}

#endif

		#endregion
	}
}