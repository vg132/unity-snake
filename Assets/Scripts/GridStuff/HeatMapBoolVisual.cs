using Assets.Scripts.Utils;
using UnityEngine;

namespace Assets.Scripts.GridStuff
{
	[RequireComponent(typeof(MeshFilter))]
	public class HeatMapBoolVisual : MonoBehaviour
	{
		private Grid<bool> _grid;
		private Mesh _mesh;
		private bool _updateMesh;

		private void Awake()
		{
			_mesh = new Mesh();
			GetComponent<MeshFilter>().mesh = _mesh;
		}

		public void SetGrid(Grid<bool> grid)
		{
			_grid = grid;
			_grid.OnGridValueChanged += Grid_OnGridValueChanged;
			_updateMesh = true;
		}

		private void Grid_OnGridValueChanged(object sender, OnGridValueChangedEventArgs<bool> e)
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
					var gridValueNormalized = gridValue ? 1.0f : 0.0f;
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