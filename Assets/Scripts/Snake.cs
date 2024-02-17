using Assets.VGSoftware.Scripts.Grid;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using static Assets.Scripts.SnakePlayer;

namespace Assets.Scripts
{
	public class Snake : MonoBehaviour
	{
		public GameObject snakePrefab;

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

		public void Initialize()
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

		private Transform CreateSnakeSegment()
		{
			var segment = Instantiate(segmentPrefab);
			segment.GetComponent<SpriteRenderer>().color = GetComponent<SpriteRenderer>().color;
			return segment;
		}
	}
}