using System.Collections.Generic;
using UnityEngine;

public class WinCondition : MonoBehaviour
{
	private int kills;

	public int desiredKills;

	private List<Collider2D> gunc;

	public static WinCondition Instance
	{
		get;
		set;
	}

	private void Awake()
	{
		Instance = this;
		kills = 0;
		desiredKills = 0;
		gunc = new List<Collider2D>();
	}

	public void AddEnemy()
	{
		kills++;
		if (kills >= desiredKills)
		{
			Lobby.Instance.WinStage();
		}
	}

	public void AddGunCollider(Collider2D c)
	{
		gunc.Add(c);
	}

	public List<Collider2D> GetGunColliders()
	{
		return gunc;
	}

	public void IncrementDesired()
	{
		desiredKills++;
		MonoBehaviour.print(desiredKills);
	}
}
