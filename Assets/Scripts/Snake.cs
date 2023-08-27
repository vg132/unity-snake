using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts
{
	public class Snake : MonoBehaviour
	{
		enum GameState
		{
			None,
			Menu,
			Running,
			Pause,
			GameOver
		}

		private Vector2 _direction;
		private SnakeControls _inputControls;
		private List<Transform> _segments = new List<Transform>();
		private int _score;
		private GameState _state;

		public Transform segmentPrefab;
		public int startSize = 4;
		public TextMeshProUGUI playerScoreText;
		public GameObject mainMenu;

		private void NewGame()
		{
			mainMenu.SetActive(false);
			for (int i = 1; i < _segments.Count; i++)
			{
				Destroy(_segments[i].gameObject);
			}
			_score = 0;
			playerScoreText.text = "0";
			transform.position = Vector3.zero;
			_direction = Vector2.right;
			_segments = new List<Transform> { transform };
			for (int i = 1; i < startSize; i++)
			{
				_segments.Add(Instantiate(segmentPrefab));
			}
			_state = GameState.Running;
		}

		private void Awake()
		{
			_inputControls = new SnakeControls();
			_state = GameState.Menu;
		}

		private void OnEnable()
		{
			_inputControls.Player.Move.performed += Move_performed;
			_inputControls.Player.Move.Enable();
		}

		private void OnDisable()
		{
			_inputControls.Player.Move.performed -= Move_performed;
			_inputControls.Player.Move.Disable();
		}

		private void Move_performed(InputAction.CallbackContext context)
		{
			if (_state == GameState.Running)
			{
				var newDirection = context.ReadValue<Vector2>();
				var x = Mathf.RoundToInt(newDirection.x);
				var y = Mathf.RoundToInt(newDirection.y);

				if (_direction.x != 0 && y != 0)
				{
					_direction = newDirection;
				}
				if (_direction.y != 0 && x != 0)
				{
					_direction = newDirection;
				}
			}
		}

		private void FixedUpdate()
		{
			if(_state == GameState.Running)
			{
				for (int i = _segments.Count - 1; i > 0; i--)
				{
					_segments[i].position = _segments[i - 1].position;
				}
				var x = Mathf.RoundToInt(transform.position.x) + Mathf.RoundToInt(_direction.x);
				var y = Mathf.RoundToInt(transform.position.y) + Mathf.RoundToInt(_direction.y);
				transform.position = new Vector3(x, y, transform.position.z);
			}
		}

		private void OnTriggerEnter2D(Collider2D collision)
		{
			if (collision.tag == "Food")
			{
				_score++;
				playerScoreText.text = _score.ToString();
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
			segment.position = _segments.Last().position;
			_segments.Add(segment);
		}

		public void ExitGame()
		{
			Application.Quit();
		}

		public void OnePlayer()
		{
			NewGame();
		}

		public void TwoPlayers()
		{
			NewGame();
		}

		public void VsCPU()
		{
			NewGame();
		}
	}
}