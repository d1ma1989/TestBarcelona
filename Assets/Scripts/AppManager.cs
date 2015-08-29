using UnityEngine;

public class AppManager
{
	public static AppManager I { get; private set; }

	public static GUIManager GUI { get; set; }
	public static GameController GameController { get; set; }

	public static void Create()
	{
		if (I != null)
		{
			Debug.LogWarning("AppManager is already created");
			return;
		}

		I = new AppManager();
	}

	public void LoadLevel(int level)
	{
		GUI.ShowChooseLevelMenu(false);
		GUI.ShowLoadingText(true);
		Application.LoadLevel(level);
	}

	public void ReturnToMenu()
	{
		Application.LoadLevel(1);
	}
}
