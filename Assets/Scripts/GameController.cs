using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {
	
	public PlayerController playerController;

	public enum EGameState
	{
		E_START_GAME,
		E_INGAME,
		E_END_GAME_WIN,
		E_END_GAME_PLAYER_DETECTED,
		E_END_GAME_CORPSE_DETECTED,
		E_END_GAME_TIMEOUT
	}

	private int _remainingEnemies;

	private EnemyController[] _aEnemies;
	private float _countDownValue = 15f;
	private EGameState _currentState = EGameState.E_START_GAME; 


	void Awake () {
		Input.simulateMouseWithTouches = true;
		_aEnemies = FindObjectsOfType<EnemyController> ();
		_remainingEnemies = _aEnemies.Length;
	}
	

	void Update () {

		if (_currentState == EGameState.E_START_GAME)
		{
			if (Input.GetMouseButtonDown(0)) {

				OnStartGame();
				
			}
		}
		else if (_currentState == EGameState.E_INGAME)
		{

			_countDownValue -= Time.deltaTime;

			if (_countDownValue > 0)
			{
			
				if (Input.GetMouseButtonDown(0)) {

						RaycastHit hit;
						Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
						if (Physics.Raycast(ray, out hit))
						{
							if (hit.collider.tag == "Enemy")
							{
								playerController.goToEnemy(hit.collider.gameObject);
							}
							else
							{
								playerController.goToPosition(hit.point);
							}
						}

				}
			}
			else
			{
				_countDownValue = 0f;
				OnTimeOut();
			}
		}



	}

	public void OnStartGame()
	{
		FindObjectOfType<GUIController> ().OnStartGame ();
		_currentState = EGameState.E_INGAME;
	}

	public void OnEnemyKilled()
	{
		_remainingEnemies--;
		if (_remainingEnemies <= 0) {
			_currentState = EGameState.E_END_GAME_WIN;
			FindObjectOfType<GUIController> ().OnEndGame (_currentState);

			// Disable all enemies
			foreach(EnemyController enemy in FindObjectOfType<GameController>().getEnemies())
			{
				enemy.OnEndGame();
			}
		}
	}

	public void OnPlayerKilled()
	{
		_currentState = EGameState.E_END_GAME_PLAYER_DETECTED;
		FindObjectOfType<GUIController> ().OnEndGame (_currentState);

		// Disable all enemies
		foreach(EnemyController enemy in FindObjectOfType<GameController>().getEnemies())
		{
			enemy.OnEndGame();
		}
	}

	public void OnCorpseDetected()
	{
		_currentState = EGameState.E_END_GAME_CORPSE_DETECTED;
		FindObjectOfType<GUIController> ().OnEndGame (_currentState);
				
		// Disable all enemies
		foreach(EnemyController enemy in FindObjectOfType<GameController>().getEnemies())
		{
			enemy.OnEndGame();
		}
	}

	public void OnTimeOut()
	{
		_currentState = EGameState.E_END_GAME_TIMEOUT;
		FindObjectOfType<GUIController> ().OnEndGame (_currentState);

	}

	public EnemyController[] getEnemies()
	{
		return _aEnemies;
	}

	public float getCountDown()
	{
		return _countDownValue;
	}

}
