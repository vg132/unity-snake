//using System.Collections.Generic;
//using System.Linq;
//using UnityEngine;

//namespace Assets.Scripts.GridStuff
//{
//	public class Node
//	{
//		public Node parent;
//		public Vector2Int position;
//		public int g;
//		public int h;
//		public int f => g + h;
//	}

//	public class GridPathfinder
//	{
//		public List<Node> CalculatePath(Grid grid, Vector2Int startPoint, Vector2Int endPoint)
//		{
//			var openNodes = new List<Node>();
//			var closedNodes = new List<Node>();

//			var start = new Node { position = startPoint };
//			var end = new Node { position = endPoint };

//			openNodes.Add(start);
//			var failSafe = 10000;
//			while (openNodes.Count > 0 && failSafe > 0)
//			{
//				failSafe--;
//				var currentIndex = 0;
//				var currentNode = openNodes[currentIndex];
//				for (int i = 1; i < openNodes.Count; i++)
//				{
//					if (openNodes[i].f < currentNode.f)
//					{
//						currentNode = openNodes[i];
//						currentIndex = i;
//					}
//				}
//				openNodes.RemoveAt(currentIndex);
//				closedNodes.Add(currentNode);
//				if (currentNode.position == endPoint)
//				{
//					Debug.Log("Found the end!");

//					var path = new List<Node>();
//					do
//					{
//						path.Add(currentNode);
//						currentNode = currentNode.parent;
//					} while (currentNode != null);
//					path.Reverse();
//					Debug.Log($"Path: {string.Join(" - ", path.Select(item => item.position))} (Iterations: {10000 - failSafe})");
//					return path;
//				}
//				var children = new List<Node>();
//				for (int x = -1; x < 2; x++)
//				{
//					for (int y = -1; y < 2; y++)
//					{
//						if (y == 0 && x == 0)
//						{
//							continue;
//						}
//						var position = new Vector2Int(currentNode.position.x + x, currentNode.position.y + y);
//						if (position.y < 0 || position.x < 0 || position.y >= grid.Height || position.x >= grid.Width)
//						{
//							continue;
//						}
//						children.Add(new Node { position = position, parent = currentNode });
//					}
//				}

//				foreach (var child in children)
//				{
//					if (closedNodes.Any(item => item.position == child.position))
//					{
//						continue;
//					}
//					child.g = currentNode.g + 1;
//					child.h = (int)(Mathf.Pow(child.position.x - endPoint.x, 2) + Mathf.Pow(child.position.y - endPoint.y, 2) * 100);

//					if (openNodes.Any(item => item.position == child.position && item.g < child.g))
//					{
//						continue;
//					}
//					openNodes.Add(child);
//				}
//			}
//			Debug.Log("Failsafe: " + failSafe);
//			return new List<Node>();
//		}
//	}
//}