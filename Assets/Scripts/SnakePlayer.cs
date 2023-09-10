using Assets.VGSoftware.Scripts.Grid;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts
{
	[RequireComponent(typeof(SpriteRenderer))]
	public class SnakePlayer : MonoBehaviour
	{
		public enum PlayerType
		{
			PlayerOne,
			PlayerTwo,
			CPU
		}

		private Vector2 _direction;
		private Vector2 _nextDirection;
		private SnakeControls _inputControls;
		private List<Transform> _segments = new List<Transform>();
		
		private float _speedCounter;
		private float _speed;

		public bool IsAlive { get; set; }
		public int Score { get; set; }

		public Transform segmentPrefab;
		public int startSize = 4;
		public TextMeshProUGUI playerScoreText;
		public float startSpeed = 0.13f;
		public AnimationCurve speedCurve;
		public PlayerType playerType;

		private Grid<LevelGridItem> levelGrid;

		public void NewGame()
		{
			for (int i = 1; i < _segments.Count; i++)
			{
				Destroy(_segments[i].gameObject);
			}
			_speed = startSpeed;
			Score = 0;
			IsAlive = true;
			playerScoreText.text = "0";
			transform.position = Vector3.zero;
			_nextDirection = Vector2.right;
			_direction = Vector2.right;
			_segments = new List<Transform> { transform };
			for (int i = 1; i < startSize; i++)
			{
				_segments.Add(CreateSnakeSegment());
			}
		}

		private Transform CreateSnakeSegment()
		{
			var segment = Instantiate(segmentPrefab);
			segment.GetComponent<SpriteRenderer>().color = GetComponent<SpriteRenderer>().color;
			return segment;
		}

		private void Awake()
		{
			_inputControls = new SnakeControls();
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
			var newDirection = context.ReadValue<Vector2>();
			var x = Mathf.RoundToInt(newDirection.x);
			var y = Mathf.RoundToInt(newDirection.y);
			if (_direction.x != 0 && y != 0)
			{
				_nextDirection = new Vector2(0, y);
			}
			if (_direction.y != 0 && x != 0)
			{
				_nextDirection= new Vector2(x, 0);
			}
		}

		private void Update()
		{
		}

		private void FixedUpdate()
		{
			if (isActiveAndEnabled)
			{
				_speedCounter += Time.fixedDeltaTime;
				if (_speedCounter > _speed)
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

		private void OnTriggerEnter2D(Collider2D collision)
		{
			if (isActiveAndEnabled)
			{
				if (collision.tag == Constants.Tags.Food)
				{
					Score += 1;
					_speed = ((1.0f - speedCurve.Evaluate(Score / 50.0f)) * ((startSpeed - 0.02f) * 100.0f)) / 100.0f + 0.02f;
					playerScoreText.text = Score.ToString();
					Settings.HighScore = Score;
					Grow();
				}
				else if (collision.tag == Constants.Tags.Obsticle || collision.tag == Constants.Tags.Player)
				{
					IsAlive = false;
				}
			}
		}

		private void Grow()
		{
			var segment = CreateSnakeSegment();
			segment.position = _segments.Last().position;
			_segments.Add(segment);
		}
	}
}