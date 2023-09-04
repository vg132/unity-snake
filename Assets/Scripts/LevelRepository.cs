//using UnityEngine;

//namespace Assets.Scripts
//{
//	public class LevelRepository
//	{
//		public static GridStuff.Grid LoadLevel(string levelPath)
//		{
//			//var level = Resources.Load<TextAsset>("Levels/Level1");
//			var level = Resources.Load<TextAsset>(levelPath);
//			var grid = new GridStuff.Grid(46, 23, 10f);
//			var lines = level.text.Split('\n');
//			for (int i = 0; i < lines.Length; i++)
//			{
//				var line = lines[i];
//				if (line.StartsWith("#"))
//				{
//					continue;
//				}
//				if (line.StartsWith("@LEVEL START"))
//				{
//					var y = 13;
//					for (; i < lines.Length; i++)
//					{
//						line = lines[i];
//						if (line.StartsWith("#"))
//						{
//							continue;
//						}
//						if (line.StartsWith("@LEVEL END"))
//						{
//							break;
//						}
//						y--;
//						for (int charIndex = 0; charIndex < line.Length; charIndex++)
//						{
//							var x = -24 + charIndex;
//							if (line[charIndex] == '0')
//							{
//								var wall =new Vector2Int(x, y);
//								//grid.AddItem(AIGrid.GridItemTypes.Wall, wall);
//							}
//							else if (line[charIndex] == '1')
//							{
//								var obsticle = new Vector2Int(x, y);
//								//grid.AddItem(AIGrid.GridItemTypes.Obstacle, obsticle);
//							}
//						}
//					}
//				}
//			}
//			return grid;
//		}
//	}
//}