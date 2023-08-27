using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts
{
	public class Snake : MonoBehaviour
	{
		private Vector2 direction;
		private SnakeControls inputControls;
		private List<Transform> segments;

		public Transform segmentPrefab;
		public int startSize = 4;

		private void Start()
		{
			NewGame();
		}

		private void NewGame()
		{
			if (segments != null)
			{
				for (int i = 1; i < segments.Count; i++)
				{
					Destroy(segments[i].gameObject);
				}
			}
			transform.position = Vector3.zero;
			direction = Vector2.right;
			segments = new List<Transform> { transform };
			for (int i = 1; i < startSize; i++)
			{
				segments.Add(Instantiate(segmentPrefab));
			}
		}

		private void Awake()
		{
			inputControls = new SnakeControls();
		}

		private void OnEnable()
		{
			inputControls.Player.Move.performed += Move_performed;
			inputControls.Player.Move.Enable();
		}

		private void OnDisable()
		{
			inputControls.Player.Move.performed -= Move_performed;
			inputControls.Player.Move.Disable();
		}

		private void Move_performed(InputAction.CallbackContext context)
		{
			direction = context.ReadValue<Vector2>();
		}

		private void Update()
		{
		}

		private void FixedUpdate()
		{
			for(int i=segments.Count-1; i>0; i--)
			{
				segments[i].position = segments[i - 1].position;
			}
			transform.position = new Vector3(Mathf.RoundToInt(transform.position.x) + direction.x, Mathf.RoundToInt(transform.position.y) + direction.y, transform.position.z);
		}

		private void OnTriggerEnter2D(Collider2D collision)
		{
			Debug.Log(collision.tag); 
			if (collision.tag == "Food")
			{
				Grow();
			}
			else if (collision.tag == "Obsticle")
			{
				NewGame();
			}
		}

		private void Grow()
		{
			var segment = Instantiate(segmentPrefab);
			segment.position = segments.Last().position;
			segments.Add(segment);
		}
	}
}