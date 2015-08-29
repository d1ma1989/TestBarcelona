using UnityEngine;

using System;

public class GameController : MonoBehaviour
{
	[SerializeField] private EnemyController[] _aEnemies;
	[SerializeField] private PlayerController _playerController;
	[SerializeField] private GroundClickListener _groundClickListener;

	private int _remainingEnemies;

	public Camera MainCamera { get; private set; }

	public EGameState CurrentState { get; private set; }

	public PlayerController Player { get { return _playerController; } }

	public EnemyController[] Enemies { get { return _aEnemies; } }

	private float _countDown;

	private void Awake()
	{
		AppManager.GameController = this;

		AppManager.GUI.ShowLoadingText(false);
		AppManager.GUI.ShowInfoScreen(true);

		Input.simulateMouseWithTouches = true;

		_countDown = 15f;
		_remainingEnemies = _aEnemies.Length;
		MainCamera = Camera.main;

		CurrentState = EGameState.E_START_GAME;

		foreach (EnemyController enemy in _aEnemies)
		{
			enemy.OnEnemyKilled += OnEnemyKilled;
			enemy.OnPlayerDetected += OnPlayerKilled;
			enemy.OnCorpseDetected += OnCorpseDetected;
		}

		_groundClickListener.OnGroundClick += (sender, args) =>
		{
			if (CurrentState == EGameState.E_INGAME)
			{
				_playerController.GoToPosition(args.Pos);
			}
		};
	}

	private void OnPlayerKilled(object sender, EventArgs args)
	{
		CurrentState = EGameState.E_END_GAME_PLAYER_DETECTED;
		Player.Die();
		EndGame();
	}

	private void OnEnemyKilled(object sender, EventArgs args)
	{
		_remainingEnemies--;
		if (_remainingEnemies <= 0)
		{
			CurrentState = EGameState.E_END_GAME_WIN;
			EndGame();
		}
	}

	private void OnCorpseDetected(object sender, EventArgs args)
	{
		CurrentState = EGameState.E_END_GAME_CORPSE_DETECTED;
		EndGame();
	}

	private void Update()
	{
		if (Input.GetMouseButtonUp(0))
		{
			if (CurrentState == EGameState.E_START_GAME)
			{
				AppManager.GUI.ShowInfoScreen(false);
				AppManager.GUI.ShowTimer(true);
				CurrentState = EGameState.E_INGAME;
			}
			else if( CurrentState != EGameState.E_INGAME)
			{
				AppManager.I.ReturnToMenu();
			}
		}

		if (CurrentState == EGameState.E_INGAME)
		{
			_countDown -= Time.deltaTime;
			AppManager.GUI.Timer = _countDown;
			if (_countDown <= 0)
			{
				_countDown = 0f;
				CurrentState = EGameState.E_END_GAME_TIMEOUT;
				EndGame();
			}
		}
	}

	private void EndGame()
	{
		AppManager.GUI.ShowInfoScreen(true);
		AppManager.GUI.ShowTimer(false);

		// Disable all enemies
		foreach (EnemyController enemy in Enemies)
		{
			enemy.OnEndGame();
		}
	}
}

public enum EGameState
{
	E_START_GAME,
	E_INGAME,
	E_END_GAME_WIN,
	E_END_GAME_PLAYER_DETECTED,
	E_END_GAME_CORPSE_DETECTED,
	E_END_GAME_TIMEOUT
}
