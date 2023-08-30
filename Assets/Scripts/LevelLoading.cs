using UnityEngine;

namespace Assets.Scripts
{
	public class LevelLoading : MonoBehaviour
	{
		public Transform wallPrefab;
		public Transform obsticlePrefab;

		private void Start()
		{
			var level = Resources.Load<TextAsset>("Levels/Level1");

			var lines = level.text.Split('\n');
			for (int i=0;i<lines.Length; i++)
			{
				var line = lines[i];
				if (line.StartsWith("#"))
				{
					continue;
				}
				if(line.StartsWith("@LEVEL START"))
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
								var wall = Instantiate(wallPrefab);
								wall.transform.position = new Vector3(x, y, wall.transform.position.z);
							}
							else if (line[charIndex] == '1')
							{
								var obsticle = Instantiate(obsticlePrefab);
								obsticle.transform.position = new Vector3(x, y, obsticle.transform.position.z);
							}
						}
					}
				}
			}
		}
	}
}