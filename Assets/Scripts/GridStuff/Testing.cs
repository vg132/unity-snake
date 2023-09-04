using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.GridStuff
{
	public class Testing : MonoBehaviour
	{
		[SerializeField]
		private HeatMapBoolVisual heatMapVisual;
		private SnakeControls snakeControls;
		private Grid<bool> _grid;
		private Grid<HeatMapGridObject> _grid2;
		private Grid<StringGridObject> _grid3;
		[SerializeField]
		private HeatMapGenericVisual heatMapGenericVisual;
		private float mouseMoveTimer;
		private float mouseMoveTimerMax = 0.01f;
		private Pathfinding _pathfinding;

		private Vector2Int start;
		private Vector2Int end;

		private void Awake()
		{
			snakeControls = new SnakeControls();
		}

		private void Start()
		{
			//_grid = new Grid<bool>(5, 5, 4.0f);
			//_grid2 = new Grid<HeatMapGridObject>(5, 5, 4.0f, new Vector3(-(6 * 4), 0.0f), (Grid<HeatMapGridObject> grid, int x, int y) => new HeatMapGridObject(grid, x, y));
			//_grid3 = new Grid<StringGridObject>(5, 5, 4.0f, new Vector3(-(6 * 2), -(6 * 4)), (Grid<StringGridObject> grid, int x, int y) => new StringGridObject(grid, x, y));
			//heatMapVisual.SetGrid(_grid);
			//heatMapGenericVisual.SetGrid(_grid2);
			//new GridDebug<bool>(_grid);
			//new GridDebug<HeatMapGridObject>(_grid2);
			//new GridDebug<StringGridObject>(_grid3);

			_pathfinding = new Pathfinding(10, 10);
			Camera.main.transform.position = new Vector3(50, 50, -10);
			Camera.main.orthographicSize = 75;
			new GridDebug<PathNode>(_pathfinding.Grid);
			//_heatMapIntVisual.SetGrid(_grid);
			//_grid.SetValue(2, 1, 0);
			//var heatMapVisual = new HeatMapVisual(_grid, GetComponent<MeshFilter>());
			//grid.CalculatePath(new Vector2Int(1, 1), new Vector2Int(25, 15));
		}

		private void OnEnable()
		{
			snakeControls.Player.MouseClick.performed += MouseClick_performed;
			snakeControls.Player.MouseClick.Enable();

			snakeControls.Player.ReadValue.performed += ReadValue_performed;
			snakeControls.Player.ReadValue.Enable();
		}

		public void Update()
		{
			//mouseMoveTimer -= Time.deltaTime;
			//if (mouseMoveTimer < 0.0f)
			//{
			//	mouseMoveTimer += mouseMoveTimerMax;
			//	var worldPosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
			//	_grid2.GetGridItem(worldPosition)?.AddValue(5);
			//	//_grid.SetValue(worldPosition, gridItem + 1);
			//}
		}

		private void ReadValue_performed(InputAction.CallbackContext obj)
		{
			var position = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
			//_grid.SetGridItem(position, !_grid.GetGridItem(position));
			_pathfinding.Grid.GetGridItem(position).IsObsticle = !_pathfinding.Grid.GetGridItem(position).IsObsticle;
			_pathfinding.Grid.TriggerGridObjectChanged(_pathfinding.Grid.GetGridItem(position).X, _pathfinding.Grid.GetGridItem(position).Y);
		}

		private void MouseClick_performed(InputAction.CallbackContext context)
		{
			var position = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
			//_grid.SetGridItem(position, !_grid.GetGridItem(position));
			end = _pathfinding.Grid.GetLocalPosition(position);
			DrawPath();
		}

		private void DrawPath()
		{
			var path = _pathfinding.FindPath(start.x, start.y, end.x, end.y);
			if (path != null)
			{
				for (int i = 0; i < path.Count - 1; i++)
				{
					Debug.DrawLine(new Vector3(path[i].X, path[i].Y) * _pathfinding.Grid.CellSize + Vector3.one * (_pathfinding.Grid.CellSize/2), new Vector3(path[i + 1].X, path[i + 1].Y) * _pathfinding.Grid.CellSize + Vector3.one * (_pathfinding.Grid.CellSize / 2), Color.green, 10.0f);
				}
			}
		}
	}

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

		protected virtual void TriggerGridObjectChanged()
		{
			_grid.TriggerGridObjectChanged(X, Y);
		}
	}

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

	public class StringGridObject : BaseGridObject<StringGridObject>
	{
		private string _letters;
		private string _numbers;

		public StringGridObject(Grid<StringGridObject> grid, int x, int y)
			: base(grid, x, y)
		{
		}

		public void AddLetter(string letter)
		{
			_letters += letter;
			TriggerGridObjectChanged();
		}

		public void AddNumber(string number)
		{
			_numbers += number;
			TriggerGridObjectChanged();
		}

		public override string ToString() => $"{_letters}\n{_numbers}";
	}
}