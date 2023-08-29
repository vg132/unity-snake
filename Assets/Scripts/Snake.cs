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
			Countdown,
			Running,
			Pause,
			GameOver
		}

		private Vector2 _direction;
		private Vector2 _nextDirection;
		private SnakeControls _inputControls;
		private List<Transform> _segments = new List<Transform>();
		private int _score;
		private GameState _state;
		private float _countdown;
		private float _speedCounter;
		private float _speed;

		public Transform segmentPrefab;
		public int startSize = 4;
		public TextMeshProUGUI playerScoreText;
		public TextMeshProUGUI coundownText;
		public TextMeshProUGUI highScoreText;
		public GameObject mainMenu;
		public GameObject countdownMenu;
		public float startSpeed = 0.13f;
		public AnimationCurve speedCurve;

		private void NewGame()
		{
			for (int i = 1; i < _segments.Count; i++)
			{
				Destroy(_segments[i].gameObject);
			}
			_speed = startSpeed;
			_score = 0;
			playerScoreText.text = "0";
			transform.position = Vector3.zero;
			_nextDirection = Vector2.right;
			_direction = Vector2.right;
			_segments = new List<Transform> { transform };
			for (int i = 1; i < startSize; i++)
			{
				_segments.Add(Instantiate(segmentPrefab));
			}
			State = GameState.Countdown;
		}

		private void Awake()
		{
			_inputControls = new SnakeControls();
			highScoreText.text = Settings.HighScore.ToString();
			State = GameState.Menu;
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
			if (State == GameState.Running)
			{
				var newDirection = context.ReadValue<Vector2>();
				var x = Mathf.RoundToInt(newDirection.x);
				var y = Mathf.RoundToInt(newDirection.y);

				if (_direction.x != 0 && y != 0)
				{
					_nextDirection = newDirection;
				}
				if (_direction.y != 0 && x != 0)
				{
					_nextDirection = newDirection;
				}
			}
		}

		private void Update()
		{
			if (State == GameState.Countdown)
			{
				_countdown -= Time.deltaTime;
				if(_countdown > 0)
				{
					coundownText.text = Mathf.CeilToInt(_countdown * 2).ToString();
				}
				else
				{
					coundownText.text = "GO!";
					State = GameState.Running;
				}
			}
		}

		private void FixedUpdate()
		{
			if(State == GameState.Running)
			{
				_speedCounter += Time.fixedDeltaTime;
				if(_speedCounter > _speed)
				{
					_speedCounter = 0;
					for (int i = _segments.Count - 1; i > 0; i--)
					{
						_segments[i].position = _segments[i - 1].position;
					}
					_direction = _nextDirection;
					var x = Mathf.RoundToInt(transform.position.x) + Mathf.RoundToInt(_direction.x);
					var y = Mathf.RoundToInt(transform.position.y) + Mathf.RoundToInt(_direction.y);
					transform.position = new Vector3(x, y, transform.position.z);
				}
			}
		}

		private void HideCountdownMenu()
		{
			countdownMenu.SetActive(false);
		}

		private GameState State
		{
			get => _state;
			set
			{
				if (_state == value)
				{
					return;
				}
				_state = value;
				if (_state == GameState.Menu)
				{
					mainMenu.SetActive(true);
					countdownMenu.SetActive(false);
				}
				else if (_state == GameState.Running)
				{
					mainMenu.SetActive(false);
					Invoke(nameof(HideCountdownMenu), 0.5f);
				}
				else if (_state == GameState.Countdown)
				{
					mainMenu.SetActive(false);
					countdownMenu.SetActive(true);
					_countdown = 1.5f;
				}
				else if (_state == GameState.GameOver)
				{
					mainMenu.SetActive(true);
					countdownMenu.SetActive(false);
				}
			}
		}

		private void OnTriggerEnter2D(Collider2D collision)
		{
			if (State == GameState.Running)
			{
				if (collision.tag == "Food")
				{
					_score++;
					_speed = ((1.0f - speedCurve.Evaluate(_score / 50.0f)) * ((startSpeed-0.02f) * 100.0f)) / 100.0f + 0.02f;
					playerScoreText.text = _score.ToString();
					Settings.HighScore = _score;
					highScoreText.text = Settings.HighScore.ToString();
					Grow();
				}
				else if (collision.tag == "Obsticle")
				{
					State = GameState.GameOver;
				}
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