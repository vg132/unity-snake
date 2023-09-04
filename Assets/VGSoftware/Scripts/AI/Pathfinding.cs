using Assets.VGSoftware.Scripts.Grid;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.VGSoftware.Scripts.AI
{
	public class Pathfinding
	{
		public Grid<PathNode> Grid { get; private set; }

		public Pathfinding(int width, int height)
		{
			Grid = new Grid<PathNode>(width, height, 10f, createGridObject: (Grid<PathNode> grid, int x, int y) => new PathNode(grid, x, y));
		}

		public List<Vector3> FindPath(Vector3 startWorldPosition, Vector3 endWorldPosition)
		{
			var startNode = Grid.GetGridItem(startWorldPosition);
			var endNode = Grid.GetGridItem(endWorldPosition);
			var path = FindPath(startNode.X, startNode.Y, endNode.X, endNode.Y);
			return path.Select(item => Grid.GetWorldPosition(item.X, item.Y)).ToList();
		}

		public IList<PathNode> FindPath(int startX, int startY, int endX, int endY)
		{
			var startNode = Grid.GetGridItem(startX, startY);
			var endNode = Grid.GetGridItem(endX, endY);
			var openList = new List<PathNode>() { startNode };
			var closedList = new List<PathNode>();

			for (var x = 0; x < Grid.Width; x++)
			{
				for (var y = 0; y < Grid.Height; y++)
				{
					var node = Grid.GetGridItem(x, y);
					node.CameFromNode = null;
					node.gCost = int.MaxValue;
					Grid.TriggerGridObjectChanged(x, y);
				}
			}

			startNode.gCost = 0;
			startNode.hCost = CalculateDistanceCost(startNode, endNode);

			while (openList.Count > 0)
			{
				var currentNode = openList.OrderBy(item => item.fCost).First();
				if (currentNode == endNode)
				{
					return CalculatePath(endNode);
				}
				openList.Remove(currentNode);
				closedList.Add(currentNode);
				foreach (var neighbour in GetNeighbourList(currentNode))
				{
					if(neighbour.IsObsticle)
					{
						closedList.Add(neighbour);
						continue;
					}
					if (closedList.Contains(neighbour))
					{
						continue;
					}
					var newGCost = currentNode.gCost + CalculateDistanceCost(neighbour, currentNode);
					if (newGCost < neighbour.gCost)
					{
						neighbour.gCost = newGCost;
						neighbour.hCost = CalculateDistanceCost(neighbour, endNode);
						neighbour.CameFromNode = currentNode;
						if (!openList.Contains(neighbour))
						{
							openList.Add(neighbour);
						}
						Grid.TriggerGridObjectChanged(neighbour.X, neighbour.Y);
					}
				}
			};
			return new List<PathNode>();
		}

		private List<PathNode> CalculatePath(PathNode endNode)
		{
			var path = new List<PathNode>();
			var currentNode = endNode;
			do
			{
				path.Add(currentNode);
				currentNode = currentNode.CameFromNode;
			} while (currentNode != null);
			path.Reverse();
			return path;
		}

		private List<PathNode> GetNeighbourList(PathNode currentNode)
		{
			// Get surrounding nodes
			var neighbors = new List<PathNode>();
			for (int x = -1; x < 2; x++)
			{
				for (int y = -1; y < 2; y++)
				{
					if (y == 0 && x == 0)
					{
						continue;
					}
					var position = new Vector2Int(currentNode.X + x, currentNode.Y + y);
					if (Grid.InRange(position))
					{
						neighbors.Add(Grid.GetGridItem(position.x, position.y));
					}
				}
			}
			return neighbors;
		}

		private int CalculateDistanceCost(PathNode startNode, PathNode endNode) => (int)(Mathf.Pow(startNode.X - endNode.X, 2) + Mathf.Pow(startNode.Y - endNode.Y, 2) * 100);
	}
}