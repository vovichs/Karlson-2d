using Audio;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Lobby : MonoBehaviour
{
	public GameObject menu;

	public GameObject death;

	public GameObject levelSelect;

	public GameObject win;

	public GameObject pause;

	public Slider musicSlider;

	public Slider sfxSlider;

	public static Lobby Instance
	{
		get;
		private set;
	}

	private void Awake()
	{
		Instance = this;
		StartApplication();
	}

	public void PlayerDied()
	{
		if (!win.active)
		{
			death.SetActive(value: true);
		}
	}

	public void RestartRound()
	{
		death.SetActive(value: false);
		win.SetActive(value: false);
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	public void GoToMenu()
	{
		Game.Instance.EndGame();
		death.SetActive(value: false);
		win.SetActive(value: false);
		menu.SetActive(value: true);
		SceneManager.LoadScene("Lobby");
	}

	public void SelectLevel(int l)
	{
		levelSelect.SetActive(value: false);
		Game.Instance.StartGame();
		SceneManager.LoadScene("Stage" + l);
	}

	public void NextLevel()
	{
		win.SetActive(value: false);
		Game.Instance.StartGame();
		int num = SceneManager.GetActiveScene().buildIndex - 1;
		if (num > 7)
		{
			GoToMenu();
		}
		else
		{
			SceneManager.LoadScene("Stage" + num);
		}
	}

	public void WinStage()
	{
		if (!death.active)
		{
			win.SetActive(value: true);
		}
	}

	public void ExitGame()
	{
		Application.Quit(0);
	}

	public void StartApplication()
	{
		SceneManager.LoadScene("Lobby");
	}

	public void Pause()
	{
		MonoBehaviour.print("pausing");
		if (!death.active && !menu.active && !win.active)
		{
			pause.SetActive(value: true);
			Game.Instance.paused = true;
			Time.timeScale = 0f;
			MonoBehaviour.print("pausing complete");
		}
	}

	public void UnPause()
	{
		pause.SetActive(value: false);
		Game.Instance.paused = false;
		Time.timeScale = 1f;
		MonoBehaviour.print("unpausing complete");
	}

	public void UpdateVolumeM()
	{
		float value = musicSlider.value;
		float value2 = sfxSlider.value;
		AudioManager.Instance.SetVolumeSfx(value2);
		AudioManager.Instance.SetVolumeMusic(value);
	}
}
