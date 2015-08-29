using UnityEngine;

public class StartSceneManager : MonoBehaviour
{
	[SerializeField] private GUIManager _guiManager;

	// Application entry point
	private void Awake()
	{
		AppManager.Create();
		AppManager.GUI = _guiManager;
		_guiManager.Init();
		Application.LoadLevel(1);
	}
}
