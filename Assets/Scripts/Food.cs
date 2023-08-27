using UnityEngine;

namespace Assets.Scripts
{
	public class Food : MonoBehaviour
	{
		public BoxCollider2D gridArea;

		private void Start()
		{
			RandomizePostion();
		}

		private void RandomizePostion()
		{
			var bounds = gridArea.bounds;
			var x = Mathf.RoundToInt(Random.Range(bounds.min.x, bounds.max.x));
			var y = Mathf.RoundToInt(Random.Range(bounds.min.y, bounds.max.y));

			transform.position = new Vector3(x, y, transform.position.z);
		}

		private void OnTriggerEnter2D(Collider2D collision)
		{
			if (collision.tag == "Player")
			{
				RandomizePostion();
			}
		}
	}
}