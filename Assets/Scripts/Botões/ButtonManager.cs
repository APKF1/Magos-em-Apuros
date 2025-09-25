using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
	public GameObject settingsMenu;
   
	public void BtnPlay()
	{
		SceneManager.LoadScene("Game");
	}

	public void BtnQuit()
	{
		Application.Quit();
		#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
		#endif
	}

	public void BtnSettings()
	{
		settingsMenu.SetActive(true);
	}
}
