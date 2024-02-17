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
			playerScoreText.text = "0";
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
				_nextDirection = new Vector2(x, 0);
			}
		}
	}
}