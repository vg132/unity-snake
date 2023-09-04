using Assets.Scripts.Utils;
using UnityEngine;

namespace Assets.VGSoftware.Scripts.Grid.Visualizer
{
	[RequireComponent(typeof(MeshFilter))]
	public class HeatMapIntVisual : MonoBehaviour
	{
		private Grid<int> _grid;
		private Mesh _mesh;
		private bool _updateMesh = true;

		private void Awake()
		{
			_mesh = new Mesh();
			GetComponent<MeshFilter>().mesh = _mesh;
		}

		public void SetGrid(Grid<int> grid)
		{
			_grid = grid;
			_grid.OnGridValueChanged += Grid_OnGridValueChanged;
		}

		private void Grid_OnGridValueChanged(object sender, OnGridValueChangedEventArgs<int> e)
		{
			_updateMesh = true;
		}

		private void LateUpdate()
		{
			if (_updateMesh)
			{
				_updateMesh = false;
				UpdateHeatMapVisual();
			}
		}

		public void UpdateHeatMapVisual()
		{
			Vector3[] vertices;
			Vector2[] uv;
			int[] triangles;
			MeshUtils.CreateEmptyMeshArrays(_grid.Width * _grid.Height, out vertices, out uv, out triangles);

			for (int x = 0; x < _grid.Width; x++)
			{
				for (int y = 0; y < _grid.Height; y++)
				{
					var index = x * _grid.Height + y;
					var baseSize = new Vector3(1, 1) * _grid.CellSize;
					var gridValue = _grid.GetGridItem(x, y);
					var maxGridValue = 100;
					var gridValueNormalized = Mathf.Clamp01((float)gridValue / maxGridValue);
					var gridCellUV = new Vector2(gridValueNormalized, 0f);
					MeshUtils.AddToMeshArrays(vertices, uv, triangles, index, _grid.GetWorldPosition(x, y) + baseSize * 0.5f, 0f, baseSize, gridCellUV, gridCellUV);
				}
			}
			_mesh.vertices = vertices;
			_mesh.uv = uv;
			_mesh.triangles = triangles;
		}
	}
}