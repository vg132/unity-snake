using UnityEngine;

namespace Assets.Scripts.GridStuff
{
	public class Testing : MonoBehaviour
	{
		private void Start()
		{
			var grid = new AIGrid(46, 23);
			grid.CalculatePath(new Vector2Int(1, 1), new Vector2Int(25, 15));
		}
	}
}