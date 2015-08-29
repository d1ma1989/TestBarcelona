using UnityEngine;
using UnityEngine.UI;

using System;

public class ChooseLevelMenu : MonoBehaviour
{
	[SerializeField] private Text _levelText;
	[SerializeField] private GameObject _notAvailableLevelText;
	[SerializeField] private GameObject _startButton;

	private int _currentChoosedLevel = 1;

	public event Action<int> LevelChoosed;

	private void IncreaseLevel()
	{
		_currentChoosedLevel++;
		UpdateMenu();
	}

	private void DecreaseLevel()
	{
		_currentChoosedLevel--;
		UpdateMenu();
	}

	private void UpdateMenu()
	{
		_levelText.text = string.Format("LEVEL {0}", _currentChoosedLevel);
		bool levelAvailable = Application.levelCount > _currentChoosedLevel + 1;
		_notAvailableLevelText.SetActive(!levelAvailable);
		_startButton.SetActive(levelAvailable);
	}

	private void OnStartButtonClick()
	{
		if (LevelChoosed != null)
		{
			LevelChoosed(_currentChoosedLevel + 1);
		}
	}
}
