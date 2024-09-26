using UnityEngine;

public class Game : MonoBehaviour
{
	public bool started;

	public bool paused;

	public static Game Instance
	{
		get;
		private set;
	}

	private void Awake()
	{
		Instance = this;
	}

	public void StartGame()
	{
		started = true;
	}

	public void EndGame()
	{
		started = false;
	}

	public void PlayerDied()
	{
		Lobby.Instance.PlayerDied();
	}
}
