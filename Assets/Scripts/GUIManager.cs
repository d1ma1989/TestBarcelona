using UnityEngine;
using UnityEngine.UI;

public class GUIManager : MonoBehaviour
{
	[SerializeField] private Text _countDownSecText;
	[SerializeField] private Text _countDownMiliSecText;
	[SerializeField] private Text _infoText;
	
	[SerializeField] private ChooseLevelMenu _chooseLevelMenu;

	[SerializeField] private GameObject _loadingTextGO;
	[SerializeField] private GameObject _backgroundGO;

	public float Timer
	{
		set
		{
			if (value < 0)
			{
				value = 0;
			}
			int sec = (int)value;
			int milli = (int)((value - sec) * 100);
			_countDownSecText.text = sec.ToString("00");
			_countDownMiliSecText.text = milli.ToString("00");
		}
	}

	public void Init()
	{
		DontDestroyOnLoad(gameObject);
		_chooseLevelMenu.LevelChoosed += AppManager.I.LoadLevel;
	}

	public void ShowChooseLevelMenu(bool value)
	{
		_chooseLevelMenu.gameObject.SetActive(value);
	}

	public void ShowLoadingText(bool value)
	{
		_loadingTextGO.SetActive(value);
	}

	public void ShowTimer(bool value)
	{
		_countDownSecText.gameObject.SetActive(value);
		_countDownMiliSecText.gameObject.SetActive(value);
	}

	public void ShowInfoScreen(bool value)
	{
		_backgroundGO.SetActive(value);
		_infoText.gameObject.SetActive(value);

		if (!value)
		{
			return;
		}

		const string restartText = "\nTOUCH TO RESTART";

		switch (AppManager.GameController.CurrentState)
		{
			case EGameState.E_START_GAME:
				_infoText.text = "TOUCH TO START";
				_infoText.color = Color.green;
				break;
			case EGameState.E_END_GAME_WIN:
				_infoText.text = "YOU WIN" + restartText;
				_infoText.color = Color.green;
				break;
			case EGameState.E_END_GAME_PLAYER_DETECTED:
				_infoText.text = "PLAYER DETECTED" + restartText;
				_infoText.color = Color.red;
				break;
			case EGameState.E_END_GAME_CORPSE_DETECTED:
				_infoText.text = "CORPSE DETECTED" + restartText;
				_infoText.color = Color.red;
				break;
			case EGameState.E_END_GAME_TIMEOUT:
				_infoText.text = "TIMEOUT" + restartText;
				_infoText.color = Color.red;
				break;
		}
	}
}
