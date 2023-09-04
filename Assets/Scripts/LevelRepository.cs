using Assets.VGSoftware.Scripts.Grid;
using UnityEngine;

namespace Assets.Scripts
{
	public enum LevelItemType
	{
		None = 0,
		Wall = 1,
		Obsticle = 2,
		Snake = 3,
	}

	public struct LevelGridItem
	{
		public LevelItemType itemType;
	}

	public class LevelRepository
	{
		public static Grid<LevelGridItem> LoadLevel(string levelPath)
		{
			//var level = Resources.Load<TextAsset>("Levels/Level1");
			var level = Resources.Load<TextAsset>(levelPath);
			var grid = new Grid<LevelGridItem>(46, 23, 10f);
			var lines = level.text.Split('\n');
			for (int i = 0; i < lines.Length; i++)
			{
				var line = lines[i];
				if (line.StartsWith("#"))
				{
					continue;
				}
				if (line.StartsWith("@LEVEL START"))
				{
					var y = 13;
					for (; i < lines.Length; i++)
					{
						line = lines[i];
						if (line.StartsWith("#"))
						{
							continue;
						}
						if (line.StartsWith("@LEVEL END"))
						{
							break;
						}
						y--;
						for (int charIndex = 0; charIndex < line.Length; charIndex++)
						{
							var x = -24 + charIndex;
							if (line[charIndex] == '0')
							{
								var gridItem = grid.GetGridItem(x, y);
								gridItem.itemType = LevelItemType.Wall;
								grid.SetGridItem(x, y, gridItem);
							}
							else if (line[charIndex] == '1')
							{
								var gridItem = grid.GetGridItem(x, y);
								gridItem.itemType = LevelItemType.Wall;
								grid.SetGridItem(x, y, gridItem);
							}
						}
					}
				}
			}
			return grid;
		}
	}
}