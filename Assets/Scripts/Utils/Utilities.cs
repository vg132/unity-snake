using TMPro;
using UnityEngine;

namespace Assets.Scripts.Utils
{
	public class Utilities
	{
		public static TextMeshPro CreateWorldTextPro(string text, Transform parent = null, Vector3 localPosition = default(Vector3), int fontSize = 40, Color? color = null, TextAlignmentOptions textAlignment = TextAlignmentOptions.Center)
		{
			if (color == null)
			{
				color = Color.white;
			}
			return CreateWorldTextPro(parent, text, localPosition, fontSize, (Color)color, textAlignment);
		}

		public static TextMeshPro CreateWorldTextPro(Transform parent, string text, Vector3 localPosition, int fontSize, Color color, TextAlignmentOptions textAlignment)
		{
			var gameObject = new GameObject("World_Text_Pro", typeof(TextMeshPro));
			var transform = gameObject.transform as RectTransform;
			transform.SetParent(parent, false);
			transform.localPosition = localPosition;
			transform.sizeDelta = new Vector2(5, 2);
			var textMesh = gameObject.GetComponent<TextMeshPro>();
			textMesh.alignment = textAlignment;
			textMesh.text = text;
			textMesh.fontSize = fontSize;
			textMesh.color = color;

			return textMesh;
		}
	}
}
