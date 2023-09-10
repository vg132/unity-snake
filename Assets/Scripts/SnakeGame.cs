using Assets.VGSoftware.Scripts.Grid;
using TMPro;
using UnityEngine;

namespace Assets.Scripts
{
	public class SnakeGame : MonoBehaviour
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

		enum GameModeType
		{
			None,
			OnePlayer,
			TwoPlayers,
			OnePlayerVSCPU
		}

		private SnakeControls _inputControls;
		private GameState _state;
		private float _countdown;
		private float _speedCounter;
		private float _speed;
		private GameModeType GameMode { get; set; }

		public TextMeshProUGUI coundownText;
		public TextMeshProUGUI highScoreText;
		public GameObject mainMenu;
		public GameObject countdownMenu;
		public SnakePlayer player1;
		public SnakePlayer player2;

		public int HighScore { get; set; }

		private Grid<LevelGridItem> levelGrid;

		private void NewGame(GameModeType gameMode)
		{
			//levelGrid = LevelRepository.LoadLevel(Constants.Levels.Level1);
			//new Pathfinding(levelGrid.Width, levelGrid.Height);
			State = GameState.Countdown;
			GameMode = gameMode;
			player1.NewGame();
			if(GameMode == GameModeType.TwoPlayers)
			{
				player2.NewGame();
			}
		}

		private void Awake()
		{
			player1.enabled = false;
			player2.enabled = false;
			_inputControls = new SnakeControls();
			highScoreText.text = Settings.HighScore.ToString();
			State = GameState.Menu;
		}

		private void Update()
		{
			if (State == GameState.Countdown)
			{
				_countdown -= Time.deltaTime;
				if (_countdown > 0)
				{
					coundownText.text = Mathf.CeilToInt(_countdown * 2).ToString();
				}
				else
				{
					coundownText.text = "GO!";
					State = GameState.Running;
				}
			}
			else if (State == GameState.Running)
			{
				highScoreText.text = Settings.HighScore.ToString();
				Debug.Log($"Player is alive: {player1.IsAlive}");
				if(!player1.IsAlive)
				{
					player1.enabled = false;
					State = GameState.GameOver;
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
					player1.enabled = true;
					if (GameMode == GameModeType.TwoPlayers)
					{
						player2.enabled = true;
					}
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
					player1.enabled = false;
					player2.enabled = false;
					mainMenu.SetActive(true);
					countdownMenu.SetActive(false);
				}
			}
		}

		public void ExitGame()
		{
			Application.Quit();
		}

		public void OnePlayer()
		{
			NewGame(GameModeType.OnePlayer);
		}

		public void TwoPlayers()
		{
			NewGame(GameModeType.TwoPlayers);
		}

		public void VsCPU()
		{
			NewGame(GameModeType.OnePlayerVSCPU);
		}
	}
}