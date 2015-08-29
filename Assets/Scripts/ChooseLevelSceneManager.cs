using UnityEngine;

public class ChooseLevelSceneManager : MonoBehaviour
{
	private void Awake()
	{
		AppManager.GUI.ShowInfoScreen(false);
		AppManager.GUI.ShowChooseLevelMenu(true);
	}
}
